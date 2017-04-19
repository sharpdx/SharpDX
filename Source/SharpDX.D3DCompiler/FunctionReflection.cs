// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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

namespace SharpDX.D3DCompiler
{
    public partial class FunctionReflection
    {
        /// <summary>	
        /// <p>Returns all constant buffers provided by this function</p>	
        /// </summary>	
        /// <returns><p>All references to <strong><see cref="SharpDX.D3DCompiler.ConstantBuffer"/></strong> that represents the constant buffers.</p></returns>	
        public ConstantBuffer[] ConstantBuffers
        {
            get
            {
                var result = new ConstantBuffer[this.Description.ConstantBuffers];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = this.GetConstantBufferByIndex(i);
                }
                return result;
            }
        }

        /// <summary>	
        /// <p>Returns all function parameters</p>	
        /// </summary>	
        /// <returns><p>All references to <strong><see cref="SharpDX.D3DCompiler.FunctionParameterReflection"/></strong> that represents the function parameters.</p></returns>	
        public FunctionParameterReflection[] Parameters
        {
            get
            {
                var result = new FunctionParameterReflection[this.Description.FunctionParameterCount];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = this.GetFunctionParameter(i);
                }
                return result;
            }
        }

        /// <summary>
        /// Returns reflection for function return parameter
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if function has no return value (void)</exception>
        public FunctionParameterReflection ReturnParameter
        {
            get
            {
                if (!this.Description.HasReturn)
                {
                    throw new ArgumentException("Function has no return parameter, check function.Description.HasReturn before to call this function");
                }
                return this.GetFunctionParameter(-1);
            }
        }

        /// <summary>	
        /// <p>Gets a description of how a resource is bound to a function. </p>	
        /// </summary>	
        /// <param name="resourceIndex"><dd>  <p>A zero-based resource index.</p> </dd></param>	
        /// <returns><p>A reference to a <strong><see cref="SharpDX.D3DCompiler.InputBindingDescription"/></strong> structure that describes input binding of the resource. </p></returns>	
        /// <remarks>	
        /// <p>A shader consists of executable code (the compiled HLSL functions) and a set of resources that supply the shader with input data. <strong>GetResourceBindingDesc</strong> gets info about how one resource in the set is bound as an input to the shader. The  <em>ResourceIndex</em> parameter specifies the index for the resource.</p>	
        /// </remarks>	
        /// <include file='..\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D11FunctionReflection::GetResourceBindingDesc']/*"/>	
        /// <msdn-id>dn280551</msdn-id>	
        /// <unmanaged>HRESULT ID3D11FunctionReflection::GetResourceBindingDesc([In] unsigned int ResourceIndex,[Out] D3D11_SHADER_INPUT_BIND_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D11FunctionReflection::GetResourceBindingDesc</unmanaged-short>	
        public InputBindingDescription GetResourceBindingDescription(int index)
        {
            InputBindingDescription result;
            this.GetResourceBindingDescription(index, out result);
            return result;
        }

        /// <summary>	
        /// <p>Gets a description of how a resource is bound to a function. </p>	
        /// </summary>	
        /// <param name="name"><dd>  <p>Resource name.</p> </dd></param>	
        /// <returns><p>A reference to a <strong><see cref="SharpDX.D3DCompiler.InputBindingDescription"/></strong> structure that describes input binding of the resource. </p></returns>	
        /// <remarks>	
        /// <p>A shader consists of executable code (the compiled HLSL functions) and a set of resources that supply the shader with input data. <strong>GetResourceBindingDesc</strong> gets info about how one resource in the set is bound as an input to the shader. The  <em>ResourceIndex</em> parameter specifies the index for the resource.</p>	
        /// </remarks>	
        /// <include file='..\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D11FunctionReflection::GetResourceBindingDesc']/*"/>	
        /// <msdn-id>dn280551</msdn-id>	
        /// <unmanaged>HRESULT ID3D11FunctionReflection::GetResourceBindingDesc([In] unsigned int ResourceIndex,[Out] D3D11_SHADER_INPUT_BIND_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D11FunctionReflection::GetResourceBindingDesc</unmanaged-short>	
        public InputBindingDescription GetResourceBindingDescription(string name)
        {
            InputBindingDescription result;
            this.GetResourceBindingDescByName(name, out result);
            return result;
        }

        /// <summary>
        /// Returns all resource bindings attached to this resource
        /// </summary>
        public InputBindingDescription[] ResourceBindings
        {
            get
            {
                InputBindingDescription[] result = new InputBindingDescription[this.Description.BoundResources];
                for (int i = 0; i < result.Length;i++)
                {
                    this.GetResourceBindingDescription(i, out result[i]);
                }
                return result;
            }
        }
    }
}
