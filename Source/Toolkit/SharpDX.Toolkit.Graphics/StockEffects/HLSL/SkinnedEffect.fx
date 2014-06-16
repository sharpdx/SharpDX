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
// SkinnedEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"

#define SKINNED_EFFECT_MAX_BONES   72


DECLARE_TEXTURE(Texture, 0);


BEGIN_CONSTANTS

    float4 DiffuseColor                         _vs(c0)  _ps(c1)  _cb(c0);
    float3 EmissiveColor                        _vs(c1)  _ps(c2)  _cb(c1);
    float3 SpecularColor                        _vs(c2)  _ps(c3)  _cb(c2);
    float  SpecularPower                        _vs(c3)  _ps(c4)  _cb(c2.w);

    float3 DirLight0Direction                   _vs(c4)  _ps(c5)  _cb(c3);
    float3 DirLight0DiffuseColor                _vs(c5)  _ps(c6)  _cb(c4);
    float3 DirLight0SpecularColor               _vs(c6)  _ps(c7)  _cb(c5);

    float3 DirLight1Direction                   _vs(c7)  _ps(c8)  _cb(c6);
    float3 DirLight1DiffuseColor                _vs(c8)  _ps(c9)  _cb(c7);
    float3 DirLight1SpecularColor               _vs(c9)  _ps(c10) _cb(c8);

    float3 DirLight2Direction                   _vs(c10) _ps(c11) _cb(c9);
    float3 DirLight2DiffuseColor                _vs(c11) _ps(c12) _cb(c10);
    float3 DirLight2SpecularColor               _vs(c12) _ps(c13) _cb(c11);

    float3 EyePosition                          _vs(c13) _ps(c14) _cb(c12);

    float3 FogColor                                      _ps(c0)  _cb(c13);
    float4 FogVector                            _vs(c14)          _cb(c14);

    row_major float4x4 World                              _vs(c19)          _cb(c15);
    row_major float3x3 WorldInverseTranspose              _vs(c23)          _cb(c19);
    
    float4x3 Bones[SKINNED_EFFECT_MAX_BONES]    _vs(c26)          _cb(c22);

MATRIX_CONSTANTS

    row_major float4x4 WorldViewProj                      _vs(c15)          _cb(c0);

END_CONSTANTS


#include "Structures.fxh"
#include "Common.fxh"
#include "Lighting.fxh"


void Skin(inout VSInputNmTxWeights vin, uniform int boneCount)
{
    float4x3 skinning = 0;

    [unroll]
    for (int i = 0; i < boneCount; i++)
    {
        skinning += Bones[vin.Indices[i]] * vin.Weights[i];
    }

    vin.Position.xyz = mul(vin.Position, skinning);
    vin.Normal = mul(vin.Normal, (float3x3)skinning);
}


