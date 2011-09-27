// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

namespace SharpDX.Diagnostics
{
    /// <summary>
    /// ErrorManager helper to get a description from a HRESULT/SharpDX.Result code.
    /// </summary>
    public partial class ErrorManager
    {
        private static Dictionary<int, ErrorDescription> _map = new Dictionary<int, ErrorDescription>();

        /// <summary>
        /// Error description holder
        /// </summary>
        private class ErrorDescription
        {
            public ErrorDescription(uint code, string codeString, string description)
            {
                Code = code;
                CodeString = codeString;
                Description = description;
            }

            public uint Code;
            public string CodeString;
            public string Description;

            public override string ToString()
            {
                string prefix = unchecked((int) Code) < 0 ? "Error: " : "";
                if (string.IsNullOrEmpty(Description))
                    return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1} (0x{2:X})", prefix, CodeString, Code);
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1} (0x{2:X}) {3}", prefix, CodeString, Code, Description);
            }
        }


        /// <summary>
        /// No instance
        /// </summary>
        private ErrorManager()
        {            
        }

        /// <summary>
        /// Initialize Map
        /// </summary>
        static ErrorManager()
        {
            InitDefaultErrors();
        }

        /// <summary>
        /// Add a message description for a particular code.
        /// </summary>
        /// <param name="code">the error code</param>
        /// <param name="codeString">string representation of the code</param>
        /// <param name="description">description of the error</param>
        private static void Add(uint code, string codeString, string description)
        {
            ErrorDescription errorDescription = new ErrorDescription(code, codeString, description);
            _map.Add(unchecked((int)errorDescription.Code), errorDescription);
        }

        /// <summary>
        /// Returns an error description.
        /// </summary>
        /// <param name="code">An error code</param>
        /// <returns>a description</returns>
        public static string GetErrorMessage(int code)
        {
            ErrorDescription description;
            if (!_map.TryGetValue(code, out description))
            {
                description = new ErrorDescription(unchecked((uint)code), "Unknown", "An unknown error occured");
            }
            return description.ToString();
        }
    }
}