Shader "FiveGenCar/CornerScreenTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TexWidth ("TexWidth", Float) = 0
        _TexHeight ("TexHeight", Float) = 0

        _XAxisAngle ("XAxisAngle", Float) = 0
        _YAxisAngle ("YAxisAngle", Float) = 0
        _ZAxisAngle ("ZAxisAngle", Float) = 0
        _XOffset ("XOffset", Range(0,1.0)) = 0
        _YOffset ("YOffset", Range(0,1.0)) = 0
        _DiagonalOffset ("DiagonalOffset", Range(0,1.0)) = 0

        _Debug ("Debug", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TexWidth;
            float _TexHeight;

            float _XAxisAngle;
            float _YAxisAngle;
            float _ZAxisAngle;
            float _XOffset;
            float _YOffset;
            float _DiagonalOffset;

            float _Debug;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld,v.vertex); // unity_ObjectToWorld = UNITY_MATRIX_M
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.worldPos = mul(unity_ObjectToWorld,v.uv); 
                o.uv = v.uv;
                return o;
            }
            float2 convertStraightOffet (float2 offset, float2 uv)
            {
                //float xOffset = uv.x * cos(radians(_ZAxisAngle)) * offset.x - uv.y * cos(radians(90.0 - _ZAxisAngle)) * offset.y; // move x & y axis edge
                //float yOffset = uv.x * sin(radians(_ZAxisAngle)) * offset.x + uv.y * sin(radians(90.0 - _ZAxisAngle)) * offset.y;
                float xOffset = uv.x * cos(radians(_ZAxisAngle)) * offset.x * (1-uv.y) - uv.y * cos(radians(90.0 - _ZAxisAngle)) * offset.y * (1-uv.x); // x & y axis edge not move
                float yOffset = uv.x * sin(radians(_ZAxisAngle)) * offset.x * (1-uv.y) + uv.y * sin(radians(90.0 - _ZAxisAngle)) * offset.y * (1-uv.x);
                return float2(xOffset, yOffset);
            }
            float2 extrusionToZeroPoint (float offset, float2 uv) // extrusion to zero point
            {
                float xOffset = cos(radians(_ZAxisAngle)) * offset - cos(radians(90.0 - _ZAxisAngle)) * offset;
                float yOffset = sin(radians(_ZAxisAngle)) * offset + sin(radians(90.0 - _ZAxisAngle)) * offset;

                float2 finaloffset = float2(uv.y * xOffset * uv.x, uv.x * yOffset * uv.y); // x & y axis edge not move
                // float shiftValue = (uv.x + uv.y) / 2; // move x & y axis edge
                // float2 finaloffset = float2(xOffset, yOffset) * shiftValue; // move x & y axis edge
                return finaloffset;
            }
            float2 extrusionToOneZeroPoint (float offset, float2 uv) // extrusion to zero point
            {
                float xOffset = cos(radians(_ZAxisAngle)) * offset - cos(radians(90.0 - _ZAxisAngle)) * offset;
                float yOffset = sin(radians(_ZAxisAngle)) * offset + sin(radians(90.0 - _ZAxisAngle)) * offset;

                //float2 finaloffset = float2(uv.y * xOffset * uv.x, uv.x * yOffset * uv.y); // x & y axis edge not move
                float shiftValue = (uv.x + uv.y) / 2; // move x & y axis edge
                float2 finaloffset = float2(xOffset, yOffset) * shiftValue; // move x & y axis edge
                return finaloffset;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                float ratio = _TexWidth / _TexHeight;
                float2 horizontalProjection = float2(i.worldPos.x / ratio,i.worldPos.y);
                horizontalProjection += float2(0.5, 0.5);
                horizontalProjection += convertStraightOffet(float2(_XOffset, _YOffset), i.uv);
                horizontalProjection += extrusionToZeroPoint(_DiagonalOffset, i.uv);
                fixed4 col = tex2D(_MainTex, horizontalProjection);
                return col;
            }
            ENDCG
        }
    }
}
