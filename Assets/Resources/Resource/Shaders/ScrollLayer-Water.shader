// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// - Scroll 2 layers /w Multiplicative op

Shader "MADFINGER/Environment/Scroll Layer - Water" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_ScrollX ("Base layer Scroll speed X", Float) = 1.0
	_ScrollY ("Base layer Scroll speed Y", Float) = 0.0
	_AMultiplier ("Layer Multiplier", Float) = 0.5
}

SubShader {
	Tags { "Queue"="Geometry+10" "RenderType"="Opaque" }
	
	Lighting Off Fog { Mode Off }
//	ZWrite Off
	
	LOD 100
	
		
	CGINCLUDE
	#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;

	float4 _MainTex_ST;

	float _ScrollX;
	float _ScrollY;

	float _AMultiplier;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : TEXCOORD2;		
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord.xy,_MainTex) + frac(float2(_ScrollX, _ScrollY) * _Time);
		o.color = fixed4(_AMultiplier, _AMultiplier, _AMultiplier, _AMultiplier);

		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 o;
			fixed4 tex = tex2D (_MainTex, i.uv);
			
			o = tex * i.color;
			
			return o;
		}
		ENDCG 
	}	
}
}
