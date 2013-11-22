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
#if !W8CORE && NET35Plus

namespace SharpDX.Toolkit
{
    /// <summary>
    /// A <see cref="GameContextWpf"/> to use for rendering to an existing <see cref="SharpDXElement"/>.
    /// </summary>
    /// <remarks>This class was added to avoid WPF references in the projects using WinForms only.</remarks>
    public class GameContextWpf : GameContext 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameContextWpf" /> class from a WPF <see cref="SharpDXElement"/>.
        /// </summary>
        /// <param name="element">The WPF element on which surface scene will be presented.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public GameContextWpf(SharpDXElement element, int requestedWidth = 0, int requestedHeight = 0)
            : base(element, requestedWidth, requestedHeight)
        {
        }
    }
}
#endif