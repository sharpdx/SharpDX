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
    /// <p>[This documentation is preliminary and is subject to change.]</p><p>Links the shader and produces a shader blob that the Direct3D runtime can use.</p>	
    /// </summary>	
    /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D11Linker']/*"/>	
    /// <msdn-id>dn280560</msdn-id>	
    /// <unmanaged>ID3D11Linker</unmanaged>	
    /// <unmanaged-short>ID3D11Linker</unmanaged-short>	
    public partial class Linker
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Linker"/> class.
        /// </summary>
        public Linker()
        {
            D3D.CreateLinker(this);
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Links the shader and produces a shader blob that the Direct3D runtime can use.</p>	
        /// </summary>	
        /// <param name="module"><dd>  <p>A reference to the <strong><see cref="SharpDX.D3DCompiler.ModuleInstance"/></strong> interface for the shader module instance to link from.</p> </dd></param>	
        /// <param name="entryPointName"><dd>  <p>The name of the shader module instance to link from.</p> </dd></param>	
        /// <param name="targetName"><dd>  <p>The name for the shader blob that is produced.</p> </dd></param>	
        /// <param name="flags"><dd>  <p>Reserved</p> </dd></param>	
        /// <returns><p>Returns the compiled <see cref="ShaderBytecode"/>.</p></returns>	
        /// <msdn-id>dn280560</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Linker::Link([In] ID3D11ModuleInstance* pEntry,[In] const char* pEntryName,[In] const char* pTargetName,[In] unsigned int uFlags,[Out] ID3D10Blob** ppShaderBlob,[Out] ID3D10Blob** ppErrorBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Linker::Link</unmanaged-short>	
        /// <exception cref="CompilationException">Is thrown when linking fails and the error text is available.</exception>
        /// <exception cref="SharpDXException">Is thrown when linking fails and the error text is <b>not</b> available.</exception>
        public ShaderBytecode Link(ModuleInstance module, string entryPointName, string targetName, int flags)
        {
            Blob shaderBlob;
            Blob errorBlob;

            var resultCode = Link(module, entryPointName, targetName, flags, out shaderBlob, out errorBlob);

            if (resultCode.Failure)
            {
                if (errorBlob != null)
                    throw new CompilationException(resultCode, Utilities.BlobToString(errorBlob));

                throw new SharpDXException(resultCode);
            }

            return new ShaderBytecode(shaderBlob);
        }
    }
}