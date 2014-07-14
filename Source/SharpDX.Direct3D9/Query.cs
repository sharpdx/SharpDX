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

namespace SharpDX.Direct3D9
{
    public partial class Query
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="type">The type.</param>
        public Query(Device device, QueryType type)
        {
            device.CreateQuery(type, this);
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <unmanaged>D3DQUERYTYPE IDirect3DQuery9::GetType()</unmanaged>
        public SharpDX.Direct3D9.QueryType Type
        {
            get { return GetTypeInfo(); }
        }

        /// <summary>	
        /// <p>Polls a queried resource to get the query state or a query result. For more information about queries, see Queries (Direct3D 9).</p>	
        /// </summary>	
        /// <typeparam name="T">Type of the object to query. See remarks.</typeparam>
        /// <param name="data">The value of the query </param>
        /// <param name="flush">if set to <c>true</c> [flush].</param>
        /// <returns>The return type identifies the query state (see Queries (Direct3D 9)). The method returns <strong>true</strong> if the query data is available and <strong>false</strong>if it is not.  These are considered successful return values. If the method fails when <strong>D3DGETDATA_FLUSH</strong> is used, the return value can be <see cref="SharpDX.Direct3D9.ResultCode.DeviceLost"/>. </p></returns>	
        /// <remarks>
        /// Each <see cref="QueryType"/> is expecting a particular type.
        /// <ul>
        /// <li>QueryType.VCache  => <see cref="VCache"/></li>
        /// <li>QueryType.ResourceManager  => <see cref="ResourceManager"/></li>
        /// <li>QueryType.VertexStats  => <see cref="VertexStats"/></li>
        /// <li>QueryType.Event  => <see cref="bool"/></li>
        /// <li>QueryType.Occlusion  => <see cref="int"/> or <see cref="uint"/></li>
        /// <li>QueryType.Timestamp  => <see cref="long"/> or <see cref="ulong"/></li>
        /// <li>QueryType.TimestampDisjoint  => <see cref="bool"/></li>
        /// <li>QueryType.PipelineTimings  => <see cref="PipelineTimings"/></li>
        /// <li>QueryType.InterfaceTimings  => <see cref="InterfaceTimings"/></li>
        /// <li>QueryType.VertexTimings  => <see cref="StageTimings"/></li>
        /// <li>QueryType.BandwidthTimings  => <see cref="BandwidthTimings"/></li>
        /// <li>QueryType.CacheUtilization  => <see cref="CacheUtilization"/></li>
        /// </ul>
        /// </remarks>
        /// <msdn-id>bb205873</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DQuery9::GetData([In] void* pData,[In] unsigned int dwSize,[In] unsigned int dwGetDataFlags)</unmanaged>	
        /// <unmanaged-short>IDirect3DQuery9::GetData</unmanaged-short>	
        public unsafe bool GetData<T>(out T data, bool flush) where T : struct
        {
            QueryType type = Type;
            bool isInvalid = true;
            switch (type)
            {
                case QueryType.VCache:
                    isInvalid = typeof(T) != typeof(VCache);
                    break;

                case QueryType.ResourceManager:
                    isInvalid = typeof(T) != typeof(ResourceManager);
                    break;

                case QueryType.VertexStats:
                    isInvalid = typeof(T) != typeof(VertexStats);
                    break;

                case QueryType.Event:
                    isInvalid = typeof(T) != typeof(bool);
                    break;

                case QueryType.Occlusion:
                    isInvalid = (typeof(T) != typeof(int)) && (typeof(T) != typeof(uint));
                    break;

                case QueryType.Timestamp:
                    isInvalid = (typeof(T) != typeof(long)) && (typeof(T) != typeof(ulong));
                    break;

                case QueryType.TimestampDisjoint:
                    isInvalid = typeof(T) != typeof(bool);
                    break;

                case QueryType.TimestampFreq:
                    isInvalid = (typeof(T) != typeof(long)) && (typeof(T) != typeof(ulong));
                    break;

                case QueryType.PipelineTimings:
                    isInvalid = typeof(T) != typeof(PipelineTimings);
                    break;

                case QueryType.InterfaceTimings:
                    isInvalid = typeof(T) != typeof(InterfaceTimings);
                    break;

                case QueryType.VertexTimings:
                    isInvalid = typeof(T) != typeof(StageTimings);
                    break;

                case QueryType.BandwidthTimings:
                    isInvalid = typeof(T) != typeof(BandwidthTimings);
                    break;

                case QueryType.CacheUtilization:
                    isInvalid = typeof(T) != typeof(CacheUtilization);
                    break;                
            }

            if (isInvalid)
                throw new ArgumentException(string.Format("Unsupported data size [{0}] for type [{1}]. See documentation for expecting type.", typeof(T), type));

            Result result;
            if (typeof(T) == typeof(bool))
            {
                int value = 0;
                result = GetData(new IntPtr(&value), 4, flush ? 1 : 0);
                data = (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                data = default(T);
                result = GetData((IntPtr)Interop.Fixed(ref data), Utilities.SizeOf<T>(), flush ? 1 : 0);
            }

            if (result == Result.Ok)
                return true;
            if (result == Result.False)
                return false;

            // Should throw an exception
            result.CheckError();
            return false;
        }
    }
}

