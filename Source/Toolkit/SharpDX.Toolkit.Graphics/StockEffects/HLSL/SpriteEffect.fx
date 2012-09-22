//-----------------------------------------------------------------------------
// SpriteEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"


DECLARE_TEXTURE(Texture, 0);


BEGIN_CONSTANTS
MATRIX_CONSTANTS

    float4x4 MatrixTransform    _vs(c0) _cb(c0);

END_CONSTANTS


void SpriteVertexShader(inout float4 color    : COLOR0,
                        inout float2 texCoord : TEXCOORD0,
                        inout float4 position : SV_Position)
{
    position = mul(position, MatrixTransform);
}


float4 SpritePixelShader(float4 color : COLOR0,
                         float2 texCoord : TEXCOORD0) : SV_Target0
{
    return SAMPLE_TEXTURE(Texture, texCoord) * color;
}

// ------------------------------------------------------------------------------
// With SharpDX Toolkit FX compiler, legacy D3D9 profile gets converted to D3D11 shader profiles:
// vs_2_0/ps_2_0 => vs_4_0_level_9_1 / ps_4_0_level_9_1
// vs_3_0/ps_3_0 => vs_4_0_level_9_3 / ps_4_0_level_9_3
// ------------------------------------------------------------------------------

technique SpriteBatch
{
    pass
    {
        EffectName = "Toolkit::SpriteEffect";

        VertexShader = compile vs_2_0 SpriteVertexShader();
        PixelShader  = compile ps_2_0 SpritePixelShader();
    }
}
