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
// -----------------------------------------------------------------------------
// Original code from SlimDX project.
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using System;
using System.Windows.Forms;
using SharpDX.Windows;

namespace SharpDX.Samples
{
    /// <summary>
    /// Root clas for a Demo Application
    /// </summary>
    public abstract class DemoApp
    {
        private readonly DemoTime clock = new DemoTime();
        private FormWindowState _currentFormWindowState;
        private bool _disposed;
        private Form _form;
        private float _frameAccumulator;
        private int _frameCount;
        private DemoConfiguration _demoConfiguration;

        /// <summary>
        ///   Performs object finalization.
        /// </summary>
        ~DemoApp()
        {
            if (!_disposed)
            {
                Dispose(false);
                _disposed = true;
            }
        }

        /// <summary>
        ///   Disposes of object resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Disposes of object resources.
        /// </summary>
        /// <param name = "disposeManagedResources">If true, managed resources should be
        ///   disposed of in addition to unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (_form != null)
                    _form.Dispose();
            }
        }

        /// <summary>
        /// Return the Handle to display to.
        /// </summary>
        protected IntPtr DisplayHandle
        {
            get
            {
                return _form.Handle;
            }
        }

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>The config.</value>
        public DemoConfiguration Config
        {
            get
            {
                return _demoConfiguration;
            }
        }

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FrameDelta { get; private set; }

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FramePerSecond { get; private set; }

        /// <summary>
        /// Create Form for this demo.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual Form CreateForm(DemoConfiguration config)
        {
            return new RenderForm(config.Title)
            {
                ClientSize = new System.Drawing.Size(config.Width, config.Height)
            };
        }

        /// <summary>
        /// Runs the demo with default presentation
        /// </summary>
        public void Run()
        {
            Run(new DemoConfiguration());
        }

        /// <summary>
        /// Runs the demo.
        /// </summary>
        public void Run(DemoConfiguration demoConfiguration)
        {
            _demoConfiguration = demoConfiguration ?? new DemoConfiguration();
            _form = CreateForm(_demoConfiguration);
            Initialize(_demoConfiguration);

            bool isFormClosed = false;
            bool formIsResizing = false;

            _form.MouseClick += HandleMouseClick;
            _form.KeyDown += HandleKeyDown;
            _form.KeyUp += HandleKeyUp;
            _form.Resize += (o, args) =>
            {
                if (_form.WindowState != _currentFormWindowState)
                {
                    HandleResize(o, args);
                }

                _currentFormWindowState = _form.WindowState;
            };

            _form.ResizeBegin += (o, args) => { formIsResizing = true; };
            _form.ResizeEnd += (o, args) =>
            {
                formIsResizing = false;
                HandleResize(o, args);
            };

            _form.Closed += (o, args) => { isFormClosed = true; };

            LoadContent();

            clock.Start();
            BeginRun();
            RenderLoop.Run(_form, () =>
            {
                if (isFormClosed)
                {
                    return;
                }

                OnUpdate();
                if (!formIsResizing)
                    Render();
            });

            UnloadContent();
            EndRun();

            // Dispose explicity
            Dispose();
        }

        /// <summary>
        ///   In a derived class, implements logic to initialize the sample.
        /// </summary>
        protected abstract void Initialize(DemoConfiguration demoConfiguration);

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic to update any relevant sample state.
        /// </summary>
        protected virtual void Update(DemoTime time)
        {
        }

        /// <summary>
        ///   In a derived class, implements logic to render the sample.
        /// </summary>
        protected virtual void Draw(DemoTime time)
        {
        }

        protected virtual void BeginRun()
        {
        }

        protected virtual void EndRun()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic that should occur before all
        ///   other rendering.
        /// </summary>
        protected virtual void BeginDraw()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic that should occur after all
        ///   other rendering.
        /// </summary>
        protected virtual void EndDraw()
        {
        }

        /// <summary>
        ///   Quits the sample.
        /// </summary>
        public void Exit()
        {
            _form.Close();
        }

        /// <summary>
        ///   Updates sample state.
        /// </summary>
        private void OnUpdate()
        {
            FrameDelta = (float)clock.Update();
            Update(clock);
        }

        protected System.Drawing.Size RenderingSize
        {
            get
            {
                return _form.ClientSize;
            }
        }

        /// <summary>
        ///   Renders the sample.
        /// </summary>
        private void Render()
        {
            _frameAccumulator += FrameDelta;
            ++_frameCount;
            if (_frameAccumulator >= 1.0f)
            {
                FramePerSecond = _frameCount / _frameAccumulator;

                _form.Text = _demoConfiguration.Title + " - FPS: " + FramePerSecond;
                _frameAccumulator = 0.0f;
                _frameCount = 0;
            }

            BeginDraw();
            Draw(clock);
            EndDraw();
        }

        protected virtual void MouseClick(MouseEventArgs e)
        {
        }

        protected virtual void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Exit();
        }

        protected virtual void KeyUp(KeyEventArgs e)
        {
        }

        /// <summary>
        ///   Handles a mouse click event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
        private void HandleMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick(e);
        }

        /// <summary>
        ///   Handles a key down event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(e);
        }

        /// <summary>
        ///   Handles a key up event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            KeyUp(e);
        }

        private void HandleResize(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            // UnloadContent();

            //_configuration.WindowWidth = _form.ClientSize.Width;
            //_configuration.WindowHeight = _form.ClientSize.Height;

            //if( Context9 != null ) {
            //    userInterfaceRenderer.Dispose();

            //    Context9.PresentParameters.BackBufferWidth = _configuration.WindowWidth;
            //    Context9.PresentParameters.BackBufferHeight = _configuration.WindowHeight;

            //    Context9.Device.Reset( Context9.PresentParameters );

            //    userInterfaceRenderer = new UserInterfaceRenderer9( Context9.Device, _form.ClientSize.Width, _form.ClientSize.Height );
            //} else if( Context10 != null ) {
            //    userInterfaceRenderer.Dispose();

            //    Context10.SwapChain.ResizeBuffers( 1, WindowWidth, WindowHeight, Context10.SwapChain.Description.ModeDescription.Format, Context10.SwapChain.Description.Flags );


            //    userInterfaceRenderer = new UserInterfaceRenderer10( Context10.Device, _form.ClientSize.Width, _form.ClientSize.Height );
            //}

            // LoadContent();
        }
    }
}