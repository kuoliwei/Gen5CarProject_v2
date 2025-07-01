Shader "Plactice/Plactice0"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pattern ("Pattern", 2D) = "black" {}
        //_ColorA ("Color A", Color) = (1, 1, 1, 1)
        //_ColorB ("Color B", Color) = (1, 1, 1, 1)
        //_ColorStart ("Color Start", Range(0,1)) = 0
        //_ColorEnd ("Color End", Range(0,1)) = 1
        //_Scale ("UV Scale", float) = 1.0
        //_Offset ("UV Offset", float) = 0
        //_WaveAmp ("Wave Amplitude", Range(0, 0.2)) = 2

    }
    SubShader
    {
        Tags { 
            //"RenderType"="Opaque" // tag to inform the render popeline of what type this is
            "RenderType"="Opaque" 
            //"Queue"="Transparent" // changes the render order
            //"Queue"="Geometry"
        }
        LOD 100

        Pass
        {
            //Cull Off // render bothside
            //Cull Front // only render backside
            //Cull Back // only render frontside
            //ZWrite Off
            //ZTest LEqual // less than equal
            //ZTest GEqual // granter than equal
            //ZTest Always // default
            //Blend One One // additive
            //Blend DstColor Zero // multiply

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            struct appdata
            {
                //float4 vertex : POSITION;
                //float3 normals : NORMAL;
                // float4 tangent : TANGENT;
                // float4 color : COLOR;
                //float2 uv : TEXCOORD0;
                // float2 uv1 : TEXCOORD1;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                //float2 uv : TEXCOORD1;
                //float3 normal : TEXCOORD0;
                ////UNITY_FOG_COORDS(1)
                //float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Pattern;
            float4 _MainTex_ST;
            //float4 _ColorA;
            //float4 _ColorB;
            //float _Scale;
            //float _Offset;
            //float _ColorStart;
            //float _ColorEnd;
            //float _WaveAmp;

            //float GetWave(float2 uv){
            //    float2 uvsCentered = uv * 2 - 1;

            //    float radiaDistance = length(uvsCentered);
            //    float wave = cos((radiaDistance - _Time.x * 10) * TAU * 0.8);
            //    //wave *= (1 - radiaDistance);
            //    //wave *= pow(1-radiaDistance, 3);
            //    return wave;
            //    }
            float GetWave(float coord){
                //float2 uvsCentered = uv * 2 - 1;

                //float radiaDistance = length(uvsCentered);
                float wave = cos((coord - _Time.y * 0.1) * TAU * 5) * 0.5 + 0.5;
                wave *= 1 - coord;
                return wave;
                }

            v2f vert (appdata v)
            {
                //v2f o;

                //float wave = cos((v.uv.x + (_Time.x * 3)) * TAU * 3);
                //float wave2 = cos((v.uv.y + (_Time.x * 3)) * TAU * 3);
                //v.vertex.y = wave * wave2 * _WaveAmp;

                //v.vertex.y = GetWave(v.uv);

                //o.vertex = UnityObjectToClipPos(v.vertex);
                //o.normal = UnityObjectToWorldNormal(v.normals);
                //o.uv = (v.uv + _Offset) * _Scale;
                //o.uv = v.uv;
                //o.normal = UnityObjectToWorldNormal(v.normals);
                //o.normal = mul(v.normals,(float3x3)unity_WorldToObject);
                //o.normal = mul((float3x3)unity_ObjectToWorld, v.normals);
                //o.normal = mul((float3x3)UNITY_MATRIX_M, v.normals);
                //o.vertex = v.vertex;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                //return o;
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); // unity_ObjectToWorld = UNITY_MATRIX_M
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float InverseLerp(float a, float b, float v){
                return (v-a)/(b-a);
                }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //return col;
                //return float4(UnityObjectToWorldNormal(i.normal), 1);
                //float4 outColor = lerp(_ColorA, _ColorB, i.uv.x);
                //float4 outColor = _ColorB;
                //return float4(i.uv, 0, 1);
                //return outColor;
                //float t = saturate(InverseLerp(_ColorStart, _ColorEnd, i.uv.x));
                //float t = InverseLerp(_ColorStart, _ColorEnd, i.uv.x);
                //float4 outColor = lerp(_ColorA, _ColorB, t);
                //t = frac(t); // frac = v - floor(v)
                //float t = abs(frac(i.uv.x * 3) * 2 - 1);
                //float time = _Time.y; // _Time.y = second, _Time.z = _Time.y / 10
                //float xOffset = cos(i.uv.x * TAU * 5) * 0.02 ;
                //float t = cos((i.uv.y + xOffset - _Time.x) * TAU * 5)*0.5+0.5;
                //t *= 1 - i.uv.y;
                //return float4(radiaDistance.xxx, 1);
                //float4 outColor = lerp(_ColorA, _ColorB, t);
                // float topBottomRemover = (abs(i.normal.y) < 0.999);
                // float waves = wave * topBottomRemover;
                // float4 gradient = lerp(_ColorA, _ColorB, i.uv.y);
                //return gradient * waves;
                //return GetWave(i.uv);
                float2 HorizontalProjection = float2(i.worldPos.x, i.worldPos.z) / 10;
                HorizontalProjection += 0.5;
                fixed4 col = tex2D(_MainTex, HorizontalProjection);
                float pattern = tex2D(_Pattern, i.uv).x;
                float4 finalColor = lerp(float4(1,0,0,1), col, pattern);
                return finalColor;
                //return pattern;
            }
            ENDCG
        }
    }
}
