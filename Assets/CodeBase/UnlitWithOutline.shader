Shader "Custom/UnlitWithOutline"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float4 _OutlineColor;
            float _OutlineThickness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Основной проход
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float4 positionWS = TransformObjectToWorld(IN.positionOS);
                OUT.positionHCS = TransformWorldToHClip(positionWS);
                OUT.normalWS = normalWS;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _BaseColor;
            }
            ENDHLSL
        }

        Pass
        {
            Name "Outline"
            Tags { "LightMode"="UniversalForward" }

            Cull Front
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _OutlineColor;
            float _OutlineThickness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Смещение вершин для создания контура
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float4 positionWS = TransformObjectToWorld(IN.positionOS);
                positionWS.xyz += normalWS * _OutlineThickness;
                OUT.positionHCS = TransformWorldToHClip(positionWS);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}