// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
// EnvironmentMapEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"


DECLARE_TEXTURE(Texture, 0);
DECLARE_CUBEMAP(EnvironmentMap, 1);


BEGIN_CONSTANTS

    float3 EnvironmentMapSpecular   _ps(c0)  _cb(c0);
    float  FresnelFactor            _vs(c0)  _cb(c0.w);
    float  EnvironmentMapAmount     _vs(c1)  _cb(c2.w);

    float4 DiffuseColor             _vs(c2)  _cb(c1);
    float3 EmissiveColor            _vs(c3)  _cb(c2);

    float3 DirLight0Direction       _vs(c4)  _cb(c3);
    float3 DirLight0DiffuseColor    _vs(c5)  _cb(c4);

    float3 DirLight1Direction       _vs(c6)  _cb(c5);
    float3 DirLight1DiffuseColor    _vs(c7)  _cb(c6);

    float3 DirLight2Direction       _vs(c8)  _cb(c7);
    float3 DirLight2DiffuseColor    _vs(c9)  _cb(c8);

    float3 EyePosition              _vs(c10) _cb(c9);

    float3 FogColor                 _ps(c1)  _cb(c10);
    float4 FogVector                _vs(c11) _cb(c11);

    float4x4 World                  _vs(c16) _cb(c12);
    float3x3 WorldInverseTranspose  _vs(c19) _cb(c16);

MATRIX_CONSTANTS

    float4x4 WorldViewProj          _vs(c12) _cb(c0);

END_CONSTANTS


// We don't use these parameters, but Lighting.fxh won't compile without them.
#define SpecularPower           0
#define SpecularColor           float3(0, 0, 0)
#define DirLight0SpecularColor  float3(0, 0, 0)
#define DirLight1SpecularColor  float3(0, 0, 0)
#define DirLight2SpecularColor  float3(0, 0, 0)


#include "Structures.fxh"
#include "Common.fxh"
#include "Lighting.fxh"


float ComputeFresnelFactor(float3 eyeVector, float3 worldNormal)
{
    float viewAngle = dot(eyeVector, worldNormal);
    
    return pow(max(1 - abs(viewAngle), 0), FresnelFactor) * EnvironmentMapAmount;
}


VSOutputTxEnvMap ComputeEnvMapVSOutput(VSInputNmTx vin, uniform bool useFresnel, uniform int numLights)
{
    VSOutputTxEnvMap vout;
    
    float4 pos_ws = mul(vin.Position, World);
    float3 eyeVector = normalize(EyePosition - pos_ws.xyz);
    float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, numLights);
    
    vout.PositionPS = mul(vin.Position, WorldViewProj);
    vout.Diffuse = float4(lightResult.Diffuse, DiffuseColor.a);
    
    if (useFresnel)
        vout.Specular.rgb = ComputeFresnelFactor(eyeVector, worldNormal);
    else
        vout.Specular.rgb = EnvironmentMapAmount;
    
    vout.Specular.a = ComputeFogFactor(vin.Position);
    vout.TexCoord = vin.TexCoord;
    vout.EnvCoord = reflect(-eyeVector, worldNormal);

    return vout;
}


// Vertex shader: basic.
VSOutputTxEnvMap VSEnvMap(VSInputNmTx vin)
{
    return ComputeEnvMapVSOutput(vin, false, 3);
}


// Vertex shader: fresnel.
VSOutputTxEnvMap VSEnvMapFresnel(VSInputNmTx vin)
{
    return ComputeEnvMapVSOutput(vin, true, 3);
}


// Vertex shader: one light.
VSOutputTxEnvMap VSEnvMapOneLight(VSInputNmTx vin)
{
    return ComputeEnvMapVSOutput(vin, false, 1);
}


// Vertex shader: one light, fresnel.
VSOutputTxEnvMap VSEnvMapOneLightFresnel(VSInputNmTx vin)
{
    return ComputeEnvMapVSOutput(vin, true, 1);
}


