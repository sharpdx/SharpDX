//
// Particle effect using geometry shader and stream out
// 2013 Christoph Romstoeck (lwm)
//

Texture2D<float4> _texture;
sampler _sampler : register(s0);

#define FLAG_CONSTRAINED 1
#define FLAG_FAST_FADE 2

cbuffer EveryFrame : register(b0) {
	float4x4	_view;
	float4x4	_proj;
	float4x4	_lookAtMatrix;
	float3		_camDir;
	float		_elapsedSeconds;
	float3		_gravity;
};

struct ParticleVertex
{
    float3 Position : POSITION;
	float3 Velocity : NORMAL;
	float4 Color : COLOR;
	float2 TimerLifetime : TEXCOORD0;
	uint Flags : TEXCOORD1;
	float2 SizeStartEnd : TEXCOORD2;
};

struct ParticleVertexGsUpdateOut
{
	float4 Position : SV_POSITION;
	float3 Velocity : NORMAL;
	float4 Color : COLOR;
	float2 TimerLifetime : TEXCOORD0;
	uint Flags : TEXCOORD1;
	float2 SizeStartEnd : TEXCOORD2;
};

struct ParticleVertexGsOut
{
    float4 Position : SV_POSITION;
	float4 Color : COLOR;
	float2 TexCoord : TEXCOORD0;
	float4 PositionVS : TEXCOORD1;
};

// ===

// Vertex shader has no work to do.
// Simply pass vertex on to the next stage.
ParticleVertex VS_Passthrough(ParticleVertex v)
{
	return v;
}

// Geometry shader to update one particle.
[maxvertexcount(1)]
void GS_Update(point ParticleVertex vertex[1], inout PointStream<ParticleVertexGsUpdateOut> stream) {

	ParticleVertex input = vertex[0];	

	// Calculate new age of the particle.
	float newTimer = input.TimerLifetime.x + _elapsedSeconds;

	// If the particle is older than its lifetime, don't do anything.
	if(newTimer > input.TimerLifetime.y)
		return;

	// Calculate new position by adding the particle's velocity.
	float3 newPosition = input.Position + input.Velocity * _elapsedSeconds;

	// Calculate new velocity by adding the world's gravity.
	float3 newVelocity = input.Velocity + _gravity * _elapsedSeconds;

	ParticleVertexGsUpdateOut output;
	output.Position = float4(newPosition, 1);
	output.Velocity = newVelocity;
	output.Color = input.Color;
	output.TimerLifetime.x = newTimer;
	output.TimerLifetime.y = input.TimerLifetime.y;
	output.Flags = input.Flags;
	output.SizeStartEnd = input.SizeStartEnd;

	// Append updated particle to output stream.
	stream.Append(output);
}

technique UpdateTeq
{
    pass Pass1
    {
		Profile = 10.0;
        VertexShader = VS_Passthrough;
		GeometryShader = GS_Update;
		StreamOutput = "SV_POSITION.xyz; NORMAL.xyz; COLOR.xyzw; TEXCOORD0.xy; TEXCOORD1.x; TEXCOORD2.xy";
		StreamOutputRasterizedStream = 0;
        PixelShader = null;
    }
}

// ===============================================

// Geometry shader to expand the vertex into a quad.
[maxvertexcount(4)]
void GS_Render(point ParticleVertex inputArray[1], inout TriangleStream<ParticleVertexGsOut> stream) {

	ParticleVertex input = inputArray[0];	

	// Calculate the particles age in [0..1]
	float age = input.TimerLifetime.x / input.TimerLifetime.y;

    ParticleVertexGsOut v;
	
	// Determine the particle's color based on its age.
	v.Color = input.Color;
	bool fastFade = (input.Flags & FLAG_FAST_FADE) > 0;
	if (fastFade)
        v.Color.a *= (-(256 * 256) * pow(age - 0.5f, 16) + 1);
    else
        v.Color.a *= (-4 * (age - 0.5f) * (age - 0.5f) + 1);
	
	// Calculate the particle's current size
	float2 size = lerp(input.SizeStartEnd.x, input.SizeStartEnd.y, age);	

	// Check if one of the quad's axes should be constrained to the particle's velocity.
	bool constrained = (input.Flags & FLAG_CONSTRAINED) > 0;
	float3 right, up;
	if(constrained)
	{
		right = normalize(input.Velocity);
		up = cross(_camDir, right) * size.y;
		right *= size.x;
	}
	else
	{
		float2 xr = float4(size.x, 0, 0, 1);
		float2 yr = float4(0, size.y, 0, 1);
		right = mul(xr, _lookAtMatrix).xyz;
		up = mul(yr, _lookAtMatrix).xyz;
	}

	// Create and append four vertices to form a quad.
    float4 positionWS = float4(input.Position + right + up, 1.f);
	v.PositionVS = mul(positionWS, _view);
    v.Position = mul(v.PositionVS, _proj);
	v.TexCoord = float2(1, 1);
    stream.Append(v);

    positionWS = float4(input.Position - right + up, 1.f);
    v.PositionVS = mul(positionWS, _view);
    v.Position = mul(v.PositionVS, _proj);
	v.TexCoord = float2(0, 1);
    stream.Append(v);

    positionWS = float4(input.Position + right - up, 1.f);
    v.PositionVS = mul(positionWS, _view);
    v.Position = mul(v.PositionVS, _proj);
	v.TexCoord = float2(1, 0);
    stream.Append(v);

    positionWS = float4(input.Position - right - up, 1.f);
    v.PositionVS = mul(positionWS, _view);
    v.Position = mul(v.PositionVS, _proj);
	v.TexCoord = float2(0, 0);
    stream.Append(v);

    stream.RestartStrip();
}

// Simple pixel shader to render the particles.
float4 PS_Render(ParticleVertexGsOut input) : SV_Target
{
	float4 tex = _texture.Sample(_sampler, input.TexCoord);
	return tex * input.Color;
}

technique RenderTeq
{
    pass Pass1
    {
		Profile = 10.0;
        VertexShader = VS_Passthrough;
		GeometryShader = GS_Render;
        PixelShader = PS_Render;
    }
}

