//-----------------------------------------------------------------------------
// BasicEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"


DECLARE_TEXTURE(Texture, 0);


BEGIN_CONSTANTS

    float4 DiffuseColor             _vs(c0)  _ps(c1)  _cb(c0);
    float3 EmissiveColor            _vs(c1)  _ps(c2)  _cb(c1);
    float3 SpecularColor            _vs(c2)  _ps(c3)  _cb(c2);
    float  SpecularPower            _vs(c3)  _ps(c4)  _cb(c2.w);

    float3 DirLight0Direction       _vs(c4)  _ps(c5)  _cb(c3);
    float3 DirLight0DiffuseColor    _vs(c5)  _ps(c6)  _cb(c4);
    float3 DirLight0SpecularColor   _vs(c6)  _ps(c7)  _cb(c5);

    float3 DirLight1Direction       _vs(c7)  _ps(c8)  _cb(c6);
    float3 DirLight1DiffuseColor    _vs(c8)  _ps(c9)  _cb(c7);
    float3 DirLight1SpecularColor   _vs(c9)  _ps(c10) _cb(c8);

    float3 DirLight2Direction       _vs(c10) _ps(c11) _cb(c9);
    float3 DirLight2DiffuseColor    _vs(c11) _ps(c12) _cb(c10);
    float3 DirLight2SpecularColor   _vs(c12) _ps(c13) _cb(c11);

    float3 EyePosition              _vs(c13) _ps(c14) _cb(c12);

    float3 FogColor                          _ps(c0)  _cb(c13);
    float4 FogVector                _vs(c14)          _cb(c14);

    float4x4 World                  _vs(c19)          _cb(c15);
    float3x3 WorldInverseTranspose  _vs(c23)          _cb(c19);

MATRIX_CONSTANTS

    float4x4 WorldViewProj          _vs(c15)          _cb(c0);

END_CONSTANTS


#include "Structures.fxh"
#include "Common.fxh"
#include "Lighting.fxh"


