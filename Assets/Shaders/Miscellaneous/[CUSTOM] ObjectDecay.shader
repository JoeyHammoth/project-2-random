// Custom Shader #3 
// A shader used for object decay effect. Using smooth step function to reveal the mask texture. 
// Utilizing masking to create a decaying effect on objects. Decay can be accompanied with erosion color
// or can be a disapearing effect.

Shader "Unlit/ObjectDecay"
{
    Properties
    {   
        // Blending props
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation", Float) = 0

        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {} // Texture duplicate for masking

        // Setting up the step function for revealing the mask
        _RevealValue("Reveal", float) = 0

        // Feathering for implmeneting smooth step
        _Feather("Feather", float) = 0

        // Eroding color for mask
        _ErodeColor("Erode Color", Color) = (1, 1, 1, 1)
        
        // Enable/Disable decay effect
        [Toggle] _DecayEffect("Decay Effect", float) = 0


    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
        }
        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_Opp]
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
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MaskTex;
            float4 _MaskTex_ST;

            float _RevealValue, _Feather;

            float4 _ErodeColor;

            bool _DecayEffect;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex); // xy will be first texture
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex); // zw will be second texture
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // texture sampling
                fixed4 col = tex2D(_MainTex, i.uv.xy);
                fixed4 mask = tex2D(_MaskTex, i.uv.zw);

                if (_DecayEffect == 0) {
                    // smoothstep function implementation
                    float revealAmount = smoothstep(mask.r - _Feather, mask.r + _Feather, _RevealValue);

                    // return fixed4(revealAmount.xxx, 1); 
                    return fixed4(col.rgb, col.a * revealAmount);

                } else {
                    // step function implementation
                    float revealAmountTop = step(mask.r, _RevealValue + _Feather);
                    float revealAmountBottom = step(mask.r, _RevealValue - _Feather);

                    // space between two reveals
                    float revealDifference = revealAmountTop - revealAmountBottom;

                    // when black shows main texture, white shows erode color.
                    float3 finalColor = lerp(col.rgb, _ErodeColor.rgb, revealDifference);

                    // return fixed4(revealAmount.xxx, 1); 
                    return fixed4(finalColor.rgb, col.a * revealAmountTop);
                }
            }
            ENDCG
        }

    }
}
