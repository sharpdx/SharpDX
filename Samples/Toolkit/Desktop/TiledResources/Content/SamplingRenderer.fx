cbuffer VertexShaderConstants
{
    row_major float4x4 ViewMatrix;
    row_major float4x4 ProjectionMatrix;
    row_major float4x4 ModelMatrix;
    float scaleFactor;
};

cbuffer SamplingConstants
{
    float2 EncodeConstants;
};

struct VS_IN
{
    float3 pos : POSITION;
};

struct VS_OUT
{
    float3 tex : TEXCOORD0;
    float3 utan : TANGENT0;
    float3 vtan : TANGENT1;
    float4 pos : SV_POSITION;
};

struct PS_IN
{
    float3 tex : TEXCOORD0;
};

VS_OUT VS(VS_IN input)
{
    float dataScaleFactor = 10.0f;

    VS_OUT output;
    output.tex = normalize(input.pos);
    output.utan = normalize(float3(-input.pos.y, input.pos.x, 0.0f));
    output.vtan = normalize(cross(input.pos, output.utan));
    float4 pos = float4(input.pos, 1.0f);
    float offset = length(pos.xyz) - 1.0f;
    offset /= dataScaleFactor;
    offset *= scaleFactor;
    pos.xyz = normalize(pos.xyz) * (1.0f + offset);
    pos = mul(pos, ViewMatrix);
    pos = mul(pos, ProjectionMatrix);
    output.pos = pos;
    return output;
}

// Render Target contains:
//   R : U texture coordinate at this pixel.
//   G : V texture coordinate at this pixel.
//   B : W texture coordinate at this pixel.
//   A : Encodes calculated texture level of detail at this pixel.

float4 PS(PS_IN input) : SV_TARGET
{
    float4 ret;

    // Save the interpolated texcoords.
    ret.rgb = (float3(1.0f, 1.0f, 1.0f) + input.tex) / 2.0f;

    // Get the texcoord derivatives in target space.
    float3 dtdx = ddx(input.tex);
    float3 dtdy = ddy(input.tex);

    float2 duvdx = float2(0.0f, 0.0f);
    float2 duvdy = float2(0.0f, 0.0f);

    float3 absTex = abs(input.tex);
    if (absTex.x > absTex.y && absTex.x > absTex.z)
    {
        // Major Axis = X.
        float2 texp = -input.tex.yz;
        float2 dtdxp = -dtdx.yz;
        float2 dtdyp = -dtdy.yz;
        duvdx = (input.tex.x * dtdxp - texp * dtdx.x) / (input.tex.x * input.tex.x);
        duvdy = (input.tex.x * dtdyp - texp * dtdy.x) / (input.tex.x * input.tex.x);
    }
    else if (absTex.y > absTex.x && absTex.y > absTex.z)
    {
        // Major Axis = Y.
        float2 texp = -input.tex.xz;
        float2 dtdxp = -dtdx.xz;
        float2 dtdyp = -dtdy.xz;
        duvdx = (input.tex.y * dtdxp - texp * dtdx.y) / (input.tex.y * input.tex.y);
        duvdy = (input.tex.y * dtdyp - texp * dtdy.y) / (input.tex.y * input.tex.y);
    }
    else
    {
        // Major Axis = Z.
        float2 texp = -input.tex.xy;
        float2 dtdxp = -dtdx.xy;
        float2 dtdyp = -dtdy.xy;
        duvdx = (input.tex.z * dtdxp - texp * dtdx.z) / (input.tex.z * input.tex.z);
        duvdy = (input.tex.z * dtdyp - texp * dtdy.z) / (input.tex.z * input.tex.z);
    }

    // Calculate the maximum magnitude of the scaled derivative in texel space.
    float dldx = sqrt(duvdx.x * duvdx.x + duvdy.x * duvdy.x);
    float dldy = sqrt(duvdx.y * duvdx.y + duvdy.y * duvdy.y);
    float derivative = max(dldx, dldy) * 0.5f; // Multiply by 0.5 due to texcube faces spanning -1 to 1.

    // Useful derivative values will range from TargetRatio/ResourceDimension, corresponding to the
    // most detailed mip, to TargetRatio, corresponding to the least detailed MIP. Roughly,
    // derivative = TargetRatio * 2 ^ MipLevel / ResourceDimension. Since the resulting MipLevel is
    // the interesting linear value to extract, we want to encode it as
    // MipLevel = log2(derivative * ResourceDimension / TargetRatio). To encode this in the UNORM
    // range of 0..1, we divide by the total MIP count of the resource.
    // EncodedValue = log2(derivative * ResourceDimension / TargetRatio) / MipCount.
    // The constants (ResourceDimension / TargetRatio) and (MipCount) are stored in
    // EncodeConstants.x and EncodeConstants.y, respectively.
    float encodedLevelOfDetail = log2(derivative * EncodeConstants.x) / EncodeConstants.y;
    ret.a = encodedLevelOfDetail;

    return ret;
}

technique
{
	pass
	{
        Profile = 11.0;
		VertexShader = VS;
		PixelShader = PS;
	} 
}
