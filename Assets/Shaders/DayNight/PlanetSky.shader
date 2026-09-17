Shader "Custom/PlanetSky"
{
    Properties
    {
        _DayTex ("Day Sky Texture", 2D) = "white" {}
        _NightTex ("Night Sky Texture", 2D) = "black" {}

        _SkyColor ("Sky Color", Color) = (1,1,1,1)
        _Brightness ("Brightness", Range(0,2)) = 1

        _NightBlend ("Night Blend", Range(0,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "Sky"

            Cull Front
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_DayTex);
            SAMPLER(sampler_DayTex);

            TEXTURE2D(_NightTex);
            SAMPLER(sampler_NightTex);

            CBUFFER_START(UnityPerMaterial)

                float4 _DayTex_ST;
                float4 _NightTex_ST;

                float4 _SkyColor;
                float _Brightness;
                float _NightBlend;

            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;

                output.positionHCS =
                    TransformObjectToHClip(input.positionOS.xyz);

                output.uv =
                    TRANSFORM_TEX(input.uv, _DayTex);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 day =
                    SAMPLE_TEXTURE2D(
                        _DayTex,
                        sampler_DayTex,
                        input.uv
                    );

                half4 night =
                    SAMPLE_TEXTURE2D(
                        _NightTex,
                        sampler_NightTex,
                        input.uv
                    );

                half3 sky =
                    lerp(
                        day.rgb,
                        night.rgb,
                        _NightBlend
                    );

                sky *= _SkyColor.rgb;
                sky *= _Brightness;

                return half4(sky, 1.0);
            }

            ENDHLSL
        }
    }
}