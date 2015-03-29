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

namespace SharpDX.Direct3D11
{
    public partial class EffectVariable
    {
        /// <summary>
        /// Set data.
        /// </summary>
        /// <param name="data">A reference to the variable.</param>
        /// <param name="count">size in bytes of data to write.</param>
        /// <returns>
        /// Returns one of the following {{Direct3D 10 Return Codes}}.
        /// </returns>
        /// <remarks>
        /// This method does no conversion or type checking; it is therefore a very quick way to access array items.
        /// </remarks>
        /// <unmanaged>HRESULT ID3D11EffectVariable::SetRawValue([None] void* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void SetRawValue(DataStream data, int count)
        {
            SetRawValue(data.PositionPointer, 0, count);
        }

        /// <summary>	
        /// Get data.	
        /// </summary>	
        /// <remarks>	
        /// This method does no conversion or type checking; it is therefore a very quick way to access array items. 	
        /// </remarks>	
        /// <param name="count">The number of bytes to get. </param>
        /// <returns>Returns a <see cref="SharpDX.DataStream"/> filled with the value. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVariable::GetRawValue([None] void* pData,[None] int Offset,[None] int Count)</unmanaged>
        public DataStream GetRawValue(int count)
        {
            DataStream stream = new DataStream(count, true, true);
            GetRawValue(stream.DataPointer, 0, count);
            return stream;
        }

