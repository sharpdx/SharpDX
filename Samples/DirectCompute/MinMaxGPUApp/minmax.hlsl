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
static const uint Width = WIDTH;
static const uint Height = HEIGHT;
static const uint BatchSize = MINMAX_BATCH_COUNT;

float4 MipMapMinMaxVS( float2 pos : POSITION, out float2 coords : TEXCOORD0  ) : SV_POSITION
{
    coords = float2(pos.x * .5f  + .5f, -pos.y * .5f + .5f);
	return float4(pos, 0.0f, 1.0f);
}

float2 MipMapMinMaxSampleBegin(float2 coords, int size) {
	float2 minmax;
	[unroll]
	for(int i = 0; i < size; i++) {
		[unroll]
		for(int j = 0; j < size; j++)  {
			if (i == 0 && j == 0 ) {
				float value = FromTexture.SampleLevel(ColorSampler, coords, 0);
				minmax = float2(value, value);
			}
			else 
			{
				float value = FromTexture.SampleLevel(ColorSampler, coords, 0, int2(j,i));
				minmax = float2(min(value, minmax.x), max(value, minmax.y));
			}
		}
	}
	return minmax;
}

float2 MipMapMinMaxSample(float2 coords, int size) {
	float2 minmax;
	[unroll]
	for(int i = 0; i < size; i++) {
		[unroll]
		for(int j = 0; j < size; j++)  {
			if (i == 0 && j == 0 ) {
				minmax = MinMaxTexture.SampleLevel(ColorSampler, coords, 0);
			}
			else 
			{
				float2 value = MinMaxTexture.SampleLevel(ColorSampler, coords, 0, int2(j,i));
				minmax = float2(min(value.x, minmax.x), max(value.y, minmax.y));
			}
		}
	}
	return minmax;
}

float2 MipMapMinMaxBegin1PS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	return MipMapMinMaxSampleBegin(coords, 2);
}

float2 MipMapMinMax1PS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	return MipMapMinMaxSample(coords, 2);
}

float2 MipMapMinMaxBegin2PS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	return MipMapMinMaxSampleBegin(coords, 4);
}

float2 MipMapMinMax2PS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	return MipMapMinMaxSample(coords, 4);
}

float2 MipMapMinMaxBegin3PS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	return MipMapMinMaxSampleBegin(coords, 8);
}

float2 MipMapMinMax3PS( float4 pos : SV_POSITION, float2 coords : TEXCOORD0 ) : SV_Target
{
	return MipMapMinMaxSample(coords, 8);
}


struct VS_OUTPUT { 
	float2 minmax : MINMAX;
};

static const uint VertexPerRow = Width/BatchSize;
VS_OUTPUT VertexBlendMinMaxVS( uint vertexId : SV_VertexID)
{
	VS_OUTPUT output;
    output.minmax = float2(0,0);
	uint2 sampleCoord = uint2((vertexId % VertexPerRow) * BatchSize, vertexId / VertexPerRow);
	[unroll]
	for(uint i = 0; i < BatchSize; i++) {
		float value = FromTexture[sampleCoord + uint2(i, 0)];
		if (i == 0) {
			output.minmax.xy = value.xx;
		}
		else
		{
			output.minmax = float2(min(value, output.minmax.x), max(value, output.minmax.y));
		}
	}
	return output;
}

//static const uint VertexPerRow = Width/BatchSize;
//VS_OUTPUT VertexBlendMinMaxVS( uint vertexId : SV_VertexID)
//{
	//VS_OUTPUT output;
//
    //output.minmax = float2(0,0);
	//uint2 sampleCoord = uint2(vertexId % VertexPerRow, vertexId / VertexPerRow) * BatchSize;
	//for(uint i = 0; i < BatchSize; i++) {
		//[unroll]
		//for(uint j = 0; j < BatchSize; j++) {
			//float value = FromTexture[sampleCoord + uint2(j,i)];
			//if (i == 0 && j == 0) {
				//output.minmax.xy = value.xx;
			//}
			//else
			//{
				//output.minmax = float2(min(value, output.minmax.x), max(value, output.minmax.y));
			//}
		//}
	//}
	//output.pos = float4(0, 0, 0, 1);
	//return output;
//}
//
struct GS_OUTPUT { 
	float4 pos : SV_POSITION;
	nointerpolation float2 minmax : MINMAX;
};

[maxvertexcount(1)]
void VertexBlendMinMaxGS( InputPatch<VS_OUTPUT,32> input, inout PointStream<GS_OUTPUT> stream )
{
	GS_OUTPUT output;
	output.pos = float4(0, 0, 0, 1);
	[unroll]
	for(int i = 0; i < BatchSize; i++) {
		if (i == 0 ) {
			output.minmax = input[0].minmax;
		}
		else
		{
			output.minmax = float2(min(input[i].minmax.x, output.minmax.x), max(input[i].minmax.y, output.minmax.y));
		}
	}
	stream.Append(output);
	stream.RestartStrip();
}

float4 VertexBlendMinMaxPS( float4 pos : SV_POSITION, nointerpolation float2 minmax : MINMAX  ) : SV_Target
{
	return float4(-minmax.x, minmax.y, 0.0, 0.0);
}
