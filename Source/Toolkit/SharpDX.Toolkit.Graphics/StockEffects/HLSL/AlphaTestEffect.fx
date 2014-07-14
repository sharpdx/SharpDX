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
// -----------------------------------------------------------------------------
// The following code is a port of XNA StockEffects http://xbox.create.msdn.com/en-US/education/catalog/sample/stock_effects
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//-----------------------------------------------------------------------------
// AlphaTestEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"


DECLARE_TEXTURE(Texture, 0);


BEGIN_CONSTANTS

    float4 DiffuseColor     _vs(c0) _cb(c0);
    float4 AlphaTest        _ps(c0) _cb(c1);
    float3 FogColor         _ps(c1) _cb(c2);
    float4 FogVector        _vs(c5) _cb(c3);

MATRIX_CONSTANTS

    row_major float4x4 WorldViewProj  _vs(c1) _cb(c0);

END_CONSTANTS


#include "Structures.fxh"
#include "Common.fxh"


// Vertex shader: basic.
VSOutputTx VSAlphaTest(VSInputTx vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: no fog.
VSOutputTxNoFog VSAlphaTestNoFog(VSInputTx vin)
{
    VSOutputTxNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: vertex color.
VSOutputTx VSAlphaTestVc(VSInputTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex color, no fog.
VSOutputTxNoFog VSAlphaTestVcNoFog(VSInputTxVc vin)
{
    VSOutputTxNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Pixel shader: less/greater compare function.
float4 PSAlphaTestLtGt(PSInputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;

    clip((color.a < AlphaTest.x) ? AlphaTest.z : AlphaTest.w);

    ApplyFog(color, pin.Specular.w);

    return color;
}


// Pixel shader: less/greater compare function, no fog.
float4 PSAlphaTestLtGtNoFog(PSInputTxNoFog pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    clip((color.a < AlphaTest.x) ? AlphaTest.z : AlphaTest.w);

    return color;
}


// Pixel shader: equal/notequal compare function.
float4 PSAlphaTestEqNe(PSInputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    clip((abs(color.a - AlphaTest.x) < AlphaTest.y) ? AlphaTest.z : AlphaTest.w);

    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: equal/notequal compare function, no fog.
float4 PSAlphaTestEqNeNoFog(PSInputTxNoFog pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    clip((abs(color.a - AlphaTest.x) < AlphaTest.y) ? AlphaTest.z : AlphaTest.w);

    return color;
}

// ------------------------------------------------------------------------------
// Technique declaration is different with SharpDX Toolkit FX compiler:
// 1) It doesn't support shader compiled outside a Pass
// 2) It doesn't support indexing pre-compiled shader outside a Pass
//
// This is resolved:
// For issue 1) by inlining the shader declarations inside the pass
// For issue 2) using a SubPass concept to encapsulate a set of pass
// into a single pass.
// ------------------------------------------------------------------------------

Technique AlphaTestEffect
{
	Pass {
        EffectName = "Toolkit::AlphaTestEffect";

        // empty to setup the profile for next passes
        Profile = 9.1; 							
        
        // Setup that following pass will be collapsed into this pass as subpass
        // Original version of BasicEffect is switching correct pass at runtime											   
        // so we are supporting this feature as well, but declared slightly differently
        SubPassCount = 8;
    }  

    Pass {  VertexShader = VSAlphaTest        ; PixelShader = PSAlphaTestLtGt       ;}// lt/gt
    Pass {  VertexShader = VSAlphaTestNoFog   ; PixelShader = PSAlphaTestLtGtNoFog  ;}// lt/gt, no fog
    Pass {  VertexShader = VSAlphaTestVc      ; PixelShader = PSAlphaTestLtGt       ;}// lt/gt, vertex color
    Pass {  VertexShader = VSAlphaTestVcNoFog ; PixelShader = PSAlphaTestLtGtNoFog  ;}// lt/gt, vertex color, no fog

    Pass {  VertexShader = VSAlphaTest        ; PixelShader = PSAlphaTestEqNe       ;}// eq/ne
    Pass {  VertexShader = VSAlphaTestNoFog   ; PixelShader = PSAlphaTestEqNeNoFog  ;}// eq/ne, no fog
    Pass {  VertexShader = VSAlphaTestVc      ; PixelShader = PSAlphaTestEqNe       ;}// eq/ne, vertex color
    Pass {  VertexShader = VSAlphaTestVcNoFog ; PixelShader = PSAlphaTestEqNeNoFog  ;}// eq/ne, vertex color, no fog
}
