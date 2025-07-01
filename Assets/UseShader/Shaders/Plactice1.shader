Shader "Plactice/Plactice1"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0.0,0.5)) = 0.25
        _CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
        _Fequency("Fequency", Range(0.0,10.0)) = 0.5
        _Amplitude("Amplitude", Range(0.0,10.0)) = 0.5
        _Speed("Speed", Range(0.0,1000.0)) = 0.5
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend srcAlpha OneMinusSrcAlpha

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float _Transparency;
            float _CutoutThresh;

            float _Fequency;
            float _Amplitude;
            float _Speed;

            v2f vert (appdata v)
            {
                float _Pi = 245850922.0 / 78256779.0;
                v2f o;
                v.vertex.y += cos(2 * _Pi * _Fequency * v.vertex.z + (_Time.x * _Speed)) *_Amplitude;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
                //col.a = _Transparency;
                col.a = 1 - i.uv.y - 0.25;
                if(col.a < 0){
                    col.a = 0;
                    }
                //clip(col.r - _CutoutThresh); // = "if(col.r < _CutoutThresh) discard;"
                return col;
            }
            ENDCG
        }
    }
}
