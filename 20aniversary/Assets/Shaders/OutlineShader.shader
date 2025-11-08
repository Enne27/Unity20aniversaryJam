Shader "OutlineShader"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;

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

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 texelSize = _MainTex_TexelSize.xy;

                float3 sample0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(-texelSize.x, -texelSize.y)).rgb;
                float3 sample1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(0.0, -texelSize.y)).rgb;
                float3 sample2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(texelSize.x, -texelSize.y)).rgb;
                float3 sample3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(-texelSize.x, 0.0)).rgb;
                float3 sample4 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(texelSize.x, 0.0)).rgb;
                float3 sample5 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(-texelSize.x, texelSize.y)).rgb;
                float3 sample6 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(0.0, texelSize.y)).rgb;
                float3 sample7 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(texelSize.x, texelSize.y)).rgb;

                float3 gx = sample2 + 2.0 * sample4 + sample7 - (sample0 + 2.0 * sample3 + sample5);
                float3 gy = sample5 + 2.0 * sample6 + sample7 - (sample0 + 2.0 * sample1 + sample2);
                float3 edge = sqrt(gx * gx + gy * gy);

                float edgeIntensity = dot(edge, float3(0.3, 0.6, 0.1));

                return float4(edgeIntensity.xxx, 1.0);
            }
            ENDHLSL
        }
    }
}
