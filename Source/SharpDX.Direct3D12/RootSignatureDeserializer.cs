using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.Direct3D12
{
    public partial class RootSignatureDeserializer
    {
        /// <summary>	
        /// <p> Gets the layout of the root signature. </p>	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12RootSignatureDeserializer::GetRootSignatureDesc']/*"/>	
        /// <msdn-id>dn986887</msdn-id>	
        /// <unmanaged>GetRootSignatureDesc</unmanaged>	
        /// <unmanaged-short>GetRootSignatureDesc</unmanaged-short>	
        /// <unmanaged>const D3D12_ROOT_SIGNATURE_DESC* ID3D12RootSignatureDeserializer::GetRootSignatureDesc()</unmanaged>
        public SharpDX.Direct3D12.RootSignatureDescription RootSignatureDescription => GetRootSignatureDescription2();

        /// <summary>	
        /// <p> Gets the layout of the root signature. </p>	
        /// </summary>	
        /// <returns><p> Returns a reference to a <strong><see cref="SharpDX.Direct3D12.RootSignatureDescription"/></strong> structure that describes the layout of the root signature. </p></returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12RootSignatureDeserializer::GetRootSignatureDesc']/*"/>	
        /// <msdn-id>dn986887</msdn-id>	
        /// <unmanaged>const D3D12_ROOT_SIGNATURE_DESC* ID3D12RootSignatureDeserializer::GetRootSignatureDesc()</unmanaged>	
        /// <unmanaged-short>ID3D12RootSignatureDeserializer::GetRootSignatureDesc</unmanaged-short>	
        internal SharpDX.Direct3D12.RootSignatureDescription GetRootSignatureDescription2()
        {
            var pDesc = this.GetRootSignatureDescription();
            return new RootSignatureDescription(pDesc);
        }
    }
}
