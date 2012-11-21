// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
struct VS_IN
{
	float4 pos : POSITION;
	float4 col : COLOR;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
};

SamplerState samp;

// Test Texture Array
Texture2D textureStd;
Texture2D testTextureArray[4];
float4 PSTextureArray( PS_IN input ) : SV_Target
{
	// Try to only use part of the declaration
	return testTextureArray[3].SampleLevel(samp, float2(0.5, 0.5), 0) +
		   textureStd.SampleLevel(samp, float2(0.5, 0.5), 0) +
		   testTextureArray[2].SampleLevel(samp, float2(0.5, 0.5), 0) +
		   testTextureArray[1].SampleLevel(samp, float2(0.5, 0.5), 0);
}

technique TextureArray
{
	pass
	{
		// Compile with ps_4_0
		Profile = 10.0;
		PixelShader = PSTextureArray;
	}

	pass
	{
		// Compile with ps_5_0
		Profile = 11.0;
		PixelShader = PSTextureArray;
	}
}




