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
using System.Text.RegularExpressions;
using SharpGen.CppModel;

namespace SharpGen.Parser
{
    /// <summary>
    /// This class is responsible to get all macros defined from a C++ header file.
    /// </summary>
    public class MacroManager
    {
        private static readonly Regex MatchIncludeLine = new Regex(@"^\s*#\s+\d+\s+""([^""]+)""", RegexOptions.Compiled);
        private static readonly Regex MatchDefine = new Regex(@"^\s*#define\s+([a-zA-Z_][\w_]*)\s+(.*)", RegexOptions.Compiled);
        private readonly CastXml _gccxml;
        private Dictionary<string, string> _currentMacros = null;
        private readonly Dictionary<string, Dictionary<string, string>> _mapIncludeToMacros = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroManager"/> class.
        /// </summary>
        /// <param name="gccxml">The GccXml parser.</param>
        public MacroManager(CastXml gccxml)
        {
            _gccxml = gccxml;
        }

        /// <summary>
        /// Parses the specified C++ header file and fills the <see cref="CppModule"/> with defined macros.
        /// </summary>
        /// <param name="file">The C++ header file to parse.</param>
        /// <param name="group">The CppIncludse object to fill with macro definitions.</param>
        public void Parse(string file, CppModule group)
        {
            _gccxml.Preprocess(file, ParseLine);

            foreach (var includeName in _mapIncludeToMacros.Keys)
            {
                var includeId = Path.GetFileNameWithoutExtension(includeName).ToLower();
                var include = group.FindInclude(includeId);
                if (include == null)
                {
                    include = new CppInclude() { Name = includeId };
                    group.Add(include);
                }
                foreach (var macroDefinition in _mapIncludeToMacros[includeName])
                    include.Add(new CppDefine(macroDefinition.Key, macroDefinition.Value));
            }
        }

        /// <summary>
        /// Parses a macro definition line.
        /// </summary>
        /// <param name="sendingProcess">The sending process.</param>
        /// <param name="outLine">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void ParseLine(object sendingProcess, DataReceivedEventArgs outLine)
        {
            string line = outLine.Data;

            // Collect the sort command output.
            if (!String.IsNullOrEmpty(line))
            {
                Match result = MatchIncludeLine.Match(line);
                if (result.Success)
                {
                    if (result.Groups[1].Value.StartsWith("<"))
                        _currentMacros = null;
                    else
                    {
                        var currentFile = Path.GetFileName(result.Groups[1].Value).ToLower();
                        if (!_mapIncludeToMacros.TryGetValue(currentFile, out _currentMacros))
                        {
                            _currentMacros = new Dictionary<string,string>();
                            _mapIncludeToMacros.Add(currentFile, _currentMacros);
                        }
                    }
                }
                else if (_currentMacros != null)
                {                    
                    result = MatchDefine.Match(line);
                    if (result.Success)
                    {
                        string value = result.Groups[2].Value.TrimEnd();
                        if (!string.IsNullOrEmpty(value))
                        {
                            _currentMacros.Remove(result.Groups[1].Value);
                            _currentMacros.Add(result.Groups[1].Value, value);
                        }
                    }
                }
            }
        }
    }
}