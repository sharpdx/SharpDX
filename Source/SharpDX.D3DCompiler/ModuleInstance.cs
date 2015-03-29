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
    /// <summary>	
    /// <p>[This documentation is preliminary and is subject to change.]</p><p>Rebinds a resource by name as an unordered access view (UAV) to destination slots.</p>	
    /// </summary>	
    /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D11ModuleInstance']/*"/>	
    /// <msdn-id>dn280569</msdn-id>	
    /// <unmanaged>ID3D11ModuleInstance</unmanaged>	
    /// <unmanaged-short>ID3D11ModuleInstance</unmanaged-short>	
    public partial class ModuleInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInstance"/> class.
        /// </summary>
        /// <param name="module"><p>The address of a reference to an <strong><see cref="SharpDX.D3DCompiler.ModuleInstance"/></strong> interface to initialize.</p></param>
        public ModuleInstance(Module module)
            : this(string.Empty, module)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInstance"/> class.
        /// </summary>
        /// <param name="namespaceRef"><p>The name of a shader module to initialize. This can be <strong><c>null</c></strong> if you don't want to specify a name for the module.</p></param>
        /// <param name="module"><p>The address of a reference to an <strong><see cref="SharpDX.D3DCompiler.ModuleInstance"/></strong> interface to initialize.</p></param>
        public ModuleInstance(string namespaceRef, Module module)
        {
            if (module == null) throw new ArgumentNullException("module");

            module.CreateInstance(namespaceRef, this);
        }
    }
}