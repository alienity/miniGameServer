//RealToon V4.0.3
//MJQStudioWorks
//2018

Shader "RealToon/Version 4/Tessellation/No Outline/Default" {
    Properties {

		[Header((RealToon V4.0.3))]
    	[Header((Tessellation x No Outline x Default))]

		[Space(20)][Enum(Off,2,On,0)] _DoubleSided("Double Sided", int) = 2

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Texture Color))]_Texture ("Texture", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (0.7843137,0.7843137,0.7843137,1)

        [Space(8)]_VertexColorsIntensity ("Vertex Colors Intensity", Range(0, 1)) = 0

        [Space(8)][MaterialToggle] _Transparent ("Transparent", Float ) = 0
        [MaterialToggle] _TexturePatternStyle ("Texture Pattern Style", Float ) = 0

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Normal Map))][Normal]_NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalIntensity ("Intensity", Float ) = 1

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Color Adjustment))]_Saturation ("Saturation", Range(0, 2)) = 1

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Self Lit))]_SelfLitIntensity ("Intensity", Range(0, 1)) = 0
        _SelfLitPower ("Power", Float ) = 1
        _SelfLitColor ("Color", Color) = (1,1,1,1)
        [MaterialToggle] _SelfLitHighContrast ("High Contrast", Float ) = 1
        [Space(8)]_MaskSelfLit ("Mask Self Lit", 2D) = "white" {}

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Gloss))]_GlossIntensity ("Intensity", Float ) = 0
        _Glossiness ("Glossiness", Range(0, 1)) = 0.5
        _GlossColor ("Color", Color) = (1,1,1,1)
        [MaterialToggle] _MainTextureColorGloss ("Main Texture Color Gloss", Float ) = 0
        [MaterialToggle] _SoftGloss ("Soft Gloss", Float ) = 0
        [Space(8)]_MaskGloss ("Mask Gloss", 2D) = "white" {}

        [Space(8)][Header(Gloss Texture)]_GlossTextureIntensity ("Intensity", Range(0, 1)) = 0
        _GlossTexture ("Gloss Texture", 2D) = "black" {}
        _GlossTextureRotate ("Rotate", Float ) = 0
        [MaterialToggle] _GlossTextureFollowObjectRotation ("Follow Object Rotation", Float ) = 0
        _GlossTextureFollowLight ("Follow Light", Range(0, 1)) = 0
        [MaterialToggle] _ShadowMaskGlossTexture ("Shadow Mask Gloss Texture", Float ) = 1

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Shadow))]_ShadowOverrideColor ("Override Color", Color) = (1,1,1,1)
        _ShadowAddLight ("Add Light", Float ) = 1
        [Space(8)]_AdjustLightPointSpotlight ("Adjust Light (Point & Spotlight)", Float ) = 1
        _SaturationPointSpotlight ("Saturation (Point & Spotlight)", Range(0, 2)) = 1
        [Space(8)]_AdjustLightDirectionalLight ("Adjust Light (Directional Light)", Float ) = 1
        [MaterialToggle] _LightIntensityAffectOCALCTPTDirectionalLight ("Light Intensity Affect [OC - AL - CT - PT] (Directional Light)", Float ) = 0

        [Space(8)][Header(Self Shadow)]_SelfShadowIntensity ("Intensity", Range(0, 1)) = 1
        _SelfShadowColor ("Color", Color) = (0,0,0,1)
        _SelfShadowSize ("Size", Range(0, 1)) = 0.56
        _SelfShadowHardness ("Hardness", Range(0, 1)) = 1
        [MaterialToggle] _SelfShadowatViewDirection ("Self Shadow at View Direction", Float ) = 0

        [Space(8)][Header(Shadow Color Texture)]_ShadowColorTextureIntensity ("Intensity", Range(0, 1)) = 0
        _ShadowColorTexture ("Shadow Color Texture", 2D) = "black" {}
        _ShadowColorTexturePower ("Power", Float ) = 0

        [Space(8)][Header(ShadowT)]_ShadowTIntensity ("Intensity", Range(0, 1)) = 0
        _ShadowTTexture ("ShadowT Texture", 2D) = "white" {}
        _ShadowTColor ("Color", Color) = (0,0,0,1)
        _ShadowTLightSize ("Light Size", Float ) = 0.5
        _ShadowTShadowSize ("Shadow Size", Float ) = 0
        [MaterialToggle] _ShowShadowTOnLight ("On Light", Float ) = 1
        [MaterialToggle] _ShowShadowTOnSelfShadow ("On Self Shadow", Float ) = 0
        [MaterialToggle] _LightFalloffAffectShadowT ("Light Falloff Affect ShadowT", Float ) = 0

        [Space(8)][Header(Shadow PTexture)]_ShadowPTextureIntensity ("Intensity", Range(0, 1)) = 0
        _ShadowPTexture ("Shadow PTexture", 2D) = "white" {}
        _ShadowPTexturePower ("Power", Float ) = 1

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Lighting))][MaterialToggle] _UseGILighting ("Use GI Lighting", Float ) = 1
        [MaterialToggle] _GIFlatShade ("GI Flat Shade", Float ) = 0
        _GIShadeSize ("GI Shade Size", Range(0, 1)) = 0

        [Space(8)][MaterialToggle] _DirectionalLightAffectShadow ("Directional Light Affect Shadow", Float ) = 0
        [MaterialToggle] _EnableLightFalloff ("Enable Light Falloff", Float ) = 1

        [Space(8)][Header(Custom Light Direction)]_CustomLightDirectionIntensity ("Intensity", Range(0, 1)) = 0
        _CustomLightDirection ("Custom Light Direction", Vector) = (0,0,10,0)
        [MaterialToggle] _CustomLightDirectionFollowObjectRotation ("Follow Object Rotation", Float ) = 1

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Reflection))]_ReflectionIntensity ("Intensity", Range(0, 1)) = 0
        _DefaultReflectionRoughtness ("Default Reflection Roughtness", Range(0, 1)) = 0
        [MaterialToggle] _ReflectionBlendToMainTexture ("Reflection Blend To Main Texture", Float ) = 0
        [Space(8)]_MaskReflection ("Mask Reflection", 2D) = "white" {}

        [Space(8)][MaterialToggle] _UseFReflection ("Use FReflection", Float ) = 0
        _FReflection ("FReflection", 2D) = "white" {}

        [Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Fresnel))]_FresnelIntensity ("Intensity", Range(0, 1)) = 0
        _FresnelColor ("Color", Color) = (1,1,1,1)
        _FresnelFill ("Fill", Float ) = 1
        [MaterialToggle] _HardEdgeFresnel ("Hard Edge Fresnel", Float ) = 0
        [MaterialToggle] _FresnelOnLight ("On Light", Float ) = 1
        [MaterialToggle] _FresnelOnShadowDark ("On Shadow/Dark", Float ) = 0

		[Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((Tessellation))]_TessellationSmoothness ("Smoothness", Range(0, 1)) = 0.5
        [Space(8)]_TessellationTransition ("Transition", Range(0, 1)) = 0.8
        _TessellationNear ("Near", Float ) = 1
        _TessellationFar ("Far", Float ) = 1

		[Space(10)][Header(xxxxxxxxxxxxxxxx)][Header((See Through))]_RefVal ("ID", int ) = 0
        [Enum(None,8,A,0,B,2)] _Oper("Set 1", int) = 8
        [Enum(None,8,A,6,B,7)] _Compa("Set 2", int) = 8

    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull [_DoubleSided]

            Stencil {
            	Ref[_RefVal]
            	Comp [_Compa]
            	Pass [_Oper]
            	Fail [_Oper]
            }
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
			#pragma multi_compile_instancing
            #pragma only_renderers d3d11 glcore gles3 metal d3d11_9x xboxone ps4 switch
            #pragma target 4.6
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _MainColor;
            uniform float _Glossiness;
            uniform float4 _GlossColor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _SelfShadowHardness;
            uniform float _NormalIntensity;
            uniform float _Saturation;
            uniform float _SelfShadowIntensity;
            uniform sampler2D _MaskSelfLit; uniform float4 _MaskSelfLit_ST;
            uniform sampler2D _FReflection; uniform float4 _FReflection_ST;
            uniform sampler2D _MaskReflection; uniform float4 _MaskReflection_ST;
            uniform float _ReflectionIntensity;
            uniform sampler2D _MaskGloss; uniform float4 _MaskGloss_ST;
            uniform fixed _SoftGloss;
            uniform float _SelfLitIntensity;
            uniform fixed _Transparent;
            uniform float _SelfShadowSize;
            uniform float _GlossIntensity;
            uniform float4 _SelfLitColor;
            uniform float _SelfLitPower;
            uniform float _FresnelFill;
            uniform float4 _FresnelColor;
            uniform float _FresnelIntensity;
            uniform fixed _HardEdgeFresnel;
            uniform sampler2D _ShadowPTexture; uniform float4 _ShadowPTexture_ST;
            uniform float _ShadowPTextureIntensity;
            uniform fixed _SelfShadowatViewDirection;
            uniform sampler2D _ShadowTTexture; uniform float4 _ShadowTTexture_ST;
            uniform float _ShadowTIntensity;
            uniform fixed _ShowShadowTOnLight;
            uniform fixed _FresnelOnLight;
            uniform fixed _FresnelOnShadowDark;
            uniform sampler2D _GlossTexture; uniform float4 _GlossTexture_ST;
            uniform float _GlossTextureIntensity;
            uniform fixed _ShadowMaskGlossTexture;
            uniform fixed _MainTextureColorGloss;
            uniform fixed _TexturePatternStyle;
            uniform fixed _ShowShadowTOnSelfShadow;
            uniform float _ShadowTShadowSize;
            uniform float _ShadowTLightSize;
            uniform fixed _EnableLightFalloff;
            uniform float4 _SelfShadowColor;
            float3 GI_Light_Probe( float3 N ){
            return ShadeSH9(float4(N,1));
            }
            
            float3 ReflectionProbe( float3 VR , float Mip ){
            float4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, VR, Mip);
            return DecodeHDR (skyData, unity_SpecCube0_HDR);
            }
            
            uniform fixed _UseFReflection;
            uniform fixed _ReflectionBlendToMainTexture;
            uniform float _DefaultReflectionRoughtness;
            uniform fixed _UseGILighting;
            uniform fixed _GIFlatShade;
            uniform float _GIShadeSize;
            uniform sampler2D _ShadowColorTexture; uniform float4 _ShadowColorTexture_ST;
            uniform float _ShadowAddLight;
            uniform float4 _ShadowOverrideColor;
            uniform fixed _GlossTextureFollowObjectRotation;
            uniform float _GlossTextureRotate;
            uniform float _ShadowColorTexturePower;
            uniform float _ShadowColorTextureIntensity;
            float IFs( float A , float B ){
            if (_WorldSpaceLightPos0.z)
            {
            return A;
            }
            else
            {
            return B;
            }
            }
            
            uniform float _AdjustLightPointSpotlight;
            float IFs2( float A , float B ){
            if (_WorldSpaceLightPos0.w)
            {
            return A;
            }
            else
            {
            return B;
            }
            }
            
            uniform float _AdjustLightDirectionalLight;
            uniform float _SaturationPointSpotlight;
            uniform fixed _LightFalloffAffectShadowT;
            uniform fixed _DirectionalLightAffectShadow;
            uniform fixed _SelfLitHighContrast;
            uniform float _TessellationFar;
            uniform float _TessellationNear;
            uniform float _TessellationTransition;
            uniform float _GlossTextureFollowLight;
            uniform float _VertexColorsIntensity;
            uniform float _ShadowPTexturePower;
            uniform float4 _ShadowTColor;
            uniform float4 _CustomLightDirection;
            uniform fixed _CustomLightDirectionFollowObjectRotation;
            uniform float _CustomLightDirectionIntensity;
            uniform fixed _LightIntensityAffectOCALCTPTDirectionalLight;
			uniform float _TessellationSmoothness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
				UNITY_SETUP_INSTANCE_ID (v);
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float4 vertexColor : COLOR;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.vertexColor = v.vertexColor;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return lerp(_TessellationNear,_TessellationFar,saturate((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)*(1.0 - _TessellationTransition))));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;

					float3 pp[3];
					for (int i = 0; i < 3; ++i)
					pp[i] = v.vertex.xyz - vi[i].normal * (dot(v.vertex.xyz, vi[i].normal) - dot(vi[i].vertex.xyz, vi[i].normal));
					v.vertex.xyz = _TessellationSmoothness * (pp[0] * bary.x + pp[1] * bary.y + pp[2] * bary.z) + (1.0f - _TessellationSmoothness) * v.vertex.xyz;

                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.vertexColor = vi[0].vertexColor*bary.x + vi[1].vertexColor*bary.y + vi[2].vertexColor*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = lerp(float3(0,0,1),_NormalMap_var.rgb,_NormalIntensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float2 _TexturePatternStyle_var = lerp( i.uv0, float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg, _TexturePatternStyle );
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(_TexturePatternStyle_var, _Texture));
                clip(lerp( 1.0, _Texture_var.a, _Transparent ) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);

                float attenuation = LIGHT_ATTENUATION(i);

                float3 node_4607 = (_Texture_var.rgb*_MainColor.rgb);
                float2 node_3407 = reflect(viewDirection,normalDirection).rg;
                float2 node_7185 = (1.0 - (float2((-1*node_3407.r),node_3407.g)*0.5+0.5));
                float4 _FReflection_var = tex2D(_FReflection,TRANSFORM_TEX(node_7185, _FReflection));
                float4 _MaskReflection_var = tex2D(_MaskReflection,TRANSFORM_TEX(i.uv0, _MaskReflection));
                float3 node_8500 = lerp(node_4607,lerp(node_4607,lerp(lerp(_FReflection_var.rgb,lerp(node_4607,_FReflection_var.rgb,_FReflection_var.rgb),_ReflectionBlendToMainTexture),node_4607,(1.0 - _MaskReflection_var.r)),_ReflectionIntensity),_UseFReflection);
                float4 _ShadowColorTexture_var = tex2D(_ShadowColorTexture,TRANSFORM_TEX(i.uv0, _ShadowColorTexture));
                float3 node_487 = lerp(node_8500,(_MainColor.rgb*lerp(_ShadowColorTexture_var.rgb,(_ShadowColorTexture_var.rgb*_ShadowColorTexture_var.rgb),_ShadowColorTexturePower)),_ShadowColorTextureIntensity);
                float node_6497 = 0.0;
                float4 _ShadowPTexture_var = tex2D(_ShadowPTexture,TRANSFORM_TEX(float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg, _ShadowPTexture));
                float3 node_5080 = ((lerp(node_487,lerp(float3(node_6497,node_6497,node_6497),node_487,lerp((1.0 - _ShadowPTexturePower),1.0,_ShadowPTexture_var.r)),_ShadowPTextureIntensity)*_ShadowOverrideColor.rgb)*(0.8*_ShadowAddLight));
                float _EnableLightFalloff_var = lerp( step(0.001,attenuation), attenuation, _EnableLightFalloff );
                float node_3448 = 1.0;
                float node_3148 = 1.0;
                float4 _ShadowTTexture_var = tex2D(_ShadowTTexture,TRANSFORM_TEX(i.uv0, _ShadowTTexture));
                float node_573 = 0.5*dot(lerp( lerp(lightDirection,lerp( _CustomLightDirection.rgb, mul( unity_ObjectToWorld, float4(_CustomLightDirection.rgb,0) ).xyz.rgb, _CustomLightDirectionFollowObjectRotation ),_CustomLightDirectionIntensity), viewDirection, _SelfShadowatViewDirection ),normalDirection)+0.5;
                float node_2723 = saturate(round( 0.5*((_ShadowTTexture_var.r*(1.0 - _ShadowTShadowSize)) + saturate((lerp( node_573, (attenuation*node_573), _LightFalloffAffectShadowT )*_ShadowTLightSize)))));
                float3 node_2724 = lerp(float3(node_3148,node_3148,node_3148),(lerp(float3(node_2723,node_2723,node_2723),(_ShadowTColor.rgb*(1.0 - node_2723)),0.65)*2.86),_ShadowTIntensity);
                float node_9545 = smoothstep( lerp(0.3,0.899,_SelfShadowHardness), 0.9, (node_573*lerp(2.8,0.79,_SelfShadowSize)) );
                float3 node_6482 = lerp(lerp( node_3448, node_2724, _ShowShadowTOnSelfShadow ),lerp( node_3448, node_2724, _ShowShadowTOnLight ),node_9545);
                float node_5274 = 1.0;
                float3 node_1068 = lerp(float3(node_5274,node_5274,node_5274),(lerp(float3(node_9545,node_9545,node_9545),((1.0 - node_9545)*_SelfShadowColor.rgb),0.65)*2.86),_SelfShadowIntensity);
                float4 node_5351_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_5351_p = lerp(float4(float4(_LightColor0.rgb,0.0).zy, node_5351_k.wz), float4(float4(_LightColor0.rgb,0.0).yz, node_5351_k.xy), step(float4(_LightColor0.rgb,0.0).z, float4(_LightColor0.rgb,0.0).y));
                float4 node_5351_q = lerp(float4(node_5351_p.xyw, float4(_LightColor0.rgb,0.0).x), float4(float4(_LightColor0.rgb,0.0).x, node_5351_p.yzx), step(node_5351_p.x, float4(_LightColor0.rgb,0.0).x));
                float node_5351_d = node_5351_q.x - min(node_5351_q.w, node_5351_q.y);
                float node_5351_e = 1.0e-10;
                float3 node_5351 = float3(abs(node_5351_q.z + (node_5351_q.w - node_5351_q.y) / (6.0 * node_5351_d + node_5351_e)), node_5351_d / (node_5351_q.x + node_5351_e), node_5351_q.x);;
                float node_6160 = 0.0;
                float node_4465 = 0.2;
                float node_7903 = 0.3;
                float3 _UseGILighting_var = lerp( UNITY_LIGHTMODEL_AMBIENT.rgb, GI_Light_Probe( lerp(float3(node_6160,node_6160,node_6160),node_487,lerp( (normalDirection*_GIShadeSize), float3(smoothstep( float2(node_4465,node_4465), float2(node_7903,node_7903), (normalDirection.rb*_GIShadeSize) ),0.0), _GIFlatShade )) ), _UseGILighting );
                float node_6003 = pow(1.0-max(0,dot(normalDirection, viewDirection)),exp2((1.0 - _FresnelFill)));
                float3 node_137 = (((lerp( node_6003, smoothstep( 0.38, 0.4, node_6003 ), _HardEdgeFresnel )*1.0)*_FresnelColor.rgb)*lerp(0.0,10.0,_FresnelIntensity));
                float node_8889 = 0.0;
                float node_2697 = (_EnableLightFalloff_var*node_9545);
                float3 node_4224 = (lerp(lerp(lerp(node_5080,node_8500,_LightIntensityAffectOCALCTPTDirectionalLight),lerp(lerp(node_4607,node_5080,(1.0 - _EnableLightFalloff_var)),node_5080,(1.0 - (node_6482*node_1068))),saturate((node_5351.b+node_5351.b+0.05))),lerp(node_5080,node_8500,_LightIntensityAffectOCALCTPTDirectionalLight),IFs( 0.0 , 1.0 ))*(lerp(_UseGILighting_var,(_UseGILighting_var+_LightColor0.rgb),_DirectionalLightAffectShadow)+lerp( 0.0, lerp(node_137,float3(node_8889,node_8889,node_8889),node_2697), _FresnelOnShadowDark )));
                float4 _MaskSelfLit_var = tex2D(_MaskSelfLit,TRANSFORM_TEX(i.uv0, _MaskSelfLit));
                float3 node_4559 = lerp(node_4224,lerp(node_4224,((_SelfLitColor.rgb*node_4607*lerp( 1.0, node_4607, _SelfLitHighContrast ))*_SelfLitPower),_MaskSelfLit_var.rgb),_SelfLitIntensity);
                float3 node_155 = (1.0*ReflectionProbe( viewReflectDirection , lerp(0.0,10.0,_DefaultReflectionRoughtness) ));
                float node_3161 = (1.0 - _Saturation);
                float3 emissive = lerp(lerp(lerp(node_4559,lerp(node_4559,lerp(node_155,lerp(node_4559,node_155,node_155),_ReflectionBlendToMainTexture),_MaskReflection_var.r),_ReflectionIntensity),node_4559,_UseFReflection),dot(lerp(lerp(node_4559,lerp(node_4559,lerp(node_155,lerp(node_4559,node_155,node_155),_ReflectionBlendToMainTexture),_MaskReflection_var.r),_ReflectionIntensity),node_4559,_UseFReflection),float3(0.3,0.59,0.11)),node_3161);
                float node_3788 = IFs2( 0.0 , 1.0 );
                float3 node_7397 = (_EnableLightFalloff_var*node_1068);
                float node_7581 = 0.0;
                float node_9744 = pow(max(0,dot(normalDirection,halfDirection)),exp2(lerp(-2,15,_Glossiness)));
                float node_743_ang = _GlossTextureRotate;
                float node_743_spd = 1.0;
                float node_743_cos = cos(node_743_spd*node_743_ang);
                float node_743_sin = sin(node_743_spd*node_743_ang);
                float2 node_743_piv = float2(0.5,0.5);
                float3 node_6580 = reflect(lerp(viewDirection,halfDirection,_GlossTextureFollowLight),normalDirection);
                float2 node_1696 = lerp( node_6580, mul( unity_WorldToObject, float4(node_6580,0) ).xyz.rgb, _GlossTextureFollowObjectRotation ).rg;
                float2 node_743 = (mul(float2((-1*node_1696.r),node_1696.g)-node_743_piv,float2x2( node_743_cos, -node_743_sin, node_743_sin, node_743_cos))+node_743_piv);
                float2 node_4034 = (1.0 - (node_743*0.5+0.5));
                float4 _GlossTexture_var = tex2D(_GlossTexture,TRANSFORM_TEX(node_4034, _GlossTexture));
                float3 node_3708 = lerp(_GlossTexture_var.rgb,dot(_GlossTexture_var.rgb,float3(0.3,0.59,0.11)),0.0);
                float node_1292 = 0.0;
                float node_3179 = lerp(lerp(0.0,(lerp( smoothstep( 0.79, 0.9, (node_9744*3.0) ), node_9744, _SoftGloss )*1.0),node_2697),lerp( node_3708, lerp(float3(node_1292,node_1292,node_1292),node_3708,node_2697), _ShadowMaskGlossTexture ),_GlossTextureIntensity);
                float node_7359 = 0.0;
                float4 _MaskGloss_var = tex2D(_MaskGloss,TRANSFORM_TEX(i.uv0, _MaskGloss));
                float node_2583 = 0.0;
                float node_8388_if_leA = step(_GlossIntensity,node_2583);
                float node_8388_if_leB = step(node_2583,_GlossIntensity);
                float node_3815 = 0.0;
                float3 node_8001 = (((lerp(lerp(node_8500,dot(node_8500,float3(0.3,0.59,0.11)),(1.0 - _SaturationPointSpotlight)),node_8500,node_3788)*lerp(node_7397,(node_7397*i.vertexColor.rgb),_VertexColorsIntensity))*node_6482)+(lerp(float3(node_7581,node_7581,node_7581),lerp( lerp(float3(node_3179,node_3179,node_3179),(node_3179*_GlossColor.rgb),2.22), lerp(float3(node_7359,node_7359,node_7359),_Texture_var.rgb,node_3179), _MainTextureColorGloss ),_MaskGloss_var.rgb)*lerp((node_8388_if_leA*node_2583)+(node_8388_if_leB*_GlossIntensity),_GlossIntensity,node_8388_if_leA*node_8388_if_leB))+lerp(float3(node_3815,node_3815,node_3815),lerp( node_3815, node_137, _FresnelOnLight ),node_2697));
                float3 node_7256 = (lerp((node_8001*_AdjustLightPointSpotlight),(node_8001*_AdjustLightDirectionalLight*lerp(1.0,0.3,_DirectionalLightAffectShadow)),node_3788)*_LightColor0.rgb);
                float node_201 = 0.0;
                float3 node_3266 = lerp(node_7256,lerp(node_7256,float3(node_201,node_201,node_201),_MaskSelfLit_var.rgb),_SelfLitIntensity);
                float node_2058 = 0.0;
                float3 finalColor = emissive + lerp(lerp(lerp(node_3266,lerp(node_3266,float3(node_2058,node_2058,node_2058),_MaskReflection_var.r),_ReflectionIntensity),node_3266,_UseFReflection),dot(lerp(lerp(node_3266,lerp(node_3266,float3(node_2058,node_2058,node_2058),_MaskReflection_var.r),_ReflectionIntensity),node_3266,_UseFReflection),float3(0.3,0.59,0.11)),node_3161);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull [_DoubleSided]

            Stencil {
            	Ref[_RefVal]
            	Comp [_Compa]
            	Pass [_Oper]
            	Fail [_Oper]
            }

            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
			#pragma multi_compile_instancing
            #pragma only_renderers d3d11 glcore gles3 metal d3d11_9x xboxone ps4 switch
            #pragma target 4.6
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _MainColor;
            uniform float _Glossiness;
            uniform float4 _GlossColor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _SelfShadowHardness;
            uniform float _NormalIntensity;
            uniform float _Saturation;
            uniform float _SelfShadowIntensity;
            uniform sampler2D _MaskSelfLit; uniform float4 _MaskSelfLit_ST;
            uniform sampler2D _FReflection; uniform float4 _FReflection_ST;
            uniform sampler2D _MaskReflection; uniform float4 _MaskReflection_ST;
            uniform float _ReflectionIntensity;
            uniform sampler2D _MaskGloss; uniform float4 _MaskGloss_ST;
            uniform fixed _SoftGloss;
            uniform float _SelfLitIntensity;
            uniform fixed _Transparent;
            uniform float _SelfShadowSize;
            uniform float _GlossIntensity;
            uniform float4 _SelfLitColor;
            uniform float _SelfLitPower;
            uniform float _FresnelFill;
            uniform float4 _FresnelColor;
            uniform float _FresnelIntensity;
            uniform fixed _HardEdgeFresnel;
            uniform sampler2D _ShadowPTexture; uniform float4 _ShadowPTexture_ST;
            uniform float _ShadowPTextureIntensity;
            uniform fixed _SelfShadowatViewDirection;
            uniform sampler2D _ShadowTTexture; uniform float4 _ShadowTTexture_ST;
            uniform float _ShadowTIntensity;
            uniform fixed _ShowShadowTOnLight;
            uniform fixed _FresnelOnLight;
            uniform fixed _FresnelOnShadowDark;
            uniform sampler2D _GlossTexture; uniform float4 _GlossTexture_ST;
            uniform float _GlossTextureIntensity;
            uniform fixed _ShadowMaskGlossTexture;
            uniform fixed _MainTextureColorGloss;
            uniform fixed _TexturePatternStyle;
            uniform fixed _ShowShadowTOnSelfShadow;
            uniform float _ShadowTShadowSize;
            uniform float _ShadowTLightSize;
            uniform fixed _EnableLightFalloff;
            uniform float4 _SelfShadowColor;
            float3 GI_Light_Probe( float3 N ){
            return ShadeSH9(float4(N,1));
            }
            
            float3 ReflectionProbe( float3 VR , float Mip ){
            float4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, VR, Mip);
            return DecodeHDR (skyData, unity_SpecCube0_HDR);
            }
            
            uniform fixed _UseFReflection;
            uniform fixed _ReflectionBlendToMainTexture;
            uniform float _DefaultReflectionRoughtness;
            uniform fixed _UseGILighting;
            uniform fixed _GIFlatShade;
            uniform float _GIShadeSize;
            uniform sampler2D _ShadowColorTexture; uniform float4 _ShadowColorTexture_ST;
            uniform float _ShadowAddLight;
            uniform float4 _ShadowOverrideColor;
            uniform fixed _GlossTextureFollowObjectRotation;
            uniform float _GlossTextureRotate;
            uniform float _ShadowColorTexturePower;
            uniform float _ShadowColorTextureIntensity;
            float IFs( float A , float B ){
            if (_WorldSpaceLightPos0.z)
            {
            return A;
            }
            else
            {
            return B;
            }
            }
            
            uniform float _AdjustLightPointSpotlight;
            float IFs2( float A , float B ){
            if (_WorldSpaceLightPos0.w)
            {
            return A;
            }
            else
            {
            return B;
            }
            }
            
            uniform float _AdjustLightDirectionalLight;
            uniform float _SaturationPointSpotlight;
            uniform fixed _LightFalloffAffectShadowT;
            uniform fixed _DirectionalLightAffectShadow;
            uniform fixed _SelfLitHighContrast;
            uniform float _TessellationFar;
            uniform float _TessellationNear;
            uniform float _TessellationTransition;
            uniform float _GlossTextureFollowLight;
            uniform float _VertexColorsIntensity;
            uniform float _ShadowPTexturePower;
            uniform float4 _ShadowTColor;
            uniform float4 _CustomLightDirection;
            uniform fixed _CustomLightDirectionFollowObjectRotation;
            uniform float _CustomLightDirectionIntensity;
            uniform fixed _LightIntensityAffectOCALCTPTDirectionalLight;
			uniform float _TessellationSmoothness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
				UNITY_SETUP_INSTANCE_ID (v);
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float4 vertexColor : COLOR;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.vertexColor = v.vertexColor;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return lerp(_TessellationNear,_TessellationFar,saturate((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)*(1.0 - _TessellationTransition))));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;

					float3 pp[3];
					for (int i = 0; i < 3; ++i)
					pp[i] = v.vertex.xyz - vi[i].normal * (dot(v.vertex.xyz, vi[i].normal) - dot(vi[i].vertex.xyz, vi[i].normal));
					v.vertex.xyz = _TessellationSmoothness * (pp[0] * bary.x + pp[1] * bary.y + pp[2] * bary.z) + (1.0f - _TessellationSmoothness) * v.vertex.xyz;

                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.vertexColor = vi[0].vertexColor*bary.x + vi[1].vertexColor*bary.y + vi[2].vertexColor*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = lerp(float3(0,0,1),_NormalMap_var.rgb,_NormalIntensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float2 _TexturePatternStyle_var = lerp( i.uv0, float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg, _TexturePatternStyle );
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(_TexturePatternStyle_var, _Texture));
                clip(lerp( 1.0, _Texture_var.a, _Transparent ) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);

                float attenuation = LIGHT_ATTENUATION(i);
                float3 node_4607 = (_Texture_var.rgb*_MainColor.rgb);
                float2 node_3407 = reflect(viewDirection,normalDirection).rg;
                float2 node_7185 = (1.0 - (float2((-1*node_3407.r),node_3407.g)*0.5+0.5));
                float4 _FReflection_var = tex2D(_FReflection,TRANSFORM_TEX(node_7185, _FReflection));
                float4 _MaskReflection_var = tex2D(_MaskReflection,TRANSFORM_TEX(i.uv0, _MaskReflection));
                float3 node_8500 = lerp(node_4607,lerp(node_4607,lerp(lerp(_FReflection_var.rgb,lerp(node_4607,_FReflection_var.rgb,_FReflection_var.rgb),_ReflectionBlendToMainTexture),node_4607,(1.0 - _MaskReflection_var.r)),_ReflectionIntensity),_UseFReflection);
                float node_3788 = IFs2( 0.0 , 1.0 );
                float _EnableLightFalloff_var = lerp( step(0.001,attenuation), attenuation, _EnableLightFalloff );
                float node_5274 = 1.0;
                float node_573 = 0.5*dot(lerp( lerp(lightDirection,lerp( _CustomLightDirection.rgb, mul( unity_ObjectToWorld, float4(_CustomLightDirection.rgb,0) ).xyz.rgb, _CustomLightDirectionFollowObjectRotation ),_CustomLightDirectionIntensity), viewDirection, _SelfShadowatViewDirection ),normalDirection)+0.5;
                float node_9545 = smoothstep( lerp(0.3,0.899,_SelfShadowHardness), 0.9, (node_573*lerp(2.8,0.79,_SelfShadowSize)) );
                float3 node_1068 = lerp(float3(node_5274,node_5274,node_5274),(lerp(float3(node_9545,node_9545,node_9545),((1.0 - node_9545)*_SelfShadowColor.rgb),0.65)*2.86),_SelfShadowIntensity);
                float3 node_7397 = (_EnableLightFalloff_var*node_1068);
                float node_3448 = 1.0;
                float node_3148 = 1.0;
                float4 _ShadowTTexture_var = tex2D(_ShadowTTexture,TRANSFORM_TEX(i.uv0, _ShadowTTexture));
                float node_2723 = saturate(round( 0.5*((_ShadowTTexture_var.r*(1.0 - _ShadowTShadowSize)) + saturate((lerp( node_573, (attenuation*node_573), _LightFalloffAffectShadowT )*_ShadowTLightSize)))));
                float3 node_2724 = lerp(float3(node_3148,node_3148,node_3148),(lerp(float3(node_2723,node_2723,node_2723),(_ShadowTColor.rgb*(1.0 - node_2723)),0.65)*2.86),_ShadowTIntensity);
                float3 node_6482 = lerp(lerp( node_3448, node_2724, _ShowShadowTOnSelfShadow ),lerp( node_3448, node_2724, _ShowShadowTOnLight ),node_9545);
                float node_7581 = 0.0;
                float node_9744 = pow(max(0,dot(normalDirection,halfDirection)),exp2(lerp(-2,15,_Glossiness)));
                float node_2697 = (_EnableLightFalloff_var*node_9545);
                float node_743_ang = _GlossTextureRotate;
                float node_743_spd = 1.0;
                float node_743_cos = cos(node_743_spd*node_743_ang);
                float node_743_sin = sin(node_743_spd*node_743_ang);
                float2 node_743_piv = float2(0.5,0.5);
                float3 node_6580 = reflect(lerp(viewDirection,halfDirection,_GlossTextureFollowLight),normalDirection);
                float2 node_1696 = lerp( node_6580, mul( unity_WorldToObject, float4(node_6580,0) ).xyz.rgb, _GlossTextureFollowObjectRotation ).rg;
                float2 node_743 = (mul(float2((-1*node_1696.r),node_1696.g)-node_743_piv,float2x2( node_743_cos, -node_743_sin, node_743_sin, node_743_cos))+node_743_piv);
                float2 node_4034 = (1.0 - (node_743*0.5+0.5));
                float4 _GlossTexture_var = tex2D(_GlossTexture,TRANSFORM_TEX(node_4034, _GlossTexture));
                float3 node_3708 = lerp(_GlossTexture_var.rgb,dot(_GlossTexture_var.rgb,float3(0.3,0.59,0.11)),0.0);
                float node_1292 = 0.0;
                float node_3179 = lerp(lerp(0.0,(lerp( smoothstep( 0.79, 0.9, (node_9744*3.0) ), node_9744, _SoftGloss )*1.0),node_2697),lerp( node_3708, lerp(float3(node_1292,node_1292,node_1292),node_3708,node_2697), _ShadowMaskGlossTexture ),_GlossTextureIntensity);
                float node_7359 = 0.0;
                float4 _MaskGloss_var = tex2D(_MaskGloss,TRANSFORM_TEX(i.uv0, _MaskGloss));
                float node_2583 = 0.0;
                float node_8388_if_leA = step(_GlossIntensity,node_2583);
                float node_8388_if_leB = step(node_2583,_GlossIntensity);
                float node_3815 = 0.0;
                float node_6003 = pow(1.0-max(0,dot(normalDirection, viewDirection)),exp2((1.0 - _FresnelFill)));
                float3 node_137 = (((lerp( node_6003, smoothstep( 0.38, 0.4, node_6003 ), _HardEdgeFresnel )*1.0)*_FresnelColor.rgb)*lerp(0.0,10.0,_FresnelIntensity));
                float3 node_8001 = (((lerp(lerp(node_8500,dot(node_8500,float3(0.3,0.59,0.11)),(1.0 - _SaturationPointSpotlight)),node_8500,node_3788)*lerp(node_7397,(node_7397*i.vertexColor.rgb),_VertexColorsIntensity))*node_6482)+(lerp(float3(node_7581,node_7581,node_7581),lerp( lerp(float3(node_3179,node_3179,node_3179),(node_3179*_GlossColor.rgb),2.22), lerp(float3(node_7359,node_7359,node_7359),_Texture_var.rgb,node_3179), _MainTextureColorGloss ),_MaskGloss_var.rgb)*lerp((node_8388_if_leA*node_2583)+(node_8388_if_leB*_GlossIntensity),_GlossIntensity,node_8388_if_leA*node_8388_if_leB))+lerp(float3(node_3815,node_3815,node_3815),lerp( node_3815, node_137, _FresnelOnLight ),node_2697));
                float3 node_7256 = (lerp((node_8001*_AdjustLightPointSpotlight),(node_8001*_AdjustLightDirectionalLight*lerp(1.0,0.3,_DirectionalLightAffectShadow)),node_3788)*_LightColor0.rgb);
                float node_201 = 0.0;
                float4 _MaskSelfLit_var = tex2D(_MaskSelfLit,TRANSFORM_TEX(i.uv0, _MaskSelfLit));
                float3 node_3266 = lerp(node_7256,lerp(node_7256,float3(node_201,node_201,node_201),_MaskSelfLit_var.rgb),_SelfLitIntensity);
                float node_2058 = 0.0;
                float node_3161 = (1.0 - _Saturation);
                float3 finalColor = lerp(lerp(lerp(node_3266,lerp(node_3266,float3(node_2058,node_2058,node_2058),_MaskReflection_var.r),_ReflectionIntensity),node_3266,_UseFReflection),dot(lerp(lerp(node_3266,lerp(node_3266,float3(node_2058,node_2058,node_2058),_MaskReflection_var.r),_ReflectionIntensity),node_3266,_UseFReflection),float3(0.3,0.59,0.11)),node_3161);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull [_DoubleSided]
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
			#pragma multi_compile_instancing
            #pragma only_renderers d3d11 glcore gles3 metal d3d11_9x xboxone ps4 switch
            #pragma target 4.6
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform fixed _Transparent;
            uniform fixed _TexturePatternStyle;
            uniform float _TessellationFar;
            uniform float _TessellationNear;
            uniform float _TessellationTransition;
			uniform float _TessellationSmoothness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
				UNITY_SETUP_INSTANCE_ID (v);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return lerp(_TessellationNear,_TessellationFar,saturate((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)*(1.0 - _TessellationTransition))));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
					
					float3 pp[3];
					for (int i = 0; i < 3; ++i)
					pp[i] = v.vertex.xyz - vi[i].normal * (dot(v.vertex.xyz, vi[i].normal) - dot(vi[i].vertex.xyz, vi[i].normal));
					v.vertex.xyz = _TessellationSmoothness * (pp[0] * bary.x + pp[1] * bary.y + pp[2] * bary.z) + (1.0f - _TessellationSmoothness) * v.vertex.xyz;

                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float2 _TexturePatternStyle_var = lerp( i.uv0, float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg, _TexturePatternStyle );
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(_TexturePatternStyle_var, _Texture));
                clip(lerp( 1.0, _Texture_var.a, _Transparent ) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
