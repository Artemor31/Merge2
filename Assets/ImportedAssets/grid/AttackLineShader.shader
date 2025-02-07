Shader "Custom/ColorLerpShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        _LerpTime ("Lerp Time", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _Color1;
            float4 _Color2;
            float _LerpTime;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float t = sin(_Time.y / _LerpTime) * 0.5 + 0.5;
                return lerp(_Color1, _Color2, t);
            }
            ENDHLSL
        }
    }
}