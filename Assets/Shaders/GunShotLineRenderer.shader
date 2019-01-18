Shader "Unlit/GunShotLineRenderer"
{
    Properties
    {
        [HDR]
        _Color ("Color", Color) = (0,0,0,1)
        _Pct ("Percent", Range (0, 1)) = 0.5

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
            float _Pct;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // remap 0-1
                float ratio = (_SinTime.z + 1) / 2;
                float remap = ratio * .5 + .2;

                float y = step(remap, i.uv.x);
                fixed3 mul = fixed3(y,y,y) * _Color.rgb;
                fixed4 col = fixed4(mul, y);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
