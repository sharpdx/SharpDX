// @@@ Begin Header
float4x4 worldViewProj;
// @@@ End Header

// This is the default code in the source section.
// @@@ Begin Source
export void VertexFunction(inout float4 position, inout float4 color)
{
	position = mul(position, worldViewProj);
	color = color;
}

export float4 ColorFunction(float4 position, float4 color)
{
	return color;
}
// @@@ End Source

// This code is not displayed, but is used as part of the linking process.
// @@@ Begin Hidden
export float4 InvertColor(float4 color)
{
	return float4(float3(1,1,1)-color.xyz, 1);
}

export float4 Grayscale(float4 color)
{
	float l = (color.x + color.y + color.z)/3;
	return float4(l, l, l, color.a);
}
// @@@ End Hidden