// Vertex shader: basic.
VSOutput VSBasic(VSInput vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: no fog.
VSOutputNoFog VSBasicNoFog(VSInput vin)
{
    VSOutputNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    return vout;
}


// Vertex shader: vertex color.
VSOutput VSBasicVc(VSInputVc vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex color, no fog.
VSOutputNoFog VSBasicVcNoFog(VSInputVc vin)
{
    VSOutputNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: texture.
VSOutputTx VSBasicTx(VSInputTx vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: texture, no fog.
VSOutputTxNoFog VSBasicTxNoFog(VSInputTx vin)
{
    VSOutputTxNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: texture + vertex color.
VSOutputTx VSBasicTxVc(VSInputTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: texture + vertex color, no fog.
VSOutputTxNoFog VSBasicTxVcNoFog(VSInputTxVc vin)
{
    VSOutputTxNoFog vout;
    
    CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);
    SetCommonVSOutputParamsNoFog;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex lighting.
VSOutput VSBasicVertexLighting(VSInputNm vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: vertex lighting + vertex color.
VSOutput VSBasicVertexLightingVc(VSInputNmVc vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: vertex lighting + texture.
VSOutputTx VSBasicVertexLightingTx(VSInputNmTx vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: vertex lighting + texture + vertex color.
VSOutputTx VSBasicVertexLightingTxVc(VSInputNmTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: one light.
VSOutput VSBasicOneLight(VSInputNm vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    return vout;
}


// Vertex shader: one light + vertex color.
VSOutput VSBasicOneLightVc(VSInputNmVc vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: one light + texture.
VSOutputTx VSBasicOneLightTx(VSInputNmTx vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: one light + texture + vertex color.
VSOutputTx VSBasicOneLightTxVc(VSInputNmTxVc vin)
{
    VSOutputTx vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 1);
    SetCommonVSOutputParams;
    
    vout.TexCoord = vin.TexCoord;
    vout.Diffuse *= vin.Color;
    
    return vout;
}


// Vertex shader: pixel lighting.
VSOutputPixelLighting VSBasicPixelLighting(VSInputNm vin)
{
    VSOutputPixelLighting vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;

    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    
    return vout;
}


// Vertex shader: pixel lighting + vertex color.
VSOutputPixelLighting VSBasicPixelLightingVc(VSInputNmVc vin)
{
    VSOutputPixelLighting vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse.rgb = vin.Color.rgb;
    vout.Diffuse.a = vin.Color.a * DiffuseColor.a;
    
    return vout;
}


// Vertex shader: pixel lighting + texture.
VSOutputPixelLightingTx VSBasicPixelLightingTx(VSInputNmTx vin)
{
    VSOutputPixelLightingTx vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse = float4(1, 1, 1, DiffuseColor.a);
    vout.TexCoord = vin.TexCoord;

    return vout;
}


// Vertex shader: pixel lighting + texture + vertex color.
VSOutputPixelLightingTx VSBasicPixelLightingTxVc(VSInputNmTxVc vin)
{
    VSOutputPixelLightingTx vout;
    
    CommonVSOutputPixelLighting cout = ComputeCommonVSOutputPixelLighting(vin.Position, vin.Normal);
    SetCommonVSOutputParamsPixelLighting;
    
    vout.Diffuse.rgb = vin.Color.rgb;
    vout.Diffuse.a = vin.Color.a * DiffuseColor.a;
    vout.TexCoord = vin.TexCoord;
    
    return vout;
}


// Pixel shader: basic.
float4 PSBasic(PSInput pin) : SV_Target0
{
    float4 color = pin.Diffuse;
    
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: no fog.
float4 PSBasicNoFog(PSInputNoFog pin) : SV_Target0
{
    return pin.Diffuse;
}


// Pixel shader: texture.
float4 PSBasicTx(PSInputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: texture, no fog.
float4 PSBasicTxNoFog(PSInputTxNoFog pin) : SV_Target0
{
    return SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
}


// Pixel shader: vertex lighting.
float4 PSBasicVertexLighting(PSInput pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    AddSpecular(color, pin.Specular.rgb);
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: vertex lighting, no fog.
float4 PSBasicVertexLightingNoFog(PSInput pin) : SV_Target0
{
    float4 color = pin.Diffuse;
    
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}


// Pixel shader: vertex lighting + texture.
float4 PSBasicVertexLightingTx(PSInputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    AddSpecular(color, pin.Specular.rgb);
    ApplyFog(color, pin.Specular.w);
    
    return color;
}


// Pixel shader: vertex lighting + texture, no fog.
float4 PSBasicVertexLightingTxNoFog(PSInputTx pin) : SV_Target0
{
    float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
    
    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}


// Pixel shader: pixel lighting.
float4 PSBasicPixelLighting(PSInputPixelLighting pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    float3 eyeVector = normalize(EyePosition - pin.PositionWS.xyz);
    float3 worldNormal = normalize(pin.NormalWS);
    
    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, 3);

    color.rgb *= lightResult.Diffuse;
    
    AddSpecular(color, lightResult.Specular);
    ApplyFog(color, pin.PositionWS.w);
    
    return color;
}


// Pixel shader: pixel lighting + texture.
float4 PSBasicPixelLightingTx(PSInputPixelLightingTx pin) : SV_Target0
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

Technique BasicEffect
{
	Pass {  
        // empty to setup the profile for next passes
        Profile = 9.1; 							
        
        // Setup that following pass will be collapsed into this pass as subpass
        // Original version of BasicEffect is switching correct pass at runtime											   
        // so we are supporting this feature as well, but declared slightly differently
        SubPassCount = 32;
    }  

	Pass {  VertexShader = VSBasic                   ; PixelShader = PSBasic                      ;}  // basic
	Pass {  VertexShader = VSBasic                   ; PixelShader = PSBasic                      ;}  // no fog
	Pass {  VertexShader = VSBasicNoFog              ; PixelShader = PSBasicTx                    ;}  // vertex color
	Pass {  VertexShader = VSBasicVc                 ; PixelShader = PSBasic                      ;}  // vertex color, no fog
	Pass {  VertexShader = VSBasicVcNoFog            ; PixelShader = PSBasicNoFog                 ;}  // texture
	Pass {  VertexShader = VSBasicTx                 ; PixelShader = PSBasicTx                    ;}  // texture, no fog
	Pass {  VertexShader = VSBasicTxNoFog            ; PixelShader = PSBasicNoFog                 ;}  // texture + vertex color
	Pass {  VertexShader = VSBasicTxVc               ; PixelShader = PSBasicTx                    ;}  // texture + vertex color, no fog
	Pass {  VertexShader = VSBasicTxVcNoFog          ; PixelShader = PSBasicTxNoFog               ;}  // vertex lighting
	Pass {  VertexShader = VSBasicTxVcNoFog          ; PixelShader = PSBasicVertexLighting        ;}  // vertex lighting, no fog
	Pass {  VertexShader = VSBasicVertexLighting     ; PixelShader = PSBasicTxNoFog               ;}  // vertex lighting + vertex color
	Pass {  VertexShader = VSBasicVertexLighting     ; PixelShader = PSBasicVertexLighting        ;}  // vertex lighting + vertex color, no fog
	Pass {  VertexShader = VSBasicVertexLightingVc   ; PixelShader = PSBasicVertexLightingNoFog   ;}  // vertex lighting + texture
	Pass {  VertexShader = VSBasicVertexLightingVc   ; PixelShader = PSBasicVertexLightingTx      ;}  // vertex lighting + texture, no fog
	Pass {  VertexShader = VSBasicVertexLightingTx   ; PixelShader = PSBasicVertexLightingNoFog   ;}  // vertex lighting + texture + vertex color
	Pass {  VertexShader = VSBasicVertexLightingTx   ; PixelShader = PSBasicVertexLightingTx      ;}  // vertex lighting + texture + vertex color, no fog
	Pass {  VertexShader = VSBasicVertexLightingTxVc ; PixelShader = PSBasicTxNoFog               ;}  // one light
	Pass {  VertexShader = VSBasicVertexLightingTxVc ; PixelShader = PSBasicVertexLighting        ;}  // one light, no fog
	Pass {  VertexShader = VSBasicOneLight           ; PixelShader = PSBasicTxNoFog               ;}  // one light + vertex color
	Pass {  VertexShader = VSBasicOneLight           ; PixelShader = PSBasicVertexLighting        ;}  // one light + vertex color, no fog
	Pass {  VertexShader = VSBasicOneLightVc         ; PixelShader = PSBasicVertexLightingNoFog   ;}  // one light + texture
	Pass {  VertexShader = VSBasicOneLightVc         ; PixelShader = PSBasicVertexLightingTx      ;}  // one light + texture, no fog
	Pass {  VertexShader = VSBasicOneLightTx         ; PixelShader = PSBasicVertexLightingNoFog   ;}  // one light + texture + vertex color
	Pass {  VertexShader = VSBasicOneLightTx         ; PixelShader = PSBasicVertexLightingTx      ;}  // one light + texture + vertex color, no fog
	Pass {  VertexShader = VSBasicOneLightTxVc       ; PixelShader = PSBasicVertexLightingTxNoFog ;}  // pixel lighting
	Pass {  VertexShader = VSBasicOneLightTxVc       ; PixelShader = PSBasicVertexLightingTxNoFog ;}  // pixel lighting, no fog
	Pass {  VertexShader = VSBasicPixelLighting      ; PixelShader = PSBasicVertexLightingTxNoFog ;}  // pixel lighting + vertex color
	Pass {  VertexShader = VSBasicPixelLighting      ; PixelShader = PSBasicVertexLightingTxNoFog ;}  // pixel lighting + vertex color, no fog
	Pass {  VertexShader = VSBasicPixelLightingVc    ; PixelShader = PSBasicPixelLighting         ;}  // pixel lighting + texture
	Pass {  VertexShader = VSBasicPixelLightingVc    ; PixelShader = PSBasicPixelLighting         ;}  // pixel lighting + texture, no fog
	Pass {  VertexShader = VSBasicPixelLightingTx    ; PixelShader = PSBasicPixelLighting         ;}  // pixel lighting + texture + vertex color
	Pass {  VertexShader = VSBasicPixelLightingTx    ; PixelShader = PSBasicPixelLighting         ;}  // pixel lighting + texture + vertex color, no fog
}
