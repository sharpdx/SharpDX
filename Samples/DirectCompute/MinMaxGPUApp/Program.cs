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
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace MinMaxGPUApp
{
    /// <summary>
    /// SharpDX MinMax reduction on the GPU.
    /// </summary>
    internal class Program : Component
    {
        public void Run()
        {

            // --------------------------------------------------------------------------------------
            // Init Direct3D11
            // --------------------------------------------------------------------------------------

            // Create Device and SwapChain 
            var device = new Device(DriverType.Hardware, DeviceCreationFlags.Debug);
            var context = device.ImmediateContext;

            const int Width = 1024;
            const int Height = 1024;

            // Create random buffer
            var random = new Random();
            var randbomBuffer = new DataStream(sizeof(float) * Width * Height, true, true);
            var min = float.MaxValue;
            var max = float.MinValue;
            for (int i = 0; i < Width * Height; i++)
            {
                var value = (float)random.NextDouble();
                if (value < min) min = value;
                if (value > max) max = value;
                randbomBuffer.Write(value);
            }

            // Create random 2D texture 
            var texture = ToDispose(new Texture2D(
                device,
                new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = Format.R32_Float,
                    Width = Width,
                    Height = Height,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Immutable
                }, new DataRectangle(randbomBuffer.DataPointer, sizeof(float) * Width)));
            var textureView = ToDispose(new ShaderResourceView(device, texture));

            // Create result 2D texture to readback by CPU
            var textureReadback = ToDispose(new Texture2D(
                device,
                new Texture2DDescription
                {
                    ArraySize = 1,
                    BindFlags = BindFlags.None,
                    CpuAccessFlags = CpuAccessFlags.Read,
                    Format = Format.R32G32_Float,
                    Width = 1,
                    Height = 1,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Staging
                }));


            Console.WriteLine("CPU MinMax: {0} / {1}", min, max);

            var gpuProfiler = new GPUProfiler();
            gpuProfiler.Initialize(device);
            double elapsedTime = 0.0f;

            var pixelShaderMinMax = ToDispose(new PixelShaderMinMax());
            pixelShaderMinMax.Initialize(device);
            pixelShaderMinMax.Size = new Size(Width, Height);

            var blendMinMax = ToDispose(new BlendMinMax());
            blendMinMax.Initialize(device);
            blendMinMax.Size = new Size(Width, Height);

            var testRunner = new Action<IMinMaxProcessor>( (processor) =>
                    {

                        gpuProfiler.Begin(context);
                        for (int i = 0; i < 1000; i++)
                        {
                            processor.Reduce(context, textureView);
                        }
                        gpuProfiler.End(context);

                        context.Flush();
                        processor.Copy(context, textureReadback);
                        DataStream result;
                        context.MapSubresource(textureReadback, 0, MapMode.Read, MapFlags.None, out result);
                        var minMaxFactor = processor.MinMaxFactor;
                        var newMin =  minMaxFactor.X * result.ReadFloat();
                        var newMax = minMaxFactor.Y * result.ReadFloat();
                        context.UnmapSubresource(textureReadback, 0);

                        elapsedTime = gpuProfiler.GetElapsedMilliseconds(context);
                        Console.WriteLine("GPU {0}: {1} / {2} in {3}ms", processor.GetType().Name, newMin, newMax, elapsedTime);
                    });

            testRunner(pixelShaderMinMax);
            testRunner(blendMinMax);

            // Dispose all resource created
            Dispose();
        }

        [STAThread]
        private static void Main()
        {
            var program = new Program();
            program.Run();
        }
    }
}