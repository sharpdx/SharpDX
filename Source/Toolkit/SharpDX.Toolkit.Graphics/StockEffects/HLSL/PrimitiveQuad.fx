//-----------------------------------------------------------------------------
// PrimitiveQuad.fx
// Defines the global vertex shader: Toolkit::PrimitiveQuad::VS
// to be reusable.
//-----------------------------------------------------------------------------
#include "Macros.fxh"

DECLARE_TEXTURE(Texture, 0);

BEGIN_CONSTANTS
MATRIX_CONSTANTS
    row_major float4x4 MatrixTransform    _vs(c0) _cb(c0);
END_CONSTANTS

void VS(inout float2 texCoord : TEXCOORD0, inout float4 position : SV_Position)
{
    position = mul(position, MatrixTransform);
}

float4 PS(float2 texCoord : TEXCOORD0) : SV_Target0
{
    return SAMPLE_TEXTURE(Texture, texCoord);
}

technique SpriteBatch
{
    pass
    {
        EffectName = "Toolkit::PrimitiveQuad";
		Profile = 9.1;
		Export = VS;	// This will export VS to "Toolkit::PrimitiveQuad::VS"
		VertexShader = VS;
    }

    pass
    {
		Profile = 9.1;
		VertexShader = VS;
		PixelShader = PS;
    }
}
