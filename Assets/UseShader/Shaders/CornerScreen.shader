Shader "FiveGenCar/CornerScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width ("Width", float) = 0
        _Height ("Height", float) = 0
        _CamPosX ("CamPosX", float) = 0
        _CamPosY ("CamPosY", float) = 0
        _CamPosZ ("CamPosZ", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
                float3 worldPos : TEXCOORD1;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST; // optional
            float _Width;
            float _Height;
            float _CamPosX;
            float _CamPosY;
            float _CamPosZ;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); // unity_ObjectToWorld = UNITY_MATRIX_M
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float ratio = _Width / _Height;
                float2 HorizontalProjection = float2(i.worldPos.x / ratio, i.worldPos.y);
                HorizontalProjection += 0.5;
                float adj = abs(_CamPosZ-i.worldPos.z);
                float2 opp = float2((i.worldPos.x-_CamPosX) / ratio, (i.worldPos.y-_CamPosY));
                float2 angle = float2(atan(opp.x/adj), atan(opp.y/adj));
                float2 offset = float2(abs(i.worldPos.z) * tan(angle.x), abs(i.worldPos.z) * tan(angle.y));
                HorizontalProjection += offset;
                fixed4 col = tex2D(_MainTex, HorizontalProjection);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                if (abs(i.worldPos.x) > 0.4 * ratio - abs(offset.x) || abs(i.worldPos.y) > 0.5 - abs(offset.y))
                {
                return fixed4(0, 0, 0, 0);
                }
                return col;
            }
            ENDCG
        }
    }
}