// Vertex shader: vertex lighting, one bone.
VSOutputTx VSSkinnedVertexLightingOneBone(VSInputNmTxWeights vin)
{
    VSOutputTx vout;
    
    Skin(vin, 1);
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: vertex lighting, two bones.
VSOutputTx VSSkinnedVertexLightingTwoBones(VSInputNmTxWeights vin)
{
    VSOutputTx vout;
    
    Skin(vin, 2);
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: vertex lighting, four bones.
VSOutputTx VSSkinnedVertexLightingFourBones(VSInputNmTxWeights vin)
{
    VSOutputTx vout;
    
    Skin(vin, 4);
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: one light, one bone.
VSOutputTx VSSkinnedOneLightOneBone(VSInputNmTxWeights vin)
{
    VSOutputTx vout;
    
    Skin(vin, 1);

    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: one light, two bones.
VSOutputTx VSSkinnedOneLightTwoBones(VSInputNmTxWeights vin)
{
    VSOutputTx vout;
    
    Skin(vin, 2);

    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: one light, four bones.
VSOutputTx VSSkinnedOneLightFourBones(VSInputNmTxWeights vin)
{
    VSOutputTx vout;
    
    Skin(vin, 4);

    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: pixel lighting, one bone.
VSOutputPixelLightingTx VSSkinnedPixelLightingOneBone(VSInputNmTxWeights vin)
{
    VSOutputPixelLightingTx vout;
    
    Skin(vin, 1);

    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: pixel lighting, two bones.
VSOutputPixelLightingTx VSSkinnedPixelLightingTwoBones(VSInputNmTxWeights vin)
{
    VSOutputPixelLightingTx vout;
    
    Skin(vin, 2);

    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: pixel lighting, four bones.
VSOutputPixelLightingTx VSSkinnedPixelLightingFourBones(VSInputNmTxWeights vin)
{
    VSOutputPixelLightingTx vout;
    
    Skin(vin, 4);

    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Pixel shader: vertex lighting.
float4 PSSkinnedVertexLighting(PSInputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    AddSpecular(color, pin.Specular.rgb);
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: vertex lighting, no fog.
float4 PSSkinnedVertexLightingNoFog(PSInputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}


// Pixel shader: pixel lighting.
float4 PSSkinnedPixelLighting(PSInputPixelLightingTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = normalize(pin.NormalWS);
    
    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, 3);
    
    color.rgb *= lightResult.Diffuse;

    AddSpecular(color, lightResult.Specular);
    ApplyFog(color, pin.PositionWS.w);
    
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

Technique SkinnedEffect
{
	Pass {  
        EffectName = "Toolkit::SkinnedEffect";

        // empty to setup the profile for next passes
        Profile = 9.1; 							
        
        // Setup that following pass will be collapsed into this pass as subpass
        // Original version of BasicEffect is switching correct pass at runtime											   
        // so we are supporting this feature as well, but declared slightly differently
        SubPassCount = 18;
    }  

    Pass {  VertexShader = VSSkinnedVertexLightingOneBone   ; PixelShader = PSSkinnedVertexLighting       ;}  // vertex lighting, one bone
    Pass {  VertexShader = VSSkinnedVertexLightingOneBone   ; PixelShader = PSSkinnedVertexLightingNoFog  ;}  // vertex lighting, one bone, no fog
    Pass {  VertexShader = VSSkinnedVertexLightingTwoBones  ; PixelShader = PSSkinnedVertexLighting       ;}  // vertex lighting, two bones
    Pass {  VertexShader = VSSkinnedVertexLightingTwoBones  ; PixelShader = PSSkinnedVertexLightingNoFog  ;}  // vertex lighting, two bones, no fog
    Pass {  VertexShader = VSSkinnedVertexLightingFourBones ; PixelShader = PSSkinnedVertexLighting       ;}  // vertex lighting, four bones
    Pass {  VertexShader = VSSkinnedVertexLightingFourBones ; PixelShader = PSSkinnedVertexLightingNoFog  ;}  // vertex lighting, four bones, no fog

    Pass {  VertexShader = VSSkinnedOneLightOneBone         ; PixelShader = PSSkinnedVertexLighting       ;}  // one light, one bone
    Pass {  VertexShader = VSSkinnedOneLightOneBone         ; PixelShader = PSSkinnedVertexLightingNoFog  ;}  // one light, one bone, no fog
    Pass {  VertexShader = VSSkinnedOneLightTwoBones        ; PixelShader = PSSkinnedVertexLighting       ;}  // one light, two bones
    Pass {  VertexShader = VSSkinnedOneLightTwoBones        ; PixelShader = PSSkinnedVertexLightingNoFog  ;}  // one light, two bones, no fog
    Pass {  VertexShader = VSSkinnedOneLightFourBones       ; PixelShader = PSSkinnedVertexLighting       ;}  // one light, four bones
    Pass {  VertexShader = VSSkinnedOneLightFourBones       ; PixelShader = PSSkinnedVertexLightingNoFog  ;}  // one light, four bones, no fog

    Pass {  VertexShader = VSSkinnedPixelLightingOneBone    ; PixelShader = PSSkinnedPixelLighting        ;}  // pixel lighting, one bone
    Pass {  VertexShader = VSSkinnedPixelLightingOneBone    ; PixelShader = PSSkinnedPixelLighting        ;}  // pixel lighting, one bone, no fog
    Pass {  VertexShader = VSSkinnedPixelLightingTwoBones   ; PixelShader = PSSkinnedPixelLighting        ;}  // pixel lighting, two bones
    Pass {  VertexShader = VSSkinnedPixelLightingTwoBones   ; PixelShader = PSSkinnedPixelLighting        ;}  // pixel lighting, two bones, no fog
    Pass {  VertexShader = VSSkinnedPixelLightingFourBones  ; PixelShader = PSSkinnedPixelLighting        ;}  // pixel lighting, four bones
    Pass {  VertexShader = VSSkinnedPixelLightingFourBones  ; PixelShader = PSSkinnedPixelLighting        ;}  // pixel lighting, four bones, no fog
}
