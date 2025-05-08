Shader "Custom/FadeSkyDome"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FadeStart ("Fade Start Y", Float) = 0
        _FadeEnd ("Fade End Y", Float) = 10
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Lighting Off
        Cull Front

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _FadeStart;
            float _FadeEnd;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float worldY : TEXCOORD1;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldY = worldPos.y;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float alpha = saturate((i.worldY - _FadeStart) / (_FadeEnd - _FadeStart));
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}
