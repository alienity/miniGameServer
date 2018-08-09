Shader "Unlit/Dissolve"
{
	Properties
	{
		_SpringMainTex ("Spring Texture", 2D) = "white" {}
		_WinterMainTex ("Winter Texture", 2D) = "white" {}
		_DissolveRatio("Dissolve Ratio", Range(-1,1)) = 0
		_NoiseScale("Noise Scale", Float) = 5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _SpringMainTex; float4 _SpringMainTex_ST;
			sampler2D _WinterMainTex; float4 _WinterMainTex_ST;
			float _DissolveRatio;
			float _NoiseScale;
			
			//**************************Gradient Noise******************
			float2 unity_gradientNoise_dir(float2 p)
			{
				p = p % 289;
				float x = (34 * p.x + 1) * p.x % 289 + p.y;
				x = (34 * x + 1) * x % 289;
				x = frac(x / 41) * 2 - 1;
				return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
			}

			float unity_gradientNoise(float2 p)
			{
				float2 ip = floor(p);
				float2 fp = frac(p);
				float d00 = dot(unity_gradientNoise_dir(ip), fp);
				float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
				float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
				float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
				fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
				return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
			}
			//**********************************************************

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _SpringMainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 springCol = tex2D(_SpringMainTex, i.uv);
				fixed4 winterCol = tex2D(_WinterMainTex, i.uv);

				float noise = unity_gradientNoise(i.uv * _NoiseScale) + 0.5;

				//return fixed4(noise, noise, noise, 1);

				fixed4 finalColor = lerp(winterCol, springCol, saturate(noise + _DissolveRatio));

				return finalColor;
			}
			ENDCG
		}
	}
}
