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
#if DIRECTX11_1
using System;

namespace SharpDX.MediaFoundation
{
    public partial class Activate
    {
        /// <summary>	
        /// Creates an activation object for a Windows Runtime class.
        /// </summary>	
        /// <param name="activatableClassId"><dd> <p>The class identifier that is associated with the activatable runtime class.</p> </dd></param>	
        /// <param name="propertySet"><dd> <p>An optional friendly name for the activation object. The friendly name is stored in the object's <see cref="SharpDX.MediaFoundation.TransformAttributeKeys.MftFriendlyNameAttribute"/> attribute. This parameter can be <strong><c>null</c></strong>.</p> </dd></param>	
        /// <remarks>	
        /// <p>To create the Windows Runtime object, call <strong><see cref="SharpDX.MediaFoundation.Activate.ActivateObject"/></strong> or <strong>IClassFactory::CreateInstance</strong>.</p>	
        /// </remarks>	
        /// <msdn-id>hh162753</msdn-id>	
        /// <unmanaged>HRESULT MFCreateMediaExtensionActivate([In] const wchar_t* szActivatableClassId,[In, Optional] IUnknown* pConfiguration,[In] const GUID&amp; riid,[Out] void** ppvObject)</unmanaged>	
        /// <unmanaged-short>MFCreateMediaExtensionActivate</unmanaged-short>	
        public Activate(string activatableClassId, ComObject propertySet = null)
        {
            IntPtr temp;
            MediaFactory.CreateMediaExtensionActivate(activatableClassId, propertySet, Utilities.GetGuidFromType(typeof (Activate)), out temp);
            NativePointer = temp;
        }
    }
}
#endif