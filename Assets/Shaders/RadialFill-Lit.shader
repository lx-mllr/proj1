Shader "Custom/RadialFill-Lit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Emissive ("Emissive", 2D) = "white" {}
        
        _AlphaCutoffs ("Alpha Cutoffs (x/y)", Vector) = (0,0,0,0)
        _BorderColor ("Border Color", Color) = (1,1,1,1)
        _BorderThickness ("Border Thickness", float) = 0.05
        _FillPct ("Fill Ratio", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags {  "RenderType"="Transparent" 
                "Queue"="Transparent" 
                "PreviewType"="Plane" 
                "ForceNoShadowCasting"="True" 
                "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:blend

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Emissive;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        
        fixed4 _BorderColor;
        half _BorderThickness;
        half2 _AlphaCutoffs;
        half _FillPct;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half max = length(half2(.5,.5));
            float2 st = IN.uv_MainTex - 0.5;
            half r = 1 - (length(st.xy) / max);
            half angle = atan2(st.y, st.x) / 3.1415927;
            
            half a = step(_AlphaCutoffs.x, r) * !step(1 - _AlphaCutoffs.y, r);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 e = (tex2D (_Emissive, IN.uv_MainTex) + fixed4(1, 0, 0, 0)) / 2;
            e *= ((_SinTime.w / 2)+1);
            e *= step(_FillPct, 0.25);

            // outer border
            half bVal = !step(_AlphaCutoffs.x + _BorderThickness, r);
            c += bVal * (-c + _BorderColor);
            e -= bVal * e;
            // inner
            bVal = step(1 - _AlphaCutoffs.y - _BorderThickness, r);
            c += bVal * (-c + _BorderColor);
            e -= bVal * e;

            _FillPct *= 2;
            _FillPct -= 1;
            a *= !step(_FillPct, angle);

            e *= a;

            o.Albedo = c.rgb;
            o.Alpha = a;
            o.Emission = e.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
