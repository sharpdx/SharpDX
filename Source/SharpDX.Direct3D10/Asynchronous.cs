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

namespace SharpDX.Direct3D10
{
    public partial class Asynchronous
    {

        /// <summary>	
        /// Get data from the GPU asynchronously.	
        /// </summary>	
        /// <remarks>	
        /// GetData retrieves the data collected between calls to <see cref="SharpDX.Direct3D10.Asynchronous.Begin"/> and <see cref="SharpDX.Direct3D10.Asynchronous.End"/>.  Certain queries only require a call to ID3D10Asynchronous::End in which case the data returned by GetData is accurate up to the last call to ID3D10Asynchronous::End (See <see cref="SharpDX.Direct3D10.Query"/>). If DataSize is 0, GetData is only used to check status where a return value of S_OK indicates that data is available to give to an application, and a return value of S_FALSE indicates data is not yet available. It is invalid to invoke this function on a predicate created with the flag D3D10_QUERY_MISCFLAG_PREDICATEHINT. If the asynchronous interface that calls this function is <see cref="SharpDX.Direct3D10.Query"/>, then the following table applies.  Query TypeOutput Data TypeSupports Begin Method EVENTBOOLNO OCCLUSIONUINT64YES TIMESTAMPUINT64NO TIMESTAMP_DISJOINTQUERYDATA_TIMESTAMP_DISJOINTYES PIPELINE_STATISTICSQUERYDATA_PIPELINE_STATISTICSYES OCCLUSION_PREDICATEBOOLYES SO_STATISTICSQUERYDATA_SO_STATISTICSYES SO_OVERFLOW_PREDICATEBOOLYES  ? If the asynchronous interface that calls this API is <see cref="SharpDX.Direct3D10.Counter"/>, then the following applies.  Counter TypeOutput Data TypeUnits GPU_IDLEFLOAT32fraction of time VERTEX_PROCESSINGFLOAT32fraction of time GEOMETRY_PROCESSINGFLOAT32fraction of time PIXEL_PROCESSINGFLOAT32fraction of time OTHER_GPU_PROCESSINGFLOAT32fraction of time HOST_ADAPTER_BANDWIDTH_UTILIZATIONFLOAT32fraction of theoretical maximum LOCAL_VIDMEM_BANDWIDTH_UTILIZATIONFLOAT32fraction of theoretical maximum VERTEX_THROUGHPUT_UTILIZATIONFLOAT32fraction of theoretical maximum TRISETUP_THROUGHPUT_UTILIZATIONFLOAT32fraction of theoretical maximum FILLRATE_THROUGHPUT_UTILIZATIONFLOAT32fraction of theoretical maximum VERTEXSHADER_MEMORY_LIMITEDFLOAT32fraction of time VERTEXSHADER_COMPUTATION_LIMITEDFLOAT32fraction of time GEOMETRYSHADER_MEMORY_LIMITEDFLOAT32fraction of time GEOMETRYSHADER_COMPUTATION_LIMITEDFLOAT32fraction of time PIXELSHADER_MEMORY_LIMITEDFLOAT32fraction of time PIXELSHADER_COMPUTATION_LIMITEDFLOAT32fraction of time POST_TRANSFORM_CACHE_HIT_RATEFLOAT32fraction TEXTURE_CACHE_HIT_RATEFLOAT32fraction  ? The value returned by a GPU_IDLE, VERTEX_PROCESSING, GEOMETRY_PROCESSING, PIXEL_PROCESSING, or OTHER_GPU_PROCESSING counter may be different depending on the number of parallel counters that exist on a video card, and those values can be interpreted with the following equation:  ?  Equation to interpret the number of parallel counters ? The number of parallel counters that a video card has is available from NumDetectableParallelUnits in <see cref="CounterMetadata"/>, and it can be retrieved by calling <see cref="Device.CheckCounter"/>. 	
        /// </remarks>	
        /// <returns>If this function succeeds, returns a <see cref="SharpDX.DataStream"/> containing the asynchronous data sent from the GPU. </returns>
        /// <unmanaged>HRESULT ID3D10Asynchronous::GetData([Out, Buffer, Optional] void* pData,[In] int DataSize,[In] int GetDataFlags)</unmanaged>
        public DataStream GetData()
        {
            return GetData(AsynchronousFlags.None);
        }