// Pixel shader: basic.
float4 PSEnvMap(PSInputTxEnvMap pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    float4 envmap = SAMPLE_CUBEMAP(EnvironmentMap, pin.EnvCoord) * color.a;

    color.rgb = lerp(color.rgb, envmap.rgb, pin.Specular.rgb);
    
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: no fog.
float4 PSEnvMapNoFog(PSInputTxEnvMap pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    float4 envmap = SAMPLE_CUBEMAP(EnvironmentMap, pin.EnvCoord) * color.a;

    color.rgb = lerp(color.rgb, envmap.rgb, pin.Specular.rgb);
    
    return color;
}


// Pixel shader: specular.
float4 PSEnvMapSpecular(PSInputTxEnvMap pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    float4 envmap = SAMPLE_CUBEMAP(EnvironmentMap, pin.EnvCoord) * color.a;

    color.rgb = lerp(color.rgb, envmap.rgb, pin.Specular.rgb);
    color.rgb += EnvironmentMapSpecular * envmap.a;
    
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: specular, no fog.
float4 PSEnvMapSpecularNoFog(PSInputTxEnvMap pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    float4 envmap = SAMPLE_CUBEMAP(EnvironmentMap, pin.EnvCoord) * color.a;

    color.rgb = lerp(color.rgb, envmap.rgb, pin.Specular.rgb);
    color.rgb += EnvironmentMapSpecular * envmap.a;
    
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

Technique EnvironmentMapEffect
{
	Pass {  
        EffectName = "Toolkit::EnvironmentMapEffect";

        // empty to setup the profile for next passes
        Profile = 9.1; 							
        
        // Setup that following pass will be collapsed into this pass as subpass
        // Original version of BasicEffect is switching correct pass at runtime											   
        // so we are supporting this feature as well, but declared slightly differently
        SubPassCount = 16;
    }  

    Pass {  VertexShader = VSEnvMap                ; PixelShader = PSEnvMap              ;}  // basic
    Pass {  VertexShader = VSEnvMap                ; PixelShader = PSEnvMapNoFog         ;}  // basic, no fog
    Pass {  VertexShader = VSEnvMapFresnel         ; PixelShader = PSEnvMap              ;}  // fresnel
    Pass {  VertexShader = VSEnvMapFresnel         ; PixelShader = PSEnvMapNoFog         ;}  // fresnel, no fog
    Pass {  VertexShader = VSEnvMap                ; PixelShader = PSEnvMapSpecular      ;}  // specular
    Pass {  VertexShader = VSEnvMap                ; PixelShader = PSEnvMapSpecularNoFog ;}  // specular, no fog
    Pass {  VertexShader = VSEnvMapFresnel         ; PixelShader = PSEnvMapSpecular      ;}  // fresnel + specular
    Pass {  VertexShader = VSEnvMapFresnel         ; PixelShader = PSEnvMapSpecularNoFog ;}  // fresnel + specular, no fog

    Pass {  VertexShader = VSEnvMapOneLight        ; PixelShader = PSEnvMap              ;}  // one light
    Pass {  VertexShader = VSEnvMapOneLight        ; PixelShader = PSEnvMapNoFog         ;}  // one light, no fog
    Pass {  VertexShader = VSEnvMapOneLightFresnel ; PixelShader = PSEnvMap              ;}  // one light, fresnel
    Pass {  VertexShader = VSEnvMapOneLightFresnel ; PixelShader = PSEnvMapNoFog         ;}  // one light, fresnel, no fog
    Pass {  VertexShader = VSEnvMapOneLight        ; PixelShader = PSEnvMapSpecular      ;}  // one light, specular
    Pass {  VertexShader = VSEnvMapOneLight        ; PixelShader = PSEnvMapSpecularNoFog ;}  // one light, specular, no fog
    Pass {  VertexShader = VSEnvMapOneLightFresnel ; PixelShader = PSEnvMapSpecular      ;}  // one light, fresnel + specular
    Pass {  VertexShader = VSEnvMapOneLightFresnel ; PixelShader = PSEnvMapSpecularNoFog ;}  // one light, fresnel + specular, no fog
}
