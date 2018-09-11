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
using System.Runtime.InteropServices;

namespace SharpDX.Animation
{
    public partial class Storyboard
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Storyboard"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public Storyboard(Manager manager)
        {
            manager.CreateStoryboard(this);
        }

        /// <summary>
        /// Sets the tag.
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <param name="id">The id.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IUIAnimationStoryboard::SetTag([In, Optional] void* object,[In] unsigned int id)</unmanaged>
        public void SetTag(object @object, int id)
        {
            // Free any previous tag set
            IntPtr tagObjectPtr = IntPtr.Zero;
            int previousId;
            GetTag(out tagObjectPtr, out previousId);
            if (tagObjectPtr != IntPtr.Zero)
                GCHandle.FromIntPtr(tagObjectPtr).Free();

            SetTag(GCHandle.ToIntPtr(GCHandle.Alloc(@object)), id);
        }

        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <param name="id">The id.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IUIAnimationStoryboard::GetTag([Out, Optional] void** object,[Out, Optional] unsigned int* id)</unmanaged>
        public void GetTag(out object @object, out int id)
        {
            IntPtr tagObjectPtr;
            GetTag(out tagObjectPtr, out id);
            @object = GCHandle.FromIntPtr(tagObjectPtr).Target;
        }
    }
}