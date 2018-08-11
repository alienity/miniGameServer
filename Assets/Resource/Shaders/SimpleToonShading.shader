Shader "Unlit/SimpleToonShading"
{
	Properties
	{
		_MainTex("主纹理", 2D) = "white" {}
		_Outline("轮廓宽度", Float) = 0.2
		_OutlineColor ("轮廓颜色", Color) = (0, 0, 0, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Name "OUTLINE"
			Cull Front

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _Outline;

			fixed4 _OutlineColor;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;

				float4 pos = mul(UNITY_MATRIX_V, mul(unity_ObjectToWorld, v.vertex));//顶点变换到视角空间 View Space
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);//法线变换到视角空间
				//normal.z = -0.5;
				float4 newNormal = float4(normalize(normal), 0); //归一化以后的normal
				pos = pos + newNormal * _Outline; //沿法线方向扩大_Outline个距离
				o.pos = mul(UNITY_MATRIX_P, pos);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return float4(_OutlineColor.rgb, 1);;
			}

			ENDCG

		}

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
