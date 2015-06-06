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

namespace SharpDX.Direct3D12
{
    public partial struct ResourceTransitionBarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceTransitionBarrier"/> struct.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="stateBefore">The state before.</param>
        /// <param name="stateAfter">The state after.</param>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        public ResourceTransitionBarrier(Resource resource, ResourceStates stateBefore, ResourceStates stateAfter)
            : this(resource, -1, stateBefore, stateAfter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceTransitionBarrier" /> struct.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="subresource">The subresource.</param>
        /// <param name="stateBefore">The state before.</param>
        /// <param name="stateAfter">The state after.</param>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        public ResourceTransitionBarrier(Resource resource, int subresource, ResourceStates stateBefore, ResourceStates stateAfter)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            ResourcePointer = resource.NativePointer;
            Subresource = subresource;
            StateBefore = stateBefore;
            StateAfter = stateAfter;
        }
    }
}