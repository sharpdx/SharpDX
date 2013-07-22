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

using System.IO;

namespace SharpDX.Toolkit.Content
{
    /// <summary>
    /// A content reader is in charge of reading object data from a stream.
    /// </summary>
    public interface IContentReader
    {
        /// <summary>
        /// Reads the content of a particular data from a stream.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        /// <param name="assetName">The name of the asset associated with the stream.</param>
        /// <param name="stream">The steam of the asset to load data from.</param>
        /// <param name="keepStreamOpen"><c>true</c> to keep the stream opened after the content was read, otherwise the stream will be closed after if this content reader succeeded to read the data.</param>
        /// <param name="options">The options passed to the content manager.</param>
        /// <returns>The data decoded from the stream, or null if the kind of asset is not supported by this content reader.</returns>
        object ReadContent(IContentManager contentManager, string assetName, Stream stream, out bool keepStreamOpen, object options);
    }
}