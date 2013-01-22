// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
namespace SharpCore
{
    /// <summary>
    /// A DocProvider is responsible to provide documentation to the Parser
    /// in order to feed each C++ element with an associated documentation.
    /// This is optional.
    /// A client of Parser API could provide a documentation provider
    /// in an external assembly.
    /// </summary>
    public interface DocProvider
    {
        /// <summary>
        /// Begins the process of the documentation provider
        /// </summary>
        void Begin();

        /// <summary>
        /// Finds the documentation for a particular C++ item.
        /// </summary>
        /// <param name="fullName">The full name. for top level elements (like struct, interfaces, enums, functions), It's the name itself of the element. For interafce methods, the name is passeed like this "IMyInterface::MyMethod".</param>
        /// <returns></returns>
        DocItem FindDocumentation(string fullName);

        /// <summary>
        // Ends the process of the documentation provider
        /// </summary>
        void End();
    }
}