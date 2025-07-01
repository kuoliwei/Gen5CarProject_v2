Shader "Plactice/Plactice2"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Health ("Health", Range(0.0,1.0)) = 1

    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        // src * srcAlpha + dst * (1-srcAlpha)
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
            float _Health;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float InverseLerp(float a, float b, float v)
            {
                return (v-a)/(b-a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //return float4(1,0,0,i.uv.x);
                // sample the texture
                fixed3 texColor = tex2D(_MainTex, float2(_Health, i.uv.y));
                //fixed3 texColor = tex2D(_MainTex, (1-i.uv)*1);
                //float tHealthColor = saturate(InverseLerp(0.35, 0.4, _Health));
                //float tHealthColor = InverseLerp(0.2, 0.3, _Health);
                // float3 healthbarColor = lerp(float3(1,0,0),float3(0,1,0),tHealthColor);
                // float3 backgroundColor = float3(1,1,1);
                //return float4(healthbarColor,0);
                //float healthbarMask = _Health > floor((1 - i.uv.x)*8)/8;
                //float transparency = clamp(_Health > 1 - i.uv.x,0.5,1.0);
                float healthbarMask = _Health > 1 - i.uv.x;
                //clip(healthbarMask-0.1);
                //float3 outputColor = lerp(backgroundColor,texColor,healthbarMask);
                return float4(texColor * healthbarMask,1);
                //return float4(outputColor,transparency);
                //return col;
            }
            ENDCG
        }
    }
}
