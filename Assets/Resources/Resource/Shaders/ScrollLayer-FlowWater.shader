// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MADFINGER/Environment/ScrollLayer-FlowWater"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Transparent ("Transparent", Range(0, 1)) = 0.5
		_ScrollX ("Base layer Scroll speed X", Float) = 1.0
		_ScrollY ("Base layer Scroll speed Y", Float) = 0.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
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

			sampler2D _MainTex;
			float _ScrollX;
			float _ScrollY;
			float _Transparent;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex) + frac(float2(_ScrollX, _ScrollY) * _Time);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.a *= _Transparent;
				return col;
			}
			ENDCG
		}
	}
}
