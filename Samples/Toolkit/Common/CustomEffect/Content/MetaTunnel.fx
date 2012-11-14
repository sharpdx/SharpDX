// ------------------------------------------------------------------------------------
// MetaTunnel - Credits to Anatole Duprat - XT95 / Frequency
// Released at Numerica Artparty 2009
// http://pouet.net/prod.php?which=52777
// Effect compiled with: tkfxc.exe /FoMetaTunnel.fxo MetaTunnel.fx
// ------------------------------------------------------------------------------------

float w;

float obj(float3 pos)
{
	float final=1.0;
	final*=distance(pos,float3(cos(w)+sin(w*0.2),0.3,2.0+cos(w*0.5)*0.5));
	final*=distance(pos,float3(-cos(w*0.7),0.3,2.0+sin(w*0.5)));
	final*=distance(pos,float3(-sin(w*0.2)*0.5,sin(w),2.0));
	final *=cos(pos.y)*cos(pos.x)-0.1-cos(pos.z*7.+w*7.)*cos(pos.x*3)*cos(pos.y*4.)*0.1;
	return final;
}

float4 PSMain(float2 tex : TEXCOORD) : SV_TARGET
{
	static const float s=0.4;
	float2 v= tex.xy * 2.0 - 1.0;
	float3 o=float3(v.x,v.y*1.25,0.0);
	float3 d=float3(v.x+cos(w)*.3,v.y,1.0)/64;
	float3 color= 0.0;

	float2 e=float2(0.01,.0);
	float t=0.0;
	for(int i=0;i<75;i++) 
	{
		float3 p = o + d * t;
		if(obj(p) < s)
		{
			t-=5.0;
			for(int j=0;j<5;j++)
			{
				p = o + d * t;
				if(obj(p)<s)
				{
					break;
				}
				t+=1.0;
			}
			float3 n= normalize(float3(obj(p)-obj(p+e.xyy), obj(p)-obj(p+e.yxy), obj(p)-obj(p+e.yyx)));
			color+=max(dot(float3(0.0,0.0,-0.5),n),0.0)+max(dot(float3(0.0,-0.5,0.5),n),0.0)*0.5;
			break;
		}
		t += 5;
	}

	return float4(color + float3(0.1,0.2,0.5) * (t*0.025), 1.0);
}

technique 
{
	pass 
	{
		Profile = 10.0;
		PixelShader = PSMain;
	}
}