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
namespace SharpDX.Direct3D12
{
    public partial class CommandSignatureDescription
    {
        /// <summary>	
        /// <dd> <p> An array of <strong><see cref="SharpDX.Direct3D12.IndirectArgumentDescription"/></strong> structures, containing details of the arguments, including whether the argument is a vertex buffer, constant, constant buffer view, shader resource view, or unordered access view. </p> </dd>	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_COMMAND_SIGNATURE_DESC::pArgumentDescs']/*"/>	
        /// <msdn-id>dn986724</msdn-id>	
        /// <unmanaged>const D3D12_INDIRECT_ARGUMENT_DESC* pArgumentDescs</unmanaged>	
        /// <unmanaged-short>D3D12_INDIRECT_ARGUMENT_DESC pArgumentDescs</unmanaged-short>	
        public IndirectArgumentDescription[] IndirectArguments { get; set; }
    }
}