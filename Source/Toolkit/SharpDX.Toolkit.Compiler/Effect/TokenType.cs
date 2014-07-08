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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Type of a token language.
    /// </summary>
    internal enum TokenType
    {
        /// <summary>
        /// A Newline.
        /// </summary>
        Newline,

        /// <summary>
        /// An identifier.
        /// </summary>
        Identifier,

        /// <summary>
        /// A number in hexadecimal form.
        /// </summary>
        Hexa,

        /// <summary>
        /// A number.
        /// </summary>
        Number,

        /// <summary>
        /// The symbol '='.
        /// </summary>
        Equal,

        /// <summary>
        /// A comma ','.
        /// </summary>
        Comma,

        /// <summary>
        /// A Semicolon ';'.
        /// </summary>
        SemiColon,

        /// <summary>
        /// A left curly brace '{'.
        /// </summary>
        LeftCurlyBrace,

        /// <summary>
        /// A right curly brace '}'.
        /// </summary>
        RightCurlyBrace,

        /// <summary>
        /// A left parenthesis '('.
        /// </summary>
        LeftParent,

        /// <summary>
        /// A right parenthesis ')'.
        /// </summary>
        RightParent,

        /// <summary>
        /// A left bracket '['.
        /// </summary>
        LeftBracket,

        /// <summary>
        /// A right bracket ']'.
        /// </summary>
        RightBracket,

        /// <summary>
        /// A string.
        /// </summary>
        String,

        /// <summary>
        /// A preprocessor token '#'
        /// </summary>
        Preprocessor,

        /// <summary>
        /// A double colon '::'.
        /// </summary>
        DoubleColon,

        /// <summary>
        /// A dot '.'.
        /// </summary>
        Dot,

        /// <summary>
        /// A '&lt;'.
        /// </summary>
        LessThan,

        /// <summary>
        /// A '&gt;'.
        /// </summary>
        GreaterThan,
        
        /// <summary>
        /// An unknown symbol.
        /// </summary>
        Unknown,

        /// <summary>
        /// A end of file token.
        /// </summary>
        EndOfFile,
    }
}