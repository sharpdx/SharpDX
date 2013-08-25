// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

// -------------------------------------------------------------
// Test for matrices
// -------------------------------------------------------------
float4x4 Matrices4x4[4];
float4x3 Matrices4x3[4];
float3x3 Matrices3x3[4];
float3x4 Matrices3x4[4];

RWStructuredBuffer<float4x4> MatrixOut;

[numthreads(1, 1, 1)]
void CSMain4x4( uint3 DTid : SV_DispatchThreadID )
{
	MatrixOut[DTid.x] = Matrices4x4[DTid.x];
}

[numthreads(1, 1, 1)]
void CSMain4x3( uint3 DTid : SV_DispatchThreadID )
{
	float4x4 temp;
	temp[0] = float4(Matrices4x3[DTid.x][0], 0);
	temp[1] = float4(Matrices4x3[DTid.x][1], 0);
	temp[2] = float4(Matrices4x3[DTid.x][2], 0);
	temp[3] = float4(Matrices4x3[DTid.x][3], 0);
	MatrixOut[DTid.x] = temp;
}

[numthreads(1, 1, 1)]
void CSMain3x3( uint3 DTid : SV_DispatchThreadID )
{
	float4x4 temp;
	temp[0] = float4(Matrices3x3[DTid.x][0], 0);
	temp[1] = float4(Matrices3x3[DTid.x][1], 0);
	temp[2] = float4(Matrices3x3[DTid.x][2], 0);
	temp[3] = 0;
	MatrixOut[DTid.x] = temp;
}

[numthreads(1, 1, 1)]
void CSMain3x4( uint3 DTid : SV_DispatchThreadID )
{
	float4x4 temp;
	temp[0] = Matrices3x4[DTid.x][0];
	temp[1] = Matrices3x4[DTid.x][1];
	temp[2] = Matrices3x4[DTid.x][2];
	temp[3] = 0;
	MatrixOut[DTid.x] = temp;
}

technique TestMatrices
{
	pass { Profile = 11.0; ComputeShader = CSMain4x4; }
	pass { Profile = 11.0; ComputeShader = CSMain4x3; }
	pass { Profile = 11.0; ComputeShader = CSMain3x3; }
	pass { Profile = 11.0; ComputeShader = CSMain3x4; }
}

// -------------------------------------------------------------
// Test for array scalar 
// -------------------------------------------------------------

float Floats[16];
float2 Floats2[8];
float4 Floats4[4];

RWStructuredBuffer<float> FloatsOut;
RWStructuredBuffer<float2> Floats2Out;
RWStructuredBuffer<float4> Floats4Out;

[numthreads(1, 1, 1)]
void CSMainFloats( uint3 DTid : SV_DispatchThreadID )
{
	FloatsOut[DTid.x] = Floats[DTid.x];
}

[numthreads(1, 1, 1)]
void CSMainFloats2( uint3 DTid : SV_DispatchThreadID )
{
	Floats2Out[DTid.x] = Floats2[DTid.x];
}

[numthreads(1, 1, 1)]
void CSMainFloats4( uint3 DTid : SV_DispatchThreadID )
{
	Floats4Out[DTid.x] = Floats4[DTid.x];
}

technique TestFloats
{
	pass { Profile = 11.0; ComputeShader = CSMainFloats; }
	pass { Profile = 11.0; ComputeShader = CSMainFloats2; }
	pass { Profile = 11.0; ComputeShader = CSMainFloats4; }
}