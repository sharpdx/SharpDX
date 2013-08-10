TextureCube<float3> ColorTexture;
TextureCube<float> ColorResidency;
TextureCube<float2> NormalTexture;
TextureCube<float> NormalResidency;

SamplerState Trilinear;
SamplerState MaxFilter;

cbuffer VertexShaderConstants
{
    row_major float4x4 ViewMatrix;
    row_major float4x4 ProjectionMatrix;
    row_major float4x4 ModelMatrix;
    float scaleFactor;
};

cbuffer PixelShaderConstants
{
    float4 SunPosition;
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
    float3 utan : TANGENT0;
    float3 vtan : TANGENT1;
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

float4 PS(PS_IN input) : SV_TARGET
{
	float3 tex = normalize(input.tex);

    // Gather can be used to emulate the MAXIMUM filter variants when on Tier 1.
    float4 normalSampleValues = NormalResidency.Gather(MaxFilter, tex) * 16.0f;
    float normalMinLod = normalSampleValues.x;
    normalMinLod = max(normalMinLod, normalSampleValues.y);
    normalMinLod = max(normalMinLod, normalSampleValues.z);
    normalMinLod = max(normalMinLod, normalSampleValues.w);
    float4 diffuseSampleValues = ColorResidency.Gather(MaxFilter, tex) * 16.0f;
    float diffuseMinLod = diffuseSampleValues.x;
    diffuseMinLod = max(diffuseMinLod, diffuseSampleValues.y);
    diffuseMinLod = max(diffuseMinLod, diffuseSampleValues.z);
    diffuseMinLod = max(diffuseMinLod, diffuseSampleValues.w);
    // SampleLevel in conjunction with CalculateLevelOfDetail can be used to emulate LOD Clamp behavior when on Tier 1.
    float diffuseCalculatedLod = ColorTexture.CalculateLevelOfDetail(Trilinear, tex);
    float3 diffuse = diffuseCalculatedLod < diffuseMinLod ? ColorTexture.SampleLevel(Trilinear, tex, diffuseMinLod) : ColorTexture.Sample(Trilinear, tex);
    float normalCalculatedLod = NormalTexture.CalculateLevelOfDetail(Trilinear, tex);
    float2 tangent = normalCalculatedLod < normalMinLod ? NormalTexture.SampleLevel(Trilinear, tex, normalMinLod) : NormalTexture.Sample(Trilinear, tex);

    float dataScaleFactor = 10.0f;
    float scaleFactor = SunPosition.w;

    float3 normal = tangent.x * input.utan + tangent.y * input.vtan;
    float arg = 1.0f - tangent.x * tangent.x - tangent.y * tangent.y;
    if (arg > 0.0f)
    {
        normal += (dataScaleFactor / scaleFactor)  * sqrt(arg) * cross(input.utan, input.vtan);
    }

    normal = normalize(normal);
    float ambient = 0.2f;
    float lighting = ambient + (1.0f - ambient) * saturate(dot(normal,SunPosition.xyz));

    float3 dustColor = float3(0.92f, 0.65f, 0.41f);

    float3 litColor = diffuse * lighting;

    float3 finalColor = lerp(litColor, dustColor, 0.5f);
    finalColor = litColor;

    return float4(finalColor, 1.0f);
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
