//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

Texture2D InputTexture : register(t0);

SamplerState InputSampler : register(s0);

cbuffer constants : register(b0)
{
    float frequency : packoffset(c0.x);
    float phase     : packoffset(c0.y);
    float amplitude : packoffset(c0.z);
    float spread    : packoffset(c0.w);
    float2 center   : packoffset(c1);
};

float4 main(
    float4 pos      : SV_POSITION,
    float4 posScene : SCENE_POSITION,
    float4 uv0      : TEXCOORD0
    ) : SV_Target
{
    float2 wave;

    float2 toPixel = posScene.xy - center; 

    float distance = length(toPixel) * uv0.z;
    float2 direction = normalize(toPixel);

    sincos(frequency * distance + phase, wave.x, wave.y);

    // Clamps the distance between 0 and 1 and squares the value.
    float falloff = saturate(1 - distance);
    falloff = pow(falloff, 1.0f / spread);

    // Calculates new mapping coordinates based on the frequency, center, and amplitude.
    float2 uv2 = uv0.xy + (wave.x * falloff * amplitude) * direction * uv0.zw;

    float lighting = lerp(1.0f, 1.0f + wave.x * falloff * 0.2f, saturate(amplitude / 20.0f));
            
    // Resamples the image based on the new coordinates.
    float4 color = InputTexture.Sample(InputSampler, uv2);
    color.rgb *= lighting;
    
    return color;
}