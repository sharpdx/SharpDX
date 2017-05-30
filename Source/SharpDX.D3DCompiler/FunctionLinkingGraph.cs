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

using SharpDX.Direct3D;

namespace SharpDX.D3DCompiler
{
    /// <summary>	
    /// <p>[This documentation is preliminary and is subject to change.]</p><p>A function-linking-graph interface is used for constructing shaders that consist of a sequence of precompiled function calls that pass values to each other .</p>	
    /// </summary>	
    /// <remarks>	
    /// <p>To get a function-linking-graph interface, call <strong><see cref="SharpDX.D3DCompiler.D3D.CreateFunctionLinkingGraph"/></strong>. </p><p>You can use the function-linking-graph (FLG) interface methods to construct shaders that consist of a sequence of precompiled function calls that pass values to each other. You don't need to write HLSL and then call the HLSL compiler. Instead, the shader structure is specified programmatically via a C++ API. FLG nodes represent input and output signatures and invocations of precompiled library functions. The order of registering the function-call nodes defines the sequence of invocations. You must specify the input signature node first and the output signature node last. FLG edges define how values are passed from one node to another. The data types of passed values must be the same; there is no implicit type conversion. Shape and swizzling rules follow the HLSL behavior. Values can only be passed forward in this sequence.</p>	
    /// </remarks>	
    /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D11FunctionLinkingGraph']/*"/>	
    /// <msdn-id>dn280535</msdn-id>	
    /// <unmanaged>ID3D11FunctionLinkingGraph</unmanaged>	
    /// <unmanaged-short>ID3D11FunctionLinkingGraph</unmanaged-short>	
    public partial class FunctionLinkingGraph
    {
        // Slot ID for library function return
        // #define D3D_RETURN_PARAMETER_INDEX (-1)
        private const int ReturnParameterIndex = -1;

