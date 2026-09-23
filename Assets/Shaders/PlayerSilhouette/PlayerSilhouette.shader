Shader "Custom/PlayerSilhouette"
{
    Properties
    {
        _BaseColor ("Silhouette Color", Color) = (0.02, 0.01, 0.04, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent+100"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "Silhouette"

            ZWrite Off
            ZTest Always

            Cull Back

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)

                float4 _BaseColor;

            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs positionInputs =
                    GetVertexPositionInputs(input.positionOS.xyz);

                output.positionHCS =
                    positionInputs.positionCS;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return _BaseColor;
            }

            ENDHLSL
        }
    }
}