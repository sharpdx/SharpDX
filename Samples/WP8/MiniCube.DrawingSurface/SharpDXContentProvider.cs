// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Info;
using SharpDX.Direct3D11;

namespace MiniTriApp
{
    /// <summary>
    /// This is a port of Direct3D C++ WP8 sample. This port is not clean and complete. 
    /// DO NOT USE IT AS A STARTING POINT FOR DEVELOPING A PRODUCTION QUALITY APPLICATION
    /// </summary>
    internal class SharpDXContentProvider : DrawingSurfaceContentProviderNativeBase
    {
        public SharpDXContentProvider(SharpDXInterop controller)
        {
            _controller = controller;

            _controller.RequestAdditionalFrame +=()=>{
                if (_host!=null)
	            {
		            _host.RequestAdditionalFrame();
	            }
            };

            _controller.RecreateSynchronizedTexture +=()=>{
                if (_host!=null)
	            {
		            _synchronizedTexture = _host.CreateSynchronizedTexture(_controller.GetTexture());
	            }
            };

        }

        public override void Connect(DrawingSurfaceRuntimeHost host)
        {
            _host = host;
            _controller.Connect(host);
        }

        public override void Disconnect()
        {
            _controller.Disconnect();
            _host = null;
            SharpDX.Utilities.Dispose(ref _synchronizedTexture);
        }

        public override void PrepareResources(DateTime presentTargetTime, out SharpDX.Bool isContentDirty)
        {
            _controller.PrepareResources(presentTargetTime, out isContentDirty);

        }

        
        public override void GetTexture(SharpDX.Size2F surfaceSize, out DrawingSurfaceSynchronizedTexture synchronizedTexture, out SharpDX.RectangleF textureSubRectangle)
        {
            if (_synchronizedTexture == null)
            {
                _synchronizedTexture = _host.CreateSynchronizedTexture(_controller.GetTexture());
            }
            
            // Set output parameters.
            _textureSubRectangle.Left = 0.0f;
            _textureSubRectangle.Top = 0.0f;
            _textureSubRectangle.Right = surfaceSize.Width;
            _textureSubRectangle.Bottom = surfaceSize.Height;


            synchronizedTexture = _synchronizedTexture;
            textureSubRectangle = _textureSubRectangle;     

            //something is going wrong here as the second time thru the BeginDraw consumes 
            //the call and controlnever returns back to this method, thus GetTexture 
            //(the call after begindraw) never fires again... ??????
            synchronizedTexture.BeginDraw();

            _controller.GetTexture(surfaceSize, synchronizedTexture, textureSubRectangle);

            synchronizedTexture.EndDraw();
        }



	    private readonly SharpDXInterop _controller;
	    DrawingSurfaceRuntimeHost _host;
	    DrawingSurfaceSynchronizedTexture _synchronizedTexture;
        SharpDX.RectangleF _textureSubRectangle;
    }
}