        /// <summary>
        /// Initializes a new instance of <see cref="FunctionLinkingGraph"/>.
        /// </summary>
        public FunctionLinkingGraph()
        {
            D3D.CreateFunctionLinkingGraph(0, this);
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Sets the input signature of the function-linking-graph.</p>	
        /// </summary>	
        /// <param name="parameters"><dd>  <p>An array of  <strong><see cref="SharpDX.D3DCompiler.ParameterDescription"/></strong> structures for the parameters of the input signature.</p> </dd></param>	
        /// <returns><p>A reference to the <strong><see cref="SharpDX.D3DCompiler.LinkingNode"/></strong> interface that represents the input signature of the function-linking-graph.</p></returns>	
        /// <msdn-id>dn280542</msdn-id>	
        /// <unmanaged>HRESULT ID3D11FunctionLinkingGraph::SetInputSignature([In, Buffer] const D3D11_PARAMETER_DESC* pInputParameters,[In] unsigned int cInputParameters,[Out] ID3D11LinkingNode** ppInputNode)</unmanaged>	
        /// <unmanaged-short>ID3D11FunctionLinkingGraph::SetInputSignature</unmanaged-short>	
        public LinkingNode SetInputSignature(params ParameterDescription[] parameters)
        {
            LinkingNode node;
            SetInputSignature(parameters, parameters.Length, out node);
            return node;
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Sets the output signature of the function-linking-graph.</p>	
        /// </summary>	
        /// <param name="parameters"><dd>  <p>An array of  <strong><see cref="SharpDX.D3DCompiler.ParameterDescription"/></strong> structures for the parameters of the output signature.</p> </dd></param>	
        /// <returns><p>A reference to the <strong><see cref="SharpDX.D3DCompiler.LinkingNode"/></strong> interface that represents the output signature of the function-linking-graph.</p></returns>	
        /// <msdn-id>dn280543</msdn-id>	
        /// <unmanaged>HRESULT ID3D11FunctionLinkingGraph::SetOutputSignature([In, Buffer] const D3D11_PARAMETER_DESC* pOutputParameters,[In] unsigned int cOutputParameters,[Out] ID3D11LinkingNode** ppOutputNode)</unmanaged>	
        /// <unmanaged-short>ID3D11FunctionLinkingGraph::SetOutputSignature</unmanaged-short>	
        public LinkingNode SetOutputSignature(params ParameterDescription[] parameters)
        {
            LinkingNode node;
            SetOutputSignature(parameters, parameters.Length, out node);
            return node;
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Initializes a shader module from the function-linking-graph object.</p>	
        /// </summary>	
        /// <returns><p>A reference to an <strong><see cref="SharpDX.D3DCompiler.ModuleInstance"/></strong> interface for the shader module to initialize.</p></returns>	
        /// <msdn-id>dn280537</msdn-id>	
        /// <unmanaged>HRESULT ID3D11FunctionLinkingGraph::CreateModuleInstance([Out] ID3D11ModuleInstance** ppModuleInstance,[Out, Optional] ID3D10Blob** ppErrorBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11FunctionLinkingGraph::CreateModuleInstance</unmanaged-short>	
        /// <exception cref="CompilationException">Is thrown when creation fails and the error text is available.</exception>
        /// <exception cref="SharpDXException">Is thrown when creation fails and the error text is <b>not</b> available.</exception>
        public ModuleInstance CreateModuleInstance()
        {
            ModuleInstance module;
            Blob errorBlob;
            var resultCode = CreateModuleInstance(out module, out errorBlob);

            if (resultCode.Failure)
            {
                if (errorBlob != null)
                    throw new CompilationException(resultCode, Utilities.BlobToString(errorBlob));

                throw new SharpDXException(resultCode);
            }

            return module;
        }

        /// <summary>
        /// Gets the error from the last function call of the function-linking-graph, as a string
        /// </summary>
        public string LastErrorString
        {
            get { return Utilities.BlobToString(this.LastError);  }
        }


        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Creates a call-function linking node to use in the function-linking-graph.</p>	
        /// </summary>	
        /// <param name="moduleWithFunctionPrototypeRef"><dd>  <p>A reference to the <strong><see cref="SharpDX.D3DCompiler.ModuleInstance"/></strong> interface for the library module that contains the function prototype.</p> </dd></param>	
        /// <param name="functionNameRef"><dd>  <p>The name of the function.</p> </dd></param>	
        /// <returns><dd>  <p>A reference to a variable that receives a reference to the <strong><see cref="SharpDX.D3DCompiler.LinkingNode"/></strong> interface that represents the function in the function-linking-graph.</p> </dd></returns>	
        /// <msdn-id>dn280536</msdn-id>	
        /// <unmanaged>HRESULT ID3D11FunctionLinkingGraph::CallFunction([In, Optional] const char* pModuleInstanceNamespace,[In] ID3D11Module* pModuleWithFunctionPrototype,[In] const char* pFunctionName,[Out] ID3D11LinkingNode** ppCallNode)</unmanaged>	
        /// <unmanaged-short>ID3D11FunctionLinkingGraph::CallFunction</unmanaged-short>	
        public LinkingNode CallFunction(Module moduleWithFunctionPrototypeRef, string functionNameRef)
        {
            return CallFunction(string.Empty, moduleWithFunctionPrototypeRef, functionNameRef);
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Passes the <b>return</b> value from a source linking node to a destination linking node.</p>	
        /// </summary>	
        /// <remarks>As return value is used the constant D3D_RETURN_PARAMETER_INDEX.</remarks>
        /// <param name="sourceNode"><dd>  <p>A reference to the <strong><see cref="SharpDX.D3DCompiler.LinkingNode"/></strong> interface for the source linking node.</p> </dd></param>	
        /// <param name="destinationNode"><dd>  <p>A reference to the <strong><see cref="SharpDX.D3DCompiler.LinkingNode"/></strong> interface for the destination linking node.</p> </dd></param>	
        /// <param name="destinationParameterIndex"><dd>  <p>The zero-based index of the destination parameter.</p> </dd></param>	
        /// <returns><p>Returns <see cref="SharpDX.Result.Ok"/> if successful; otherwise, returns one of the Direct3D 11 Return Codes.</p></returns>	
        /// <msdn-id>dn280540</msdn-id>	
        /// <unmanaged>HRESULT ID3D11FunctionLinkingGraph::PassValue([In] ID3D11LinkingNode* pSrcNode,[In] int SrcParameterIndex,[In] ID3D11LinkingNode* pDstNode,[In] int DstParameterIndex)</unmanaged>	
        /// <unmanaged-short>ID3D11FunctionLinkingGraph::PassValue</unmanaged-short>	
        public void PassValue(LinkingNode sourceNode, LinkingNode destinationNode, int destinationParameterIndex)
        {
            PassValue(sourceNode, ReturnParameterIndex, destinationNode, destinationParameterIndex);
        }

        /// <summary>
        /// Generates hlsl code for function linking grpah
        /// </summary>
        /// <param name="uFlags">Flags (unused by the runtime for now)</param>
        /// <returns>Hlsl code as string</returns>
        public string GenerateHlsl(int uFlags)
        {
            SharpDX.Direct3D.Blob blob;
            GenerateHlsl(0, out blob);
            string result = SharpDX.Utilities.BlobToString(blob);
            blob.Dispose();
            return result;
        }
    }
}