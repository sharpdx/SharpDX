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

namespace SharpDX.MediaFoundation
{
    public static partial class MediaManager
    {
        private static bool isStartup;

        /// <summary>
        ///   <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Initializes Microsoft Media Foundation.</p>
        /// </summary>
        /// <param name="useLightVersion">If true, do not initialize the sockets library, else full initialization. Default is false</param>
        /// <msdn-id>ms702238</msdn-id>
        /// <unmanaged>HRESULT MFStartup([In] unsigned int Version,[In] unsigned int dwFlags)</unmanaged>
        /// <unmanaged-short>MFStartup</unmanaged-short>
        /// <remarks>
        ///   <p> An application must call this function before using Media Foundation. Before your application quits, call <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Shutdown"/></strong> once for every previous call to <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Startup"/></strong>. </p><p> Do not call <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Startup"/></strong> or <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Shutdown"/></strong> from work queue threads. For more information about work queues, see Work Queues. </p><p>This function is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul>
        /// 		<li>Windows?XP with Service Pack?2 (SP2) and later.</li>
        /// 		<li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li>
        /// 	</ul>
        /// </remarks>
        public static void Startup(bool useLightVersion = false)
        {
            if (isStartup)
                return;
            MediaFactory.Startup(MediaFactory.Version, useLightVersion ? 1 : 0);
            isStartup = true;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Shuts down the Microsoft Media Foundation platform. Call this function once for every call to <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Startup"/></strong>. Do not call this function from work queue threads.</p>	
        /// </summary>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>This function is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms694273</msdn-id>	
        /// <unmanaged>HRESULT MFShutdown()</unmanaged>	
        /// <unmanaged-short>MFShutdown</unmanaged-short>	
        public static void Shutdown()
        {
            if (isStartup)
            {
                MediaFactory.Shutdown();
                isStartup = false;
            }
        }
    }
}