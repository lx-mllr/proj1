Shader "Unlit/GunShotLineRenderer"
{
    Properties
    {
        [HDR]
        _Color ("Color", Color) = (0,0,0,1)
        _NumDivisions ("NumDivisions", Range(1,20)) = 4
        _GapthThickness ("GapThickness", Range(0.1, 0.6)) = 0.1
        _ScrollSpeed ("ScrollSpeed", Range(0, 10)) = 4

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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
            
            fixed4 _Color;
            float _GapthThickness;
            float _ScrollSpeed;
            uint _NumDivisions;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f input) : SV_Target
            {
                float time = _Time.y * _ScrollSpeed;

                input.uv -= time;
                input.uv *= _NumDivisions;
                input.uv = frac(input.uv);

                float y = !step(1 - _GapthThickness, input.uv.x);
                fixed4 col = fixed4(_Color.rgb, y);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
