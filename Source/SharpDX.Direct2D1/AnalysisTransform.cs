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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1
{
    public partial class AnalysisTransform
    {
        /// <summary>	
        /// Supplies the analysis data to an analysis transform.
        /// </summary>	
        /// <param name="analysisData"><para>The data that the transform will analyze.</para></param>	
        /// <remarks>	
        /// The output of the transform will be copied to CPU-accessible memory by the imaging effects system before being passed to the implementation.If this call fails, the corresponding <see cref="SharpDX.Direct2D1.Effect"/> instance is placed into an error state and fails to draw.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1AnalysisTransform::ProcessAnalysisResults([In, Buffer] const unsigned char* analysisData,[In] unsigned int analysisDataCount)</unmanaged>	
        public void ProcessAnalysisResults(DataStream analysisData)
        {
            ProcessAnalysisResults(analysisData.DataPointer, (int)analysisData.Length);
        }

        /// <summary>	
        /// Supplies the analysis data to an analysis transform.
        /// </summary>	
        /// <param name="analysisData"><para>The data that the transform will analyze.</para></param>	
        /// <remarks>	
        /// The output of the transform will be copied to CPU-accessible memory by the imaging effects system before being passed to the implementation.If this call fails, the corresponding <see cref="SharpDX.Direct2D1.Effect"/> instance is placed into an error state and fails to draw.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1AnalysisTransform::ProcessAnalysisResults([In, Buffer] const unsigned char* analysisData,[In] unsigned int analysisDataCount)</unmanaged>	
        public void ProcessAnalysisResults<T>(T analysisData) where T : struct
        {
            unsafe
            {
                ProcessAnalysisResults((IntPtr)Interop.Fixed(ref analysisData), Utilities.SizeOf<T>());
            }
        }

        /// <summary>	
        /// Supplies the analysis data to an analysis transform.
        /// </summary>	
        /// <param name="analysisData"><para>The data that the transform will analyze.</para></param>	
        /// <remarks>	
        /// The output of the transform will be copied to CPU-accessible memory by the imaging effects system before being passed to the implementation.If this call fails, the corresponding <see cref="SharpDX.Direct2D1.Effect"/> instance is placed into an error state and fails to draw.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1AnalysisTransform::ProcessAnalysisResults([In, Buffer] const unsigned char* analysisData,[In] unsigned int analysisDataCount)</unmanaged>	
        public void ProcessAnalysisResults<T>(T[] analysisData) where T : struct
        {
            unsafe
            {
                ProcessAnalysisResults((IntPtr)Interop.Fixed(analysisData), Utilities.SizeOf<T>() * analysisData.Length);
            }
        }
    }
}