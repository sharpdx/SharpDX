// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
        [Option("Git label", Required = true)]
        public string Label;

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
            gitInfo.FileName = @"C:\Program Files\Git\bin\git.exe";

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

            var commitProjectOverrides = new Dictionary<string, string>();

            commitProjectOverrides["a61d092efc6ea139178e01dbb9ffeb4c1f502357"] = "SharpDX";
            commitProjectOverrides["bd046efd09852260e32c278e5296046acb6ef2cc"] = "SharpDX";
            commitProjectOverrides["4e21a6a89f0eb1563a21fc3bb80db309bd825a3e"] = "SharpDX";
            commitProjectOverrides["14b00f0edd445c35e379cd4cc840787b1678e24f"] = "SharpDX";
            commitProjectOverrides["a7aa982b473b8939f44eef3957db6f754c9a08dd"] = "SharpDX";
            commitProjectOverrides["12bfcc4c818045e87aee27ad8f7563c09de0c741"] = "SharpDX";
            commitProjectOverrides["117c159109661d9003ad1e2d865755d8f802f4d6"] = "SharpDX";
            commitProjectOverrides["9047e9ee7f18c2faa658a3ef17bb51ee5d8dd39f"] = "SharpDX";
            commitProjectOverrides["33c9e25001169b94abebcae9b82d5a994ea71298"] = "SharpDX";
            commitProjectOverrides["5fc7582a3f2e5dfbc58a7637789451c682b6340b"] = "SharpDX";
            commitProjectOverrides["10b74fc23e7b5014e898854997420bbaa77dcf28"] = "SharpDX";
            commitProjectOverrides["23cd1f19429d06030cd78700d1a193ff0a4cdd37"] = "SharpDX";
            commitProjectOverrides["c47d474e9793cc4b6e7e92d50e2dedcbd4640b3e"] = "SharpDX";
            commitProjectOverrides["bd177814374d048b01fe468cad4afab9f5527420"] = "SharpDX";
            commitProjectOverrides["92b517be71380906a4f52fbefd4028516c21b394"] = "SharpDX";
            commitProjectOverrides["580c19c8a94324d613684723da9951a7a6262a19"] = "SharpDX";
            commitProjectOverrides["8a6fb84deb1ee2a1d7f6124523fb39c81af08881"] = "SharpDX";
            commitProjectOverrides["394b07434789e070827766ebcc36f4c2a6045e46"] = "SharpDX";
            commitProjectOverrides["6d69e823dc02c1296de383a61edd64ca5145bcd4"] = "SharpDX";
            commitProjectOverrides["659423aa3e9e97b5e505c44a47ac62f9e068c633"] = "SharpDX";
            commitProjectOverrides["7bff1128a8e44ab466019c3c6890fbfed386a211"] = "SharpDX";
            commitProjectOverrides["6dd4020bd0f1e423758ce2763fef0bda1c0d5db5"] = "SharpDX";
            commitProjectOverrides["2c5205905c027e665c71ed0af8b400f7d8661e30"] = "SharpDX";
            commitProjectOverrides["e293a565b469810220f593ff9b79481499ef7a54"] = "SharpDX";
            commitProjectOverrides["73142aecd1b20619d0cdc1147cd572ddaea83cbb"] = "SharpDX";
            commitProjectOverrides["e77cdc8d10a2b2dcfba3ada82ffa2d24e1a8b9e1"] = "SharpDX";
            commitProjectOverrides["2f1fc0f67b8600554b7e99a6ababc74dac29a9ee"] = "SharpDX";
            commitProjectOverrides["1fa84a7f59616b7464b9e0028b52247d5777d8f9"] = "SharpDX";
            commitProjectOverrides["69eb4ac6701186c123f3a740aa02b0306de08e51"] = "SharpDX";
            commitProjectOverrides["bdee25f335797322bb4e32856a12a81ad1ab000a"] = "SharpDX";
            commitProjectOverrides["207ae613c8b207c661dc0c9536e63f4514a0fbab"] = "SharpDX";
            commitProjectOverrides["ee54737e247aabf3a268594b4027fe7c7cd7831d"] = "SharpDX";

            commitProjectOverrides["d3bf2518356b38d649e3705a29d955351a887b57"] = "DirectWrite";
            commitProjectOverrides["6f89c152daed03108937e25dd5540d142adc8dab"] = "DirectWrite";

            commitProjectOverrides["aa9e8239a59344dc13e2820fda8ad1a7aa816793"] = "Direct3D11";
            commitProjectOverrides["f5f7c3573cd5531c6f956092cc73195130714555"] = "Direct3D11";

            commitProjectOverrides["feeaebf9a057e148edc4a009376646b502e573b8"] = "Direct2D1";

            commitProjectOverrides["2a0a4fc930c5a4e38aeea2b2196cfd2f3a157048"] = "Toolkit";

            commitProjectOverrides["da3745a50112b08dfa8dd53d6a52fe5a07518345"] = "Toolkit.Graphics";
            commitProjectOverrides["85c8a0ff0e6d8228430a908f7b8fd2f09286b461"] = "Toolkit.Graphics";
            commitProjectOverrides["70e19f85330158fbfe4b80fb9b9e5c735946b4bd"] = "Toolkit.Graphics";
            commitProjectOverrides["e20824b26497066d323a1ae114da1078b52e5de0"] = "Toolkit.Graphics";
            commitProjectOverrides["6f40f8e97edda35825001ca1a91c3df46d959435"] = "Toolkit.Graphics";
            commitProjectOverrides["5cabe6933d70fa85340cb6317ddbba9bb236f28f"] = "Toolkit.Graphics";
            commitProjectOverrides["acdad151011376b9566a3a4fdc62b421518ac14f"] = "Toolkit.Graphics";
            commitProjectOverrides["ea35b53150f933e1e0c8d28f0b0e5e323330fc53"] = "Toolkit.Graphics";
            commitProjectOverrides["8aa0ab452e23e52401d81a4f92cb5facba12c80b"] = "Toolkit.Graphics";
            commitProjectOverrides["01b4da7116f1dc99973030efe153f9eaec4d5e69"] = "Toolkit.Graphics";
            commitProjectOverrides["0094bbb37c8f627a822fed2b10e9c4a724dfcfef"] = "Toolkit.Graphics";
            commitProjectOverrides["615f071746bf3988e4a64ada547b9b6c3f8b43f3"] = "Toolkit.Graphics";
            commitProjectOverrides["39f3b633ce66c270b1326715e6bd80746efde534"] = "Toolkit.Graphics";
            commitProjectOverrides["30c160ed716edc4d738ef8db9eeaba3bcb0f22fa"] = "Toolkit.Graphics";
            commitProjectOverrides["40fae182c06ea1d94092edf4d63f67453203c498"] = "Toolkit.Graphics";

            commitProjectOverrides["7bff1128a8e44ab466019c3c6890fbfed386a211"] = "Toolkit.Compiler";
            commitProjectOverrides["e1acdb38d72ce19cabda578076f48fc1da30ee7f"] = "Toolkit.Compiler";
            commitProjectOverrides["d255e50cf6769f46f142f9b1c32d330d3524d165"] = "Toolkit.Compiler";

            commitProjectOverrides["2f54ca695f0e5ae2b3dccded50cab1c0dccc5f09"] = "Toolkit.Input";
            commitProjectOverrides["95770c51df1555540de5e4561afa186e87013c79"] = "Toolkit.Input";
            commitProjectOverrides["df6d4e9ac8236eff19bbff6a532b3892e3059951"] = "Toolkit.Input";
            commitProjectOverrides["8b99acc728b9cd83dd39e34f4930143517b797d8"] = "Toolkit.Input";
            commitProjectOverrides["bf198f1e0d7ed300b7bd690b9775a9c81c1c064a"] = "Toolkit.Input";
            commitProjectOverrides["f66de0625f7498d17774ecb08e9e53a438ad0a66"] = "Toolkit.Input";

            commitProjectOverrides["8974e7f3bee9779dc2208fcb934160772c8b8602"] = "Samples";
            commitProjectOverrides["372e73f1342180f5b306c44efc0584c7a29d527c"] = "Samples";

            commitProjectOverrides["19572a16c003c72bb94baf9f5cf1cb2bddea998e"] = "Build";
            commitProjectOverrides["84f11be8199d2f72d28d5e0e388032cec12633d3"] = "Build";
            commitProjectOverrides["dbaa8335bb0472b5841f931067ec54b5987ecd3a"] = "Build";
            commitProjectOverrides["55a31b00cd41eb7567deb0d6c071ff5ea1ab4085"] = "Build";
            commitProjectOverrides["31abcc470cebc74bdc7cb92ad2fbf7819306cd2e"] = "Build";
            commitProjectOverrides["b00be2e5492b86a3ee3718adb4b3325dcb572e78"] = "Build";
            commitProjectOverrides["24889e6d3807cf96f37f18272225dac6a7c303c5"] = "Build";
            commitProjectOverrides["1a0626bf28f4087ae42448ef49d14000de4cadb0"] = "Build";
            commitProjectOverrides["681a54e26ffb4c636e4835d551e61ef71e7ed296"] = "Build";

            commitProjectOverrides["87beff5928157d04a08cf80d428eb758a31b56ab"] = "WIC";
            commitProjectOverrides["cd1b972be4c2ee06f733dc77c466cff1280a88eb"] = "WIC";
            commitProjectOverrides["2359346b1077e41bf2dc2d8fc2741e75ccca2abb"] = "WIC";
            commitProjectOverrides["9f318d9a4e9e2ac009fc2de14dba63bf2c26d759"] = "WIC";
            commitProjectOverrides["d742d444fc1dfad934a0e708bb23b011220a75b0"] = "WIC";
            commitProjectOverrides["524969cbaa982a43d4b96d441e361a0375ddc5d5"] = "WIC";

            var projectOverrides = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            projectOverrides[string.Empty] = "Misc";
            projectOverrides["Toolkit.input"] = "Toolkit.Input";
            projectOverrides["Toolkit.WP8"] = "WP8";
            projectOverrides["SharpDX.MathUtil"] = "SharpDX";
            projectOverrides["Samples WP8"] = "WP8";
            projectOverrides["Samples.Win8"] = "Samples";
            projectOverrides["Direct3D11/WP8Sample"] = "Samples";
            projectOverrides["DirectWrite.Samples"] = "Samples";
            projectOverrides["SharpDX.Samples"] = "Samples";
            projectOverrides["SharpDX.Toolkit.Samples"] = "Toolkit.Samples";
            projectOverrides["Toolkit.WPF"] = "Toolkit";
            projectOverrides["WP8 Sample"] = "WP8";
            projectOverrides["Sample"] = "Samples";
            projectOverrides["Direct3D11.Effects"] = "Direct3D11";
            projectOverrides["DirectX11"] = "Direct3D11";
            projectOverrides["SharpDX.Core"] = "SharpDX";
            projectOverrides["SharpDX.Direct2D1"] = "Direct2D1";
            projectOverrides["SharpDX.Direct3D11"] = "Direct3D11";
            projectOverrides["SharpDX.DirectWrite"] = "DirectWrite";
            projectOverrides["SharpDX.WIC"] = "WIC";
            projectOverrides["SharpDX.DXGI"] = "DXGI";
            projectOverrides["SharpDX.Compiler"] = "D3DCompiler";
            projectOverrides["SharpDX.Tests"] = "Tests";
            projectOverrides["SharpDX.Toolkit"] = "Toolkit";
            projectOverrides["SharpDX.Toolkit.Compiler"] = "Toolkit.Compiler";
            projectOverrides["SharpDX.Toolkit.Graphics"] = "Toolkit.Graphics";
            projectOverrides["SharpDX.Toolkit.Input"] = "Toolkit.Input";

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

                if (commitProjectOverrides.ContainsKey(commitId))
                    project = commitProjectOverrides[commitId];

                if (projectOverrides.ContainsKey(project))
                    project = projectOverrides[project];

                List<Tuple<string, string>> commitsPerProject;
                if (!commits.TryGetValue(project, out commitsPerProject))
                {
                    commitsPerProject = new List<Tuple<string, string>>();
                    commits[project] = commitsPerProject;
                }

                commitsPerProject.Add(new Tuple<string, string>(commitId, content));
            }

            var writer = (OutputFile != null) ? new StreamWriter(new FileStream(OutputFile, FileMode.Create, FileAccess.Write)) : Console.Out;

            var regexIssue = new Regex(@"issue\s+#(\d+)");
            var mergeBranchRegex = new Regex("Merge( branch)?( '.+?')?");

            var keys = commits.Keys.ToList();
            keys.Sort();
            foreach (var key in keys)
            {
                writer.WriteLine("## {0}", key);

                var values = commits[key];
                values.Sort((left, right) => left.Item2.CompareTo(right.Item2));
                var groupedValues = values.GroupBy(x => x.Item2);
                writer.WriteLine();

                foreach (var groupedValue in groupedValues)
                {
                    if (mergeBranchRegex.IsMatch(groupedValue.Key))
                        continue;

                    var message = EscapeHtml(groupedValue.Key);
                    var changesets = groupedValue.Select(x => x.Item1).ToList();

                    const string issueUrlGoogleCode = "http://code.google.com/p/sharpdx/issues/detail?id=$1";
                    const string issueUrlGithub = "https://github.com/sharpdx/SharpDX/issues/$1";

                    var issueMatches = regexIssue.Matches(message);
                    if (issueMatches.Count > 0)
                    {
                        // voodoo magic:
                        var issueNumber = int.Parse(issueMatches[0].Value.Split('#')[1].Trim());

                        var issueUrl = issueNumber > 133 ? issueUrlGoogleCode : issueUrlGithub;

                        message = regexIssue.Replace(message, $"[$0]({issueUrl})");
                    }

                    writer.Write($"  - {message} (");

                    //const string changesetUrl = "http://code.google.com/p/sharpdx/source/detail?r={0}";
                    const string changesetUrl = "https://github.com/sharpdx/SharpDX/commit/";

                    if (changesets.Count == 1)
                    {
                        writer.Write($"[changes]({changesetUrl}{changesets[0]})");
                    }
                    else
                    {
                        writer.Write("changes: ");
                        for (var i = 0; i < changesets.Count; i++)
                        {
                            if (i != 0)
                                writer.Write(", ");

                            writer.Write($"[{changesets[i]}]({changesetUrl}{changesets[i]})");
                        }
                    }

                    writer.WriteLine(")");
                }

                writer.WriteLine();
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
