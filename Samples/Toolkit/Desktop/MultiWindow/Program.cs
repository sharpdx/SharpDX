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

using System;

using MiniCube;

using SharpDX;

namespace MultiWindow
{
    /// <summary>
    /// MultiWindow Sample: Direct3D11 rendering to 2 WinForms and 3 controls inside a form.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            using (var program = new MiniCubeGame())
            {
                var triOrange = new MiniTriRenderer(program) { ForegroundColor = Color.Orange };
                program.GameSystems.Add(triOrange);

                var multiControlForm = new MultiControlForm();
                multiControlForm.Show();

                var triRed = new MiniTriRenderer(program, multiControlForm.RenderControl1) { ForegroundColor = Color.Red };
                program.GameSystems.Add(triRed);

                var triGreen = new MiniTriRenderer(program, multiControlForm.RenderControl2) { ForegroundColor = Color.Green };
                program.GameSystems.Add(triGreen);

                var triBlue = new MiniTriRenderer(program, multiControlForm.RenderControl3) { ForegroundColor = Color.Blue };
                program.GameSystems.Add(triBlue);

                program.Run();
            }
        }
    }
}
