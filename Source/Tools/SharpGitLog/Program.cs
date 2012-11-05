// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpGitLog
{
    class Program : SharpDX.ConsoleProgram
    {
        [Option("Git label", Required = true)] public string Label;

        [Option("I", Description = "Git directory (default: ./)", Value = "<directory>")]
        public string Directory = ".";

        [Option("O", Description = "Destination file output : (default is stdout)", Value = "<file>")]
        public string OutputFile;

        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public void Run(string[] args)
        {
            // Print the exe header
            PrintHeader();

            // Parse the command line
            if (!ParseCommandLine(args))
                Environment.Exit(-1);

            var gitInfo = new ProcessStartInfo();
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardOutput = true;
            gitInfo.UseShellExecute = false;
            gitInfo.FileName = @"C:\Program Files (x86)\Git\bin\git.exe";

            Process gitProcess = new Process();
            gitInfo.Arguments = string.Format(@"log {0}.. --format=""%H %s""", Label);
            gitInfo.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, Directory ?? string.Empty);

            if (!System.IO.Directory.Exists(gitInfo.WorkingDirectory))
            {
                ShowError("Directory [{0}] not found ", gitInfo.WorkingDirectory);
                Environment.Exit(0);
            }


            gitProcess.StartInfo = gitInfo;
            gitProcess.Start();

            string output = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT

            gitProcess.WaitForExit();

            if (gitProcess.ExitCode != 0)
            {
                ShowError("Error while running git command [{0}]", gitProcess.ExitCode);
                Environment.Exit(0);
            }

            gitProcess.Close();

            var reader = new StringReader(output);

            var matchProject = new Regex(@"^\[(.*?)\]\s*(.*)");

            var commits = new Dictionary<string, List<Tuple<string, string>>>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var result = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                string project = string.Empty;
                var commitId = result[0];
                var content = result[1];

                var match = matchProject.Match(result[1]);
                if (match.Success)
                {
                    project = match.Groups[1].Value;
                    content = match.Groups[2].Value;
                }

                List<Tuple<string, string>> commitsPerProject;
                if (!commits.TryGetValue(project, out commitsPerProject))
                {
                    commitsPerProject = new List<Tuple<string, string>>();
                    commits[project] = commitsPerProject;
                }

                commitsPerProject.Add(new Tuple<string, string>(commitId, content));
            }

            var writer = (OutputFile != null) ? (TextWriter)new StreamWriter(new FileStream(OutputFile, FileMode.Create, FileAccess.Write)) : Console.Out;

            var regexIssue = new Regex(@"issue\s+#(\d+)");

            var keys = commits.Keys.ToList();
            keys.Sort();
            foreach (var key in keys)
            {
                writer.WriteLine("<h4>{0}</h4>", key);

                var values = commits[key];
                values.Sort((left, right) => left.Item2.CompareTo(right.Item2));
                writer.WriteLine("<ul>");
                foreach (var value in values)
                {
                    var commitDescription = EscapeHtml(value.Item2);
                    commitDescription = regexIssue.Replace(commitDescription, "<a href='http://code.google.com/p/sharpdx/issues/detail?id=$1' target='_blank'>$0</a>");

                    writer.WriteLine("  <li>{0} (<a href='http://code.google.com/p/sharpdx/source/detail?r={1}' target='_blank'>changes</a>)</li>", commitDescription, value.Item1);
                }
                writer.WriteLine("</ul>");
            }

            writer.Flush();
            writer.Close();
        }

        public static string EscapeHtml(string stringToEscape)
        {
            return stringToEscape.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
