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

using SharpDX.Direct3D11;

namespace MinMaxGPUApp
{
    public class GPUProfiler
    {
        private Query queryTimeStampDisjoint;
        private Query queryTimeStampStart;
        private Query queryTimeStampEnd;

        public void Initialize(Device device)
        {
            queryTimeStampDisjoint = new Query(device, new QueryDescription() { Type = QueryType.TimestampDisjoint });
            queryTimeStampStart = new Query(device, new QueryDescription() { Type = QueryType.Timestamp });
            queryTimeStampEnd = new Query(device, new QueryDescription() { Type = QueryType.Timestamp });
        }

        public void Begin(DeviceContext context)
        {
            context.Begin(queryTimeStampDisjoint);
            context.End(queryTimeStampStart);
        }

        public void End(DeviceContext context)
        {
            context.End(queryTimeStampEnd);
            context.End(queryTimeStampDisjoint);
        }

        public double GetElapsedMilliseconds(DeviceContext context)
        {

            long timestampStart;
            long timestampEnd;
            QueryDataTimestampDisjoint disjointData;

            while (!context.GetData(queryTimeStampStart, AsynchronousFlags.None, out timestampStart)) ;
            while (!context.GetData(queryTimeStampEnd, AsynchronousFlags.None, out timestampEnd)) ;
            while (!context.GetData(queryTimeStampDisjoint, AsynchronousFlags.None, out disjointData)) ;

            if (disjointData.Disjoint)
                return -1;
            long delta = timestampEnd - timestampStart;
            return (delta * 1000.0) / disjointData.Frequency;
        }
    }
}