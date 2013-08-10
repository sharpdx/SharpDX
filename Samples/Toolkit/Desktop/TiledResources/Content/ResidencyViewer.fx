TextureCube<float> tex;
SamplerState sam;

cbuffer CB0
{
    float2 scale;
    float2 offset;
};

struct VS_IN
{
    float2 pos : POSITION;
    float4 tex : TEXCOORD0;
};

struct VS_OUT
{
    float4 tex : TEXCOORD0;
    float4 pos : SV_POSITION;
};

VS_OUT VS(VS_IN input)
{
    VS_OUT output;
    output.tex = input.tex;
    output.pos.xy = input.pos * scale + offset;
    output.pos.z = 0.5f;
    output.pos.w = 1.0f;
	return output;
}

float4 PS(float4 coord : TEXCOORD0) : SV_TARGET
{
    float val = 1.0f - tex.Sample(sam, normalize(coord.xyz));
    float3 color = float3(0.0f, 0.0f, 0.0f);
    if (val < 0.25f)
    {
        color.r = 1.0f;
        color.g = val / 0.25f;
    }
    else if (val < 0.5f)
    {
        color.g = 1.0f;
        color.r = 1.0f - (val - 0.25f) / 0.25f;
    }
    else if (val < 0.75f)
    {
        color.g = 1.0f;
        color.b = (val - 0.5f) / 0.25f;
    }
    else
    {
        color.b = 1.0f;
        color.g = 1.0f - (val - 0.75f) / 0.25f;
    }
    return float4(color, 1.0f);
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
