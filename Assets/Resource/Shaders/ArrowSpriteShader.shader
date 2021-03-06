﻿Shader "Unlit/TextureYAxisTest"
{
    Properties
    {
        _Color("Color Tint",Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff("Alpha Cutoff",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            fixed4 _Color;
            fixed _Cutoff;
            sampler2D _MainTex;
            float4 _MainTex_ST;
             
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
             
            fixed4 frag (v2f i) : SV_Target
            {
 
                fixed4 texColor = tex2D(_MainTex,i.uv);
				clip(1 - i.uv.y - (_Cutoff + 0.01));
				return texColor * _Color;
            }
            ENDCG
        }
    }
}