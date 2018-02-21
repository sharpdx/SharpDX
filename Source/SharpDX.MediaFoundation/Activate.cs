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
using System.Reflection;

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

        /// <summary>	
        /// <p> Creates the object associated with this activation object. </p>	
        /// </summary>	
        /// <param name="riid"><dd> <p> Interface identifier (IID) of the requested interface. </p> </dd></param>	
        /// <returns><dd> <p> A reference to the requested interface. The caller must release the interface. </p> </dd></returns>	
        /// <remarks>	
        /// <p>Some Microsoft Media Foundation objects must be shut down before being released. If so, the caller is responsible for shutting down the object that is returned in <em>ppv</em>. To shut down the object, do one of the following:</p><ul> <li>Call <strong><see cref="SharpDX.MediaFoundation.Activate.ShutdownObject"/></strong> on the activation object, or</li> <li>Call the object-specific shutdown method. This method will depend on the type of object. Possibilities include:<ul> <li>Media sources: Call <strong><see cref="SharpDX.MediaFoundation.MediaSource.Shutdown"/></strong>.</li> <li>Media sinks: Call <strong><see cref="SharpDX.MediaFoundation.MediaSink.Shutdown"/></strong>.</li> <li>Any object that supports the <strong><see cref="SharpDX.MediaFoundation.Shutdownable"/></strong> interface: Call <strong><see cref="SharpDX.MediaFoundation.Shutdownable.Shutdown"/></strong>.</li> </ul> </li> </ul><p>The <strong><see cref="SharpDX.MediaFoundation.Activate.ShutdownObject"/></strong> method is generic to all object types. If the object does not require a shutdown method, <strong>ShutdownObject</strong> succeeds and has no effect. If you do not know the specific shutdown method for the object (or do not know the object type), call <strong><see cref="SharpDX.MediaFoundation.Activate.ShutdownObject"/></strong>.</p><p> After the first call to <strong>ActivateObject</strong>, subsequent calls return a reference to the same instance, until the client calls either <strong>ShutdownObject</strong> or <strong><see cref="SharpDX.MediaFoundation.Activate.DetachObject"/></strong>. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFActivate::ActivateObject']/*"/>	
        /// <msdn-id>ms694292</msdn-id>	
        /// <unmanaged>HRESULT IMFActivate::ActivateObject([In] const GUID&amp; riid,[Out] void** ppv)</unmanaged>	
        /// <unmanaged-short>IMFActivate::ActivateObject</unmanaged-short>	
        public T ActivateObject<T>(System.Guid riid) where T : SharpDX.ComObject
        {
            unsafe
            {
                IntPtr objectRef;
                ActivateObject(riid, out objectRef);
                return ComObject.FromPointer<T>(objectRef);
            }
        }

        /// <summary>	
        /// <p> Creates the object associated with this activation object. Riid is provided via reflection on the COM object type </p>	
        /// </summary>
        /// <returns><dd> <p> A reference to the requested interface. The caller must release the interface. </p> </dd></returns>	
        /// <remarks>	
        /// <p>Some Microsoft Media Foundation objects must be shut down before being released. If so, the caller is responsible for shutting down the object that is returned in <em>ppv</em>. To shut down the object, do one of the following:</p><ul> <li>Call <strong><see cref="SharpDX.MediaFoundation.Activate.ShutdownObject"/></strong> on the activation object, or</li> <li>Call the object-specific shutdown method. This method will depend on the type of object. Possibilities include:<ul> <li>Media sources: Call <strong><see cref="SharpDX.MediaFoundation.MediaSource.Shutdown"/></strong>.</li> <li>Media sinks: Call <strong><see cref="SharpDX.MediaFoundation.MediaSink.Shutdown"/></strong>.</li> <li>Any object that supports the <strong><see cref="SharpDX.MediaFoundation.Shutdownable"/></strong> interface: Call <strong><see cref="SharpDX.MediaFoundation.Shutdownable.Shutdown"/></strong>.</li> </ul> </li> </ul><p>The <strong><see cref="SharpDX.MediaFoundation.Activate.ShutdownObject"/></strong> method is generic to all object types. If the object does not require a shutdown method, <strong>ShutdownObject</strong> succeeds and has no effect. If you do not know the specific shutdown method for the object (or do not know the object type), call <strong><see cref="SharpDX.MediaFoundation.Activate.ShutdownObject"/></strong>.</p><p> After the first call to <strong>ActivateObject</strong>, subsequent calls return a reference to the same instance, until the client calls either <strong>ShutdownObject</strong> or <strong><see cref="SharpDX.MediaFoundation.Activate.DetachObject"/></strong>. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFActivate::ActivateObject']/*"/>	
        /// <msdn-id>ms694292</msdn-id>	
        /// <unmanaged>HRESULT IMFActivate::ActivateObject([In] const GUID&amp; riid,[Out] void** ppv)</unmanaged>	
        /// <unmanaged-short>IMFActivate::ActivateObject</unmanaged-short>	
        public T ActivateObject<T>() where T : SharpDX.ComObject
        {
            unsafe
            {
                IntPtr objectRef;
                Guid riid = typeof(T).GetTypeInfo().GUID;
                ActivateObject(riid, out objectRef);
                return ComObject.FromPointer<T>(objectRef);
            }
        }
    }
}