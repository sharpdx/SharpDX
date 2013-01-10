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
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
    public class EffectDependencyList : Dictionary<string, DateTime>
    {
        private static Regex MatchLine = new Regex(@"^(.*)\s(\d+)$");

        public EffectDependencyList()
        {
        }

        public static EffectDependencyList FromReader(TextReader textReader)
        {
            var effectDependency = new EffectDependencyList();
            string line;
            while ((line = textReader.ReadLine()) != null)
            {

                var match = MatchLine.Match(line);
                if (match.Success)
                {
                    effectDependency.Add(match.Groups[1].Value, new DateTime(long.Parse(match.Groups[2].Value)));
                }
            }
            return effectDependency;
        }

        public static EffectDependencyList FromStream(Stream textStream)
        {
            var reader = new StreamReader(textStream);
            return FromReader(reader);
        }


        public static EffectDependencyList FromFile(string file)
        {
            using (var stream = new NativeFileStream(file, NativeFileMode.Open, NativeFileAccess.Read)) return FromStream(stream);
        }

        public void Save(TextWriter writer)
        {
            foreach (var value in this)
            {
                writer.WriteLine("{0} {1}", value.Key, value.Value.Ticks);
            }
            writer.Flush();
        }

        public void Save(Stream stream)
        {
            var writer = new StreamWriter(stream);
            Save(writer);            
        }

        public void Save(string file)
        {
            var filePath = typeof(EffectDependencyList).Assembly.Location;
            Remove(filePath);
            Add(filePath, File.GetLastWriteTime(filePath));

            using (var stream = new NativeFileStream(file, NativeFileMode.Create, NativeFileAccess.Write)) Save(stream);
        }

        public bool CheckForChanges()
        {
            foreach (var fileItem in this)
            {
                if (!File.Exists(fileItem.Key))
                {
                    return true;
                }

                var fileTime = File.GetLastWriteTime(fileItem.Key);

                if (fileItem.Value != fileTime)
                {
                    return true;
                }
            }

            return false;
        }
    }
}