// Custom Shader #2
// Shader for post effects where shader is applied to camer via controller script. Used for applying vignette effect when player is attacked.
// Uses vertex and fragment shaders for creating circle and application of color and feathering effects respectively. 

Shader "Unlit/PostEffects"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    CGINCLUDE
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
    ENDCG
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Passes are the different effects that can be applied on the camera. Pass no. are specified in the controller. 

        Pass // 0
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _ScreenTint;


            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _ScreenTint;
            }
            ENDCG
        }

        Pass // 1
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _Radius, _Feather;
            float4 _TintColor;
            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 newUV = i.uv * 2 - 1; // move everything to center
                float circle = length(newUV); // get distance from center
                float mask = 1 - smoothstep(_Radius, _Radius + _Feather, circle); // invert mask and apply feather
                float invertMask = 1 - mask;

                float3 displayColor = col.rgb * mask;
                float3 vingetteColor = col.rgb * _TintColor * invertMask;
                return fixed4(displayColor + vingetteColor, 1); // return mask to pass 1
            }
            ENDCG
        }
    }
}
