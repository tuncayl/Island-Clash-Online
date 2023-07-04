Shader "Lpk/SimpleToon"
{
    Properties
    {
        _BaseMap            ("Texture", 2D)                     = "white" {}
        _BaseColor          ("Color", Color)                    = (1,1,1,1)
        [HDR]_HighColor     ("HighColor", Color)                = (1,1,1,1)
        _Gloss              ("Gloss", Range(0,10))              = 10
        _ShadowStep         ("ShadowStep", Range(0, 1))         = 0.847
        _ShadowStepSmooth   ("ShadowStepSmooth", Range(0, 1))   = 0.021
        _SpecularStep       ("SpecularStep", Range(0, 1))       = 1
        _SpecularStepSmooth ("SpecularStepSmooth", Range(0, 1)) = 1
        _RimStepSmooth      ("RimStepSmooth",Range(0,1))        = 0.1
        _RimStep            ("RimStep", Range(0, 1))            = 0.95
        _RimColor           ("Color", Color)                    = (1,1,1,1)
        
        [Space]   
        _OutlineWidth      ("OutlineWidth", Range(0.0, 1.0))    = 0.1
        _OutlineColor      ("OutlineColor", Color)              = (0.0, 0.0, 0.0, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        Pass
        {
            Name "UniversalForward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            // -------------------------------------
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
             
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _HighColor;
                float _Gloss;
                float _ShadowStep;
                float _ShadowStepSmooth;
                float _SpecularStep;
                float _SpecularStepSmooth;
                float _RimStepSmooth;
                float _RimStep;
                float4 _RimColor;
            CBUFFER_END

            struct Attributes
            {     
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            }; 

            struct Varyings
            {
                float2 uv            : TEXCOORD0;
                float4 normalWS      : TEXCOORD1;    // xyz: normal, w: viewDir.x
                float4 tangentWS     : TEXCOORD2;    // xyz: tangent, w: viewDir.y
                float4 bitangentWS   : TEXCOORD3;    // xyz: bitangent, w: viewDir.z
                float3 viewDirWS     : TEXCOORD4;
				float4 shadowCoord	 : TEXCOORD5;	// shadow receive 在给到 fragment 时，要有阴影坐标

                float4 positionCS    : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                    
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                float3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;

                output.positionCS = vertexInput.positionCS;
                output.uv = input.uv;
                output.normalWS = float4(normalInput.normalWS, viewDirWS.x);
                output.tangentWS = float4(normalInput.tangentWS, viewDirWS.y);
                output.bitangentWS = float4(normalInput.bitangentWS, viewDirWS.z);
                output.viewDirWS = viewDirWS;
				output.shadowCoord = TransformWorldToShadowCoord(vertexInput.positionWS); //shadow receive 将 世界坐标 转到 灯光坐标（阴影坐标）
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float2 uv = input.uv;
                float3 N = normalize(input.normalWS.xyz);
                float3 V = normalize(input.viewDirWS.xyz);
                float3 L = normalize(_MainLightPosition.xyz);
                float3 H = normalize(V+L);
                
                float NV = dot(N,V);
                float NH = dot(N,H);
                float NL = dot(N,L);
                
                NL = NL * 0.5 + 0.5;
                NH = NH * 0.5 + 0.5;
                

                // RAMP
                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
                
                _SpecularStep = _SpecularStep * 1;
                _SpecularStepSmooth = _SpecularStepSmooth * 1;
                NL = smoothstep(_ShadowStep - _ShadowStepSmooth, _ShadowStep + _ShadowStepSmooth, NL);
                NH = smoothstep(_SpecularStep - _SpecularStepSmooth, _SpecularStep + _SpecularStepSmooth, NH);

                //阴影
                float reShadow = MainLightRealtimeShadow(input.shadowCoord);
                
                //边缘光
                float rim = smoothstep(_RimStep - _RimStepSmooth, _RimStep + _RimStepSmooth, 1 - NV);
                 
                //漫反射
                float3 diffuse = _MainLightColor.rgb * baseMap * _BaseColor * NL * reShadow;
                
                //高光
                float3 specular = pow(max(0, NH), _Gloss*_Gloss) * _HighColor * reShadow * NL;
                
                //环境光
                float3 ambient =  rim * _RimColor + SampleSH(N) * _BaseColor * baseMap;
                
                float3 finalColor = diffuse + specular + ambient;
                
                return float4(finalColor , 1.0);
            }
            ENDHLSL
        }
        
        //Outline
        Pass
        {
            Name "Outline"
            Cull Front
            Tags
            {
                "LightMode" = "SRPDefaultUnlit"
            }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            
            float _OutlineWidth;
            float4 _OutlineColor;
            
            v2f vert(appdata v)
            {
                v2f o;
                
                float4 pos = TransformObjectToHClip(float4(v.vertex.xyz + v.normal * _OutlineWidth * 0.1 ,1));
                o.pos = pos;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
