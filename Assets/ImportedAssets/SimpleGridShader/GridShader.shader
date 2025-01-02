Shader "PDT Shaders/TestGrid"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (1,1,1,1)
        _CellColor ("Cell Color", Color) = (0,0,0,0)
        _SelectedColor ("Selected Color", Color) = (1,0,0,1)
        [IntRange] _GridSizeX("Grid Size", Range(1,50)) = 10
        [IntRange] _GridSizeY("Grid Size", Range(1,50)) = 10
        _LineSize("Line Size", Range(0,1)) = 0.15
        [IntRange] _SelectCell("Select Cell Toggle ( 0 = False , 1 = True )", Range(0,1)) = 0.0
        [IntRange] _SelectedCellX("Selected Cell X", Range(0,100)) = 0.0
        [IntRange] _SelectedCellY("Selected Cell Y", Range(0,100)) = 0.0
        [PerRendererData] _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest" "RenderType"="TransparentCutout"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness = 0.0;
        half _Metallic = 0.0;
        float4 _LineColor;
        float4 _CellColor;
        float4 _SelectedColor;

        float _GridSizeX;
        float _GridSizeY;
        float _LineSize;

        float _SelectCell;
        float _SelectedCellX;
        float _SelectedCellY;

        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard output)
        {
            // Albedo comes from a texture tinted by color
            float2 uv = IN.uv_MainTex;

            _SelectedCellX = floor(_SelectedCellX);
            _SelectedCellY = floor(_SelectedCellY);

            fixed4 c = float4(0.0, 0.0, 0.0, 0.0);

            float gsize_x = floor(_GridSizeX);
            float gsize_y = floor(_GridSizeY);

            gsize_x += _LineSize;
            gsize_y += _LineSize;

            float2 id;
            
            id.x = floor(uv.x * gsize_x);
            id.y = floor(uv.y * gsize_y);

            float4 color = _CellColor;
            float brightness = _CellColor.w;
            
            //This checks that the cell is currently selected if the Select Cell slider is set to 1 ( True )
            if (round(_SelectCell) == 1.0 && id.x == _SelectedCellX && id.y == floor(gsize_y - _SelectedCellY - 1))
            {
                brightness = _SelectedColor.w;
                color = _SelectedColor;
            }

            if (frac(uv.x * gsize_x) <= _LineSize || frac(uv.y * gsize_y) <= _LineSize)
            {
                brightness = _LineColor.w;
                color = _LineColor;
            }

            //Clip transparent spots using alpha cutout
            if (brightness == 0.0)
            {
                clip(c.a - 1.0);
            }

            output.Albedo = float4(color.x * brightness, color.y * brightness, color.z * brightness, brightness);
            output.Metallic = 0.0;
            output.Smoothness = 0.0;
            output.Alpha = 0.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}