        /// <summary>	
        /// Get data from the GPU asynchronously.	
        /// </summary>	
        /// <remarks>	
        /// GetData retrieves the data collected between calls to <see cref="SharpDX.Direct3D10.Asynchronous.Begin"/> and <see cref="SharpDX.Direct3D10.Asynchronous.End"/>.  Certain queries only require a call to ID3D10Asynchronous::End in which case the data returned by GetData is accurate up to the last call to ID3D10Asynchronous::End (See <see cref="SharpDX.Direct3D10.Query"/>). If DataSize is 0, GetData is only used to check status where a return value of S_OK indicates that data is available to give to an application, and a return value of S_FALSE indicates data is not yet available. It is invalid to invoke this function on a predicate created with the flag D3D10_QUERY_MISCFLAG_PREDICATEHINT. If the asynchronous interface that calls this function is <see cref="SharpDX.Direct3D10.Query"/>, then the following table applies.  Query TypeOutput Data TypeSupports Begin Method EVENTBOOLNO OCCLUSIONUINT64YES TIMESTAMPUINT64NO TIMESTAMP_DISJOINTQUERYDATA_TIMESTAMP_DISJOINTYES PIPELINE_STATISTICSQUERYDATA_PIPELINE_STATISTICSYES OCCLUSION_PREDICATEBOOLYES SO_STATISTICSQUERYDATA_SO_STATISTICSYES SO_OVERFLOW_PREDICATEBOOLYES  ? If the asynchronous interface that calls this API is <see cref="SharpDX.Direct3D10.Counter"/>, then the following applies.  Counter TypeOutput Data TypeUnits GPU_IDLEFLOAT32fraction of time VERTEX_PROCESSINGFLOAT32fraction of time GEOMETRY_PROCESSINGFLOAT32fraction of time PIXEL_PROCESSINGFLOAT32fraction of time OTHER_GPU_PROCESSINGFLOAT32fraction of time HOST_ADAPTER_BANDWIDTH_UTILIZATIONFLOAT32fraction of theoretical maximum LOCAL_VIDMEM_BANDWIDTH_UTILIZATIONFLOAT32fraction of theoretical maximum VERTEX_THROUGHPUT_UTILIZATIONFLOAT32fraction of theoretical maximum TRISETUP_THROUGHPUT_UTILIZATIONFLOAT32fraction of theoretical maximum FILLRATE_THROUGHPUT_UTILIZATIONFLOAT32fraction of theoretical maximum VERTEXSHADER_MEMORY_LIMITEDFLOAT32fraction of time VERTEXSHADER_COMPUTATION_LIMITEDFLOAT32fraction of time GEOMETRYSHADER_MEMORY_LIMITEDFLOAT32fraction of time GEOMETRYSHADER_COMPUTATION_LIMITEDFLOAT32fraction of time PIXELSHADER_MEMORY_LIMITEDFLOAT32fraction of time PIXELSHADER_COMPUTATION_LIMITEDFLOAT32fraction of time POST_TRANSFORM_CACHE_HIT_RATEFLOAT32fraction TEXTURE_CACHE_HIT_RATEFLOAT32fraction  ? The value returned by a GPU_IDLE, VERTEX_PROCESSING, GEOMETRY_PROCESSING, PIXEL_PROCESSING, or OTHER_GPU_PROCESSING counter may be different depending on the number of parallel counters that exist on a video card, and those values can be interpreted with the following equation:  ?  Equation to interpret the number of parallel counters ? The number of parallel counters that a video card has is available from NumDetectableParallelUnits in <see cref="CounterMetadata"/>, and it can be retrieved by calling <see cref="Device.CheckCounter"/>. 	
        /// </remarks>	
        /// <param name="flags">Optional flags. Can be 0 or any combination of the flags enumerated by <see cref="SharpDX.Direct3D10.AsynchronousFlags"/>. </param>
        /// <returns>If this function succeeds, returns a <see cref="SharpDX.DataStream"/> containing the asynchronous data sent from the GPU. </returns>
        /// <unmanaged>HRESULT ID3D10Asynchronous::GetData([Out, Buffer, Optional] void* pData,[In] int DataSize,[In] int GetDataFlags)</unmanaged>
        public DataStream GetData(AsynchronousFlags flags)
        {
            int dataSize = GetDataSize();
            DataStream dataStream = new DataStream(dataSize, true, true);
            try
            {
                GetData(dataStream.DataPointer, dataSize, (int)flags);
            }
            catch (Exception)
            {
                dataStream.Dispose();
                throw;
            }
            return dataStream;
        }

        /// <summary>
        /// Gets a value indicating whether or not data is available for consumption.
        /// </summary>
        public bool IsDataAvailable
        {
            get
            {
                // http://msdn.microsoft.com/en-us/library/windows/desktop/bb173503%28v=vs.85%29.aspx
                // any result other than S_OK will indicate that there is no data available.
                return GetData(IntPtr.Zero, 0, 0) == Result.Ok;
            }
        }
    }
}