Texture2D<float4> tex;
SamplerState sam;

cbuffer CB0
{
    float2 scale;
    float2 offset;
};

struct VS_IN
{
    float2 pos : POSITION;
    float2 tex : TEXCOORD0;
};

struct VS_OUT
{
    float2 tex : TEXCOORD0;
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

float4 PS(float2 coord : TEXCOORD0) : SV_TARGET
{
	float4 val =  tex.Sample(sam, coord);
    float3 color = val.xyz * (1.0f - val.w);
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
