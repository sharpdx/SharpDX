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

// constant buffer variables
float4x4 worldViewProj;

// binded resources
Texture2D tex : register(t0);
Texture2D tex1 : register(t1);
Texture2D tex2 : register(t2);
Texture2D tex3 : register(t3);
SamplerState samp;

// -----------------------------------------------
float4 VS3( uint id : SV_VERTEXID ) : SV_POSITION
{
	return id;
}


// -----------------------------------------------
// VertexShader: VS
// -----------------------------------------------
PS_IN VS( VS_IN input)
{
	PS_IN output = (PS_IN)0;
	
	if (ACTIVATE_DUMMY_CODE) 
	{
		output.pos = mul(input.pos, worldViewProj) + 
			tex.SampleLevel(samp, float2(0.5, 0.5), 0) +
			tex1.SampleLevel(samp, float2(0.5, 0.5), 0) +
			tex2.SampleLevel(samp, float2(0.5, 0.5), 0) +
			tex3.SampleLevel(samp, float2(0.5, 0.5), 0);			
	}
	else
	{
		output.pos = input.pos;
	}

	output.col = input.col;
	
	return output;
}

// -----------------------------------------------
// VertexShader: VS2
// -----------------------------------------------
PS_IN VS2( VS_IN input)
{
	// Call to VS, but this function VS2 is compiled with a different ACTIVATE_DUMMY_CODE define
	return VS(input);
}

// -----------------------------------------------
// PixelShader: PS
// -----------------------------------------------
float4 PS( PS_IN input ) : SV_Target
{
	return input.col;
}

// Test using Toolkit FX new pass format.
// The format is specific to SharpDX Toolkit and is not compatible with legacy Direct3D9/Direct3D10/11 FX files.
technique
{
	pass t1 {
		// Defines SM profile for all valid for all shaders compiled in this file. Can be overloaded at any time.
		// Unlike traditional FX file, we don't specify vs_4_0 for vertex shader and ps_4_0 for pixel shader
		// but only at a global level. 
		//
		// Using is equivalent to define the following profile for each stage:
		// 9.1  => vs_4_0_level_9_1, ps_4_0_level_9_1 ...etc.
		// 9.2  => vs_4_0_level_9_2, ps_4_0_level_9_2 ...etc.
		// 9.3  => vs_4_0_level_9_3, ps_4_0_level_9_3 ...etc.
		// 10.0 => vs_4_0, ps_4_0 ...etc.
		// 10.1 => vs_4_1, ps_4_1 ...etc.
		// 11.0 => vs_5_0, ps_5_0 ...etc.
		// 11.1 => vs_5_1, ps_5_1 ...etc.
		Profile = 10.0; 

		// EffectName = "NewEffect"; // Use this to override the name of this whole effect file. By default, it is using the filename "TestEffect".

		// This is the way to modify this file by specifying some preprocessing directive just before trying 
		// to compile the shaders VS, PS
		Preprocessor = "#define ACTIVATE_DUMMY_CODE true";

		// Specify the pipeline (there is also all Direct3D11 other stages: DomainShader, HullShader, GeometryShader, ComputeShader).
		VertexShader = VS; 
		PixelShader = PS; 
	}

	pass t2 {
		// Multiple line defines. Can also #include or whatever valid HLSL code in the strings.
		Preprocessor = {"#define ACTIVATE_DUMMY_CODE false", "#define ANOTHER_DEFINED_NOT_USED"};

		// By default, all compiled shaders are private to an effect. But a particular shader can be reused
		// in another effect. To export a shader publicly, we just need to use the Export directive:
		Export = VS2;			// Export VS2 function as a TestEffect::VS2

		// Specify the pipeline (there is also all Direct3D11 other stages: DomainShader, HullShader, GeometryShader, ComputeShader).
		VertexShader = VS2; 
		PixelShader = PS; 
	}

	pass t3 {
		// Any variable defined in a Pass is stored as an attribute accessible from the Pass.
		// And can be retrieve at runtime.
		MyVariableV4 = float4(1,2,3,4);
		MyVariableV3 = float3(1,2,3);
		MyVariableV2 = float2(1,2);

		MyVariableFloat = 1.5;
		MyVariableArray = {1.5, float4(1,2,3,4)};

		MyVariableIdent = TestIdentifier;
		MyVariableString = "TestString";
	}
}

// Test Legacy Direct3D10/11 FX file
// Toolkit compiler supports basic legacy file declaration (but does not support SetBlendState, SetRasterizerState...etc.)
// Also Geometry shader Streaming output declaration is not supported
technique10
{
	pass t1 
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );		
	}
}