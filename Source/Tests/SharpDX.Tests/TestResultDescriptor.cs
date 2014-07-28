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

using NUnit.Framework;

using SharpDX.DXGI;
using SharpDX.Direct3D;

using Factory = SharpDX.DXGI.Factory;

namespace SharpDX.Tests
{
    /// <summary>
    /// Tests for <see cref="SharpDX.ResultDescriptor"/>
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.ResultDescriptor")]
    public class TestResultDescriptor
    {
        [Test]
        public void TestDefaultMessage()
        {
            // This will get the default message for Result.Ok code
            var descriptor = ResultDescriptor.Find(Result.Ok);
            Assert.True(!descriptor.Description.Contains("Unknown"));
        }

        [Test]
        public void TestDXGI()
        {
            // Force to load DXGI assembly
            var factory = new Factory();
            factory.Dispose();
            // Look for DXGI descriptor SharpDX.DXGI.ResultCode.DeviceRemoved
            var descriptor = ResultDescriptor.Find(0x887A0005);
            Assert.AreEqual(descriptor.NativeApiCode, "DXGI_ERROR_DEVICE_REMOVED");
        }

        [Test]
        public void TestException()
        {
            // Device is implicitly created with a DXGI Factory / Adapter
            var device = new Direct3D11.Device(DriverType.Hardware);

            // Create another DXGI Factory
            var factory = new SharpDX.DXGI.Factory1();

            try
            {
                // This call should raise a DXGI_ERROR_INVALID_CALL:
                // The reason is the SwapChain must be created with a d3d device that was created with the same factory
                // Because we were creating the D3D11 device without a DXGI factory, it is associated with another factory.
                var swapChain = new SwapChain(
                    factory,
                    device,
                    new SwapChainDescription()
                        {
                            BufferCount = 1,
                            Flags = SwapChainFlags.None,
                            IsWindowed = false,
                            ModeDescription = new ModeDescription(1024, 768, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                            SampleDescription = new SampleDescription(1, 0),
                            OutputHandle = IntPtr.Zero,
                            SwapEffect = SwapEffect.Discard,
                            Usage = Usage.RenderTargetOutput
                        });
            } catch (SharpDXException exception)
            {
                Assert.AreEqual(exception.Descriptor.NativeApiCode, "DXGI_ERROR_INVALID_CALL");
            }
        }
    }
}
