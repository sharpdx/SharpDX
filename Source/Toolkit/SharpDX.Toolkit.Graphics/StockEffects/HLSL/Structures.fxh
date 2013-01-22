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
// -----------------------------------------------------------------------------
// The following code is a port of XNA StockEffects http://xbox.create.msdn.com/en-US/education/catalog/sample/stock_effects
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//-----------------------------------------------------------------------------
// Structurs.fxh
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


// Vertex shader input structures.

struct VSInput
{
    float4 Position : SV_Position;
};

struct VSInputVc
{
    float4 Position : SV_Position;
    float4 Color    : COLOR;
};

struct VSInputTx
{
    float4 Position : SV_Position;
    float2 TexCoord : TEXCOORD0;
};

struct VSInputTxVc
{
    float4 Position : SV_Position;
    float2 TexCoord : TEXCOORD0;
    float4 Color    : COLOR;
};

struct VSInputNm
{
    float4 Position : SV_Position;
    float3 Normal   : NORMAL;
};

struct VSInputNmVc
{
    float4 Position : SV_Position;
    float3 Normal   : NORMAL;
    float4 Color    : COLOR;
};

struct VSInputNmTx
{
    float4 Position : SV_Position;
    float3 Normal   : NORMAL;
    float2 TexCoord : TEXCOORD0;
};

struct VSInputNmTxVc
{
    float4 Position : SV_Position;
    float3 Normal   : NORMAL;
    float2 TexCoord : TEXCOORD0;
    float4 Color    : COLOR;
};

struct VSInputTx2
{
    float4 Position  : SV_Position;
    float2 TexCoord  : TEXCOORD0;
    float2 TexCoord2 : TEXCOORD1;
};

struct VSInputTx2Vc
{
    float4 Position  : SV_Position;
    float2 TexCoord  : TEXCOORD0;
    float2 TexCoord2 : TEXCOORD1;
    float4 Color     : COLOR;
};

struct VSInputNmTxWeights
{
    float4 Position : SV_Position;
    float3 Normal   : NORMAL;
    float2 TexCoord : TEXCOORD0;
    int4   Indices  : BLENDINDICES0;
    float4 Weights  : BLENDWEIGHT0;
};



// Vertex shader output structures.

struct VSOutput
{
    float4 Diffuse    : COLOR0;
    float4 Specular   : COLOR1;
    float4 PositionPS : SV_Position;
};

struct VSOutputNoFog
{
    float4 Diffuse    : COLOR0;
    float4 PositionPS : SV_Position;
};

struct VSOutputTx
{
    float4 Diffuse    : COLOR0;
    float4 Specular   : COLOR1;
    float2 TexCoord   : TEXCOORD0;
    float4 PositionPS : SV_Position;
};

struct VSOutputTxNoFog
{
    float4 Diffuse    : COLOR0;
    float2 TexCoord   : TEXCOORD0;
    float4 PositionPS : SV_Position;
};

struct VSOutputPixelLighting
{
    float4 PositionWS : TEXCOORD0;
    float3 NormalWS   : TEXCOORD1;
    float4 Diffuse    : COLOR0;
    float4 PositionPS : SV_Position;
};

struct VSOutputPixelLightingTx
{
    float2 TexCoord   : TEXCOORD0;
    float4 PositionWS : TEXCOORD1;
    float3 NormalWS   : TEXCOORD2;
    float4 Diffuse    : COLOR0;
    float4 PositionPS : SV_Position;
};

struct VSOutputTx2
{
    float4 Diffuse    : COLOR0;
    float4 Specular   : COLOR1;
    float2 TexCoord   : TEXCOORD0;
    float2 TexCoord2  : TEXCOORD1;
    float4 PositionPS : SV_Position;
};

struct VSOutputTx2NoFog
{
    float4 Diffuse    : COLOR0;
    float2 TexCoord   : TEXCOORD0;
    float2 TexCoord2  : TEXCOORD1;
    float4 PositionPS : SV_Position;
};

struct VSOutputTxEnvMap
{
    float4 Diffuse    : COLOR0;
    float4 Specular   : COLOR1;
    float2 TexCoord   : TEXCOORD0;
    float3 EnvCoord   : TEXCOORD1;
    float4 PositionPS : SV_Position;
};



// Pixel shader input structures.

struct PSInput
{
    float4 Diffuse  : COLOR0;
    float4 Specular : COLOR1;
};

struct PSInputNoFog
{
    float4 Diffuse : COLOR0;
};

struct PSInputTx
{
    float4 Diffuse  : COLOR0;
    float4 Specular : COLOR1;
    float2 TexCoord : TEXCOORD0;
};

struct PSInputTxNoFog
{
    float4 Diffuse  : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

struct PSInputPixelLighting
{
    float4 PositionWS : TEXCOORD0;
    float3 NormalWS   : TEXCOORD1;
    float4 Diffuse    : COLOR0;
};

struct PSInputPixelLightingTx
{
    float2 TexCoord   : TEXCOORD0;
    float4 PositionWS : TEXCOORD1;
    float3 NormalWS   : TEXCOORD2;
    float4 Diffuse    : COLOR0;
};

struct PSInputTx2
{
    float4 Diffuse   : COLOR0;
    float4 Specular  : COLOR1;
    float2 TexCoord  : TEXCOORD0;
    float2 TexCoord2 : TEXCOORD1;
};

struct PSInputTx2NoFog
{
    float4 Diffuse   : COLOR0;
    float2 TexCoord  : TEXCOORD0;
    float2 TexCoord2 : TEXCOORD1;
};

struct PSInputTxEnvMap
{
    float4 Diffuse  : COLOR0;
    float4 Specular : COLOR1;
    float2 TexCoord : TEXCOORD0;
    float3 EnvCoord : TEXCOORD1;
};
