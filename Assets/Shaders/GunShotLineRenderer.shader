Shader "Unlit/GunShotLineRenderer"
{
    Properties
    {
        _Texture ("Texture", 2D) = "defaulttexture" {}
        _NumDivisions ("NumDivisions", Range(1,20)) = 4
        _GapThickness ("GapThickness", Range(0.1, 0.6)) = 0.1
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
            
            Texture2D _Texture;
            SamplerState point_repeat_sampler;
            // float4 _Texture_ST;
            float _GapThickness;
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

                float a = !step(1 - _GapThickness, input.uv.x);
                float2 remapedUV = smoothstep(0, 1 - _GapThickness, input.uv);
                fixed4 tex = _Texture.Sample(point_repeat_sampler, remapedUV);
                fixed4 col = fixed4(tex.rgb, a);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
