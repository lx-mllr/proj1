Shader "Unlit/SmoothMask"
{
    Properties
    {
        [HDR]
        _Color ("Color", Color ) = (1,1,1,1)
        _AlphaRangeXY ("Alpha Range X(xy)Y(zw)", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float4 _AlphaRangeXY;

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
                fixed4 col = _Color;
                float alpha = smoothstep(i.uv.x, _AlphaRangeXY.y, 1);
                alpha *= smoothstep(i.uv.y, _AlphaRangeXY.w, 1);
                alpha *= smoothstep(i.uv.x, 0, _AlphaRangeXY.x);
                alpha *= smoothstep(i.uv.y, 0, _AlphaRangeXY.z);
                col.a  = alpha * i.uv.z;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
