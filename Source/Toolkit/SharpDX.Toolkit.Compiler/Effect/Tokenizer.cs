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

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A simple tokenizer used to transform a HLSL sourcecode into a collection of tokens.
    /// </summary>
    /// <remarks>
    /// This tokenizer is used to parse tokens inside technique/pass block. 
    /// See <see cref="TokenType"/> for the list of tokens that are supported inside technique/pass.
    /// This tokenizer is not really efficient compare to a DFA (Deterministic Finite-state Automaton) 
    /// parser but enough suitable in our case (120 files from DirectX SDK parsed in 2s).
    /// </remarks>
    internal class Tokenizer
    {
        private static readonly Regex RegexTokenizer = new Regex(
            @"(?<ws>[ \t]+)|" +
            @"(?<nl>(?:\r\n|\n))|" +
            @"(?<ident>[a-zA-Z_][a-zA-Z0-9_]*)|" +
            @"(?<hexa>0x[0-9a-fA-F]+)|" +
            @"(?<number>[\-\+]?\s*[0-9]*\.?[0-9]+(?:[eE][-+]?[0-9]+)?f?)|" +
            @"(?<equal>=)|" +
            @"(?<comma>,)|" +
            @"(?<semicolon>;)|" +
            @"(?<lcb>\{)|" +
            @"(?<rcb>\})|" +
            @"(?<lpar>\()|" +
            @"(?<rpar>\))|" +
            @"(?<lb>\[)|" +
            @"(?<rb>\])|" +
            @"(?<str>""[^""\\]*(?:\\.[^""\\]*)*"")|" +
            @"(?<prep>#)|" +
            @"(?<doublecolon>::)|" +
            @"(?<dot>\.)|" +
            @"(?<lt>\<)|" +
            @"(?<gt>\>)|" +
            @"(?<unk>[^\s]+)",
            RegexOptions.Compiled
            );

        /// <summary>
        /// Runs the tokenizer on an input string.
        /// </summary>
        /// <param name="input">The string to decode to tokens.</param>
        /// <returns>An enumeration of tokens.</returns>
        public static IEnumerable<Token> Run(string input)
        {
            var matches = RegexTokenizer.Matches(input);
            foreach (Match match in matches)
            {
                int i = 0;
                foreach (Group group in match.Groups)
                {
                    string matchValue = group.Value;
                    // Skip whitespaces
                    if (group.Success && i > 1)
                    {
                        yield return new Token {Type = (TokenType) (i - 2), Value = matchValue, Span = {Index = @group.Index, Length = @group.Length}};
                    }
                    i++;
                }
            }            
        }
    }
}