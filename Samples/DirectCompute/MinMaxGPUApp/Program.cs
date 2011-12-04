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
            const int Count = 1000;

            Console.WriteLine("Texture Size: ({0},{1}) - Count: {2}", Width, Height, Width * Height);
            Console.WriteLine();

            // Create random buffer
            var random = new Random();
            float maxScale = (float)((byte)random.Next());
            var randbomBuffer = new DataStream(sizeof(float) * Width * Height, true, true);
            for (int i = 0; i < Width * Height; i++)
            {
                var value = (float)random.NextDouble();
                if (value < 0.1 || value > 0.9)
                    value = value * (float)random.NextDouble() * maxScale * ((value < 0.1) ? maxScale : 1.0f);
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

            var gpuProfiler = new GPUProfiler();
            gpuProfiler.Initialize(device);
            double elapsedTime = 0.0f;

            Console.WriteLine("Compiling Shaders...");
            Console.WriteLine();

            var pixelShaderMinMax = ToDispose(new MipMapMinMax());
            pixelShaderMinMax.Size = new Size(Width, Height);
            pixelShaderMinMax.Initialize(device);

            var blendMinMax = ToDispose(new VertexBlendMinMax());
            blendMinMax.Size = new Size(Width, Height);
            blendMinMax.Initialize(device);

            var testRunner = new Action<IMinMaxProcessor>( (processor) =>
                    {

                        gpuProfiler.Begin(context);
                        for (int i = 0; i < Count; i++)
                        {
                            processor.Reduce(context, textureView);
                        }
                        gpuProfiler.End(context);

                        context.Flush();
                        float newMin;
                        float newMax;
                        processor.GetResults(context, out newMin, out newMax);

                        elapsedTime = gpuProfiler.GetElapsedMilliseconds(context);
                        Console.WriteLine("GPU {0}: {1} / {2} in {3}ms", processor, newMin, newMax, elapsedTime);
                    });

            Console.WriteLine("Running Tests...");
            Console.WriteLine();

            var clock = new Stopwatch();
            var min = float.MaxValue;
            var max = float.MinValue;
            unsafe
            {
                var buffer = (float*)randbomBuffer.DataPointer;
                clock.Start();
                //for (int j = 0; j < Count; j++)
                for (int j = 0; j < Count; j++)
                {
                    min = float.MaxValue;
                    max = float.MinValue;
                    for (int i = 0; i < Width * Height; i++)
                    {
                        var value = buffer[i];
                        if (value < min) min = value;
                        if (value > max) max = value;
                    }
                }
                clock.Stop();
            }
            Console.WriteLine("CPU MinMax: {0} / {1} {2}ms", min, max, clock.ElapsedMilliseconds);
            
            
            Console.WriteLine();
            for (int i = 1; i < 4; i++)
            {
                pixelShaderMinMax.ReduceFactor = i;
                testRunner(pixelShaderMinMax);
            }

            Console.WriteLine();

            for (int i = 5; i < 10; i++)
            {
                blendMinMax.ReduceFactor = i;
                testRunner(blendMinMax);
            }

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