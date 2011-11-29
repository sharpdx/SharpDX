// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
Texture2D<float> FromTexture : register(t0);
Texture2D<float2> MinMaxTexture : register(t0);
SamplerState ColorSampler : register(s0);
static const int Width = 1024;
static const int Height = 1024;
static const int BatchCount = 128;
static const int WidthStride = Width/BatchCount;

float4 MinMaxVS( float2 pos : POSITION, out float2 coords : TEXCOORD0  ) : SV_POSITION
{
    coords = float2(pos.x * .5f  + .5f, -pos.y * .5f + .5f);
	return float4(pos, 0.0f, 1.0f);
}

float2 MinMaxBeginPS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	float value1 = FromTexture.SampleLevel(ColorSampler, coords, 0);
	float value2 = FromTexture.SampleLevel(ColorSampler, coords, 0, int2(1,0));
	float value3 = FromTexture.SampleLevel(ColorSampler, coords, 0, int2(0,1));
	float value4 = FromTexture.SampleLevel(ColorSampler, coords, 0, int2(1,1));
	return float2(min(min(value1.x,value2.x),min(value3.x,value4.x)), max(max(value1.x,value2.x),max(value3.x,value4.x)));
}

float2 MinMaxPS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	float2 value1 = MinMaxTexture.SampleLevel(ColorSampler, coords, 0);
	float2 value2 = MinMaxTexture.SampleLevel(ColorSampler, coords, 0, int2(1,0));
	float2 value3 = MinMaxTexture.SampleLevel(ColorSampler, coords, 0, int2(0,1));
	float2 value4 = MinMaxTexture.SampleLevel(ColorSampler, coords, 0, int2(1,1));
	return float2(min(min(value1.x,value2.x),min(value3.x,value4.x)), max(max(value1.y,value2.y),max(value3.y,value4.y)));
}

float4 MinMaxBlendVS( uint vertexId : SV_VertexID, out float2 minmax : MINMAX  ) : SV_POSITION
{
    minmax = float2(-2, -2);
	[unroll]
	for(int i = 0; i < BatchCount; i++) {
		float value = FromTexture.Load(uint3((vertexId % WidthStride) * BatchCount + i, vertexId / WidthStride,0));
		minmax.x = max(-value, minmax.x);
		minmax.y = max( value, minmax.y);
	}
	return float4(0.0f, 0.0f, 0.0f, 1.0f);
}

float4 MinMaxBlendPS( float4 pos : SV_POSITION, float2 minmax : MINMAX  ) : SV_Target
{
	return float4(minmax, 0.0f, 1.0f);
}