        /// <summary>	
        /// Get a scalar variable.	
        /// </summary>	
        /// <remarks>	
        /// AsScalar returns a version of the effect variable that has been specialized to a scalar variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain scalar data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a scalar variable. See <see cref="SharpDX.Direct3D11.EffectScalarVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectScalarVariable* ID3D11EffectVariable::AsScalar()</unmanaged>
        public SharpDX.Direct3D11.EffectScalarVariable AsScalar()
        {
            var temp = AsScalar_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a vector variable.	
        /// </summary>	
        /// <remarks>	
        /// AsVector returns a version of the effect variable that has been specialized to a vector variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain vector data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a vector variable. See <see cref="SharpDX.Direct3D11.EffectVectorVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectVectorVariable* ID3D11EffectVariable::AsVector()</unmanaged>
        public SharpDX.Direct3D11.EffectVectorVariable AsVector()
        {
            var temp = AsVector_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a matrix variable.	
        /// </summary>	
        /// <remarks>	
        /// AsMatrix returns a version of the effect variable that has been specialized to a matrix variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain matrix data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a matrix variable. See <see cref="SharpDX.Direct3D11.EffectMatrixVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectMatrixVariable* ID3D11EffectVariable::AsMatrix()</unmanaged>
        public SharpDX.Direct3D11.EffectMatrixVariable AsMatrix()
        {
            var temp = AsMatrix_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a string variable.	
        /// </summary>	
        /// <remarks>	
        /// AsString returns a version of the effect variable that has been specialized to a string variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain string data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a string variable. See <see cref="SharpDX.Direct3D11.EffectStringVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectStringVariable* ID3D11EffectVariable::AsString()</unmanaged>
        public SharpDX.Direct3D11.EffectStringVariable AsString()
        {
            var temp = AsString_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a shader-resource variable.	
        /// </summary>	
        /// <remarks>	
        /// AsShaderResource returns a version of the effect variable that has been specialized to a shader-resource variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain shader-resource data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a shader-resource variable. See <see cref="SharpDX.Direct3D11.EffectShaderResourceVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectShaderResourceVariable* ID3D11EffectVariable::AsShaderResource()</unmanaged>
        public SharpDX.Direct3D11.EffectShaderResourceVariable AsShaderResource()
        {
            var temp = AsShaderResource_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a render-target-view variable.	
        /// </summary>	
        /// <remarks>	
        /// This method returns a version of the effect variable that has been specialized to a render-target-view variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain render-target-view data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a render-target-view variable. See <see cref="SharpDX.Direct3D11.EffectRenderTargetViewVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectRenderTargetViewVariable* ID3D11EffectVariable::AsRenderTargetView()</unmanaged>
        public SharpDX.Direct3D11.EffectRenderTargetViewVariable AsRenderTargetView()
        {
            var temp = AsRenderTargetView_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a depth-stencil-view variable.	
        /// </summary>	
        /// <remarks>	
        /// This method returns a version of the effect variable that has been specialized to a depth-stencil-view variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain depth-stencil-view data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a depth-stencil-view variable. See <see cref="SharpDX.Direct3D11.EffectDepthStencilViewVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectDepthStencilViewVariable* ID3D11EffectVariable::AsDepthStencilView()</unmanaged>
        public SharpDX.Direct3D11.EffectDepthStencilViewVariable AsDepthStencilView()
        {
            var temp = AsDepthStencilView_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a class instance variable.
        /// </summary>	
        /// <returns>A reference to a <see cref="SharpDX.Direct3D11.EffectClassInstanceVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectClassInstanceVariable* ID3D11EffectVariable::AsClassInstance()</unmanaged>
        public SharpDX.Direct3D11.EffectClassInstanceVariable AsClassInstance()
        {
            var temp = AsClassInstance_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get an interface variable.
        /// </summary>	
        /// <returns>A reference to a <see cref="SharpDX.Direct3D11.EffectInterfaceVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectClassInstanceVariable* ID3D11EffectVariable::AsInterface()</unmanaged>
        public SharpDX.Direct3D11.EffectInterfaceVariable AsInterface()
        {
            var temp = AsInterface_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get an unordered access view variable.
        /// </summary>	
        /// <returns>A reference to a <see cref="SharpDX.Direct3D11.EffectUnorderedAccessViewVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectUnorderedAccessViewVariable* ID3D11EffectVariable::AsDepthStencilView()</unmanaged>
        public SharpDX.Direct3D11.EffectUnorderedAccessViewVariable AsUnorderedAccessView()
        {
            var temp = AsUnorderedAccessView_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a constant buffer.	
        /// </summary>	
        /// <remarks>	
        /// AsConstantBuffer returns a version of the effect variable that has been specialized to a constant buffer. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain constant buffer data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a constant buffer. See <see cref="SharpDX.Direct3D11.EffectConstantBuffer"/>. </returns>
        /// <unmanaged>ID3D11EffectConstantBuffer* ID3D11EffectVariable::AsConstantBuffer()</unmanaged>
        public SharpDX.Direct3D11.EffectConstantBuffer AsConstantBuffer()
        {
            var temp = AsConstantBuffer_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a shader variable.	
        /// </summary>	
        /// <remarks>	
        /// AsShader returns a version of the effect variable that has been specialized to a shader variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain shader data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a shader variable. See <see cref="SharpDX.Direct3D11.EffectShaderVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectShaderVariable* ID3D11EffectVariable::AsShader()</unmanaged>
        public SharpDX.Direct3D11.EffectShaderVariable AsShader()
        {
            var temp = AsShader_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a effect-blend variable.	
        /// </summary>	
        /// <remarks>	
        /// AsBlend returns a version of the effect variable that has been specialized to an effect-blend variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain effect-blend data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to an effect blend variable. See <see cref="SharpDX.Direct3D11.EffectBlendVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectBlendVariable* ID3D11EffectVariable::AsBlend()</unmanaged>
        public SharpDX.Direct3D11.EffectBlendVariable AsBlend()
        {
            var temp = AsBlend_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a depth-stencil variable.	
        /// </summary>	
        /// <remarks>	
        /// AsDepthStencil returns a version of the effect variable that has been specialized to a depth-stencil variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain depth-stencil data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a depth-stencil variable. See <see cref="SharpDX.Direct3D11.EffectDepthStencilVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectDepthStencilVariable* ID3D11EffectVariable::AsDepthStencil()</unmanaged>
        public SharpDX.Direct3D11.EffectDepthStencilVariable AsDepthStencil()
        {
            var temp = AsDepthStencil_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a rasterizer variable.	
        /// </summary>	
        /// <remarks>	
        /// AsRasterizer returns a version of the effect variable that has been specialized to a rasterizer variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain rasterizer data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a rasterizer variable. See <see cref="SharpDX.Direct3D11.EffectRasterizerVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectRasterizerVariable* ID3D11EffectVariable::AsRasterizer()</unmanaged>
        public SharpDX.Direct3D11.EffectRasterizerVariable AsRasterizer()
        {
            var temp = AsRasterizer_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }

        /// <summary>	
        /// Get a sampler variable.	
        /// </summary>	
        /// <remarks>	
        /// AsSampler returns a version of the effect variable that has been specialized to a sampler variable. Similar to a cast, this specialization will return an invalid object if the effect variable does not contain sampler data. Applications can test the returned object for validity by calling {{IsValid}}. 	
        /// </remarks>	
        /// <returns>A reference to a sampler variable. See <see cref="SharpDX.Direct3D11.EffectSamplerVariable"/>. </returns>
        /// <unmanaged>ID3D11EffectSamplerVariable* ID3D11EffectVariable::AsSampler()</unmanaged>
        public SharpDX.Direct3D11.EffectSamplerVariable AsSampler()
        {
            var temp = AsSampler_();
            if (temp == null || !temp.IsValid)
                return null;
            return temp;
        }
    }
}