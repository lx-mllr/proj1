Shader "Unlit/RadialFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaCutoffs ("Alpha Cutoffs (x/y)", Vector) = (0,0,0,0)
        _BorderColor ("Border Color", Color) = (1,1,1,1)
        _BorderThickness ("Border Thickness", float) = 0.05
        _FillPct ("Fill Ratio", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "PreviewType"="Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _BorderColor;
            half _BorderThickness;
            half2 _AlphaCutoffs;
            half _FillPct;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half max = length(float2(.5,.5));
                float2 st = i.uv - 0.5;
                half r = 1 - (length(st.xy) / max);
                half angle = atan2(st.y, st.x) / 3.1415927;

                half a = step(_AlphaCutoffs.x, r) * !step(1 - _AlphaCutoffs.y, r);

                fixed4 col = tex2D(_MainTex, i.uv);
                col.xyz += fixed3(0, angle * 0.5, angle * 0.8) * .5;
                col.a = a;

                fixed3 border = _BorderColor.xyz * !step(_AlphaCutoffs.x + _BorderThickness, r);   
                border += _BorderColor.xyz * step(1 - _AlphaCutoffs.y - _BorderThickness, r);             

                col.xyz = col.xyz + border;

                _FillPct *= 2;
                _FillPct -= 1;
                col *= !step(_FillPct, angle);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
