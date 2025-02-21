// Custom shader #1
// Shader used for glowing effect for objects with pulsing, where the glow is timed. Uses surface shader to allow for lighting.

Shader "Custom/Standard-Emissive"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Albedo ("Albedo (RGB), Alpha(A)", 2D) = "white" {}
        _Normal ("Normal (RGB)", 2D) = "bump" {}
        _MaskMap ("MaskMap (Metallic, Occlusion, Detail Mask, Smoothness)", 2D) = "black" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0

        // Here are the emissive properties for glow
        _Emission("Emission", 2D) = "black" {} // texture
        _EmissionColor("Emission Color", Color) = (1,1,1,1) // color of the glow
        _EmissionIntensity("Emission Intensity", float) = 1.0 // base brightness
        _EmissionGlow("Emission Glow", float) = 1.0 // glow brightness for pulse
        _EmissionGlowDuration("Emission Glow Duration", float) = 5.0 // glow duration, one cycle of the glowing pulse
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "Queue"="Geometry"
        }
        LOD 200

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _FresnelIntensity, _FresnelRamp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fresnelAmount = 1 - max(0, dot(i.normal, i.viewDir));
                fresnelAmount = pow(fresnelAmount, _FresnelRamp) * _FresnelIntensity;
                return fresnelAmount;
            }
            ENDCG
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #include "UnityPBSLighting.cginc" // enables shadow-based lighting

        struct Input
        {
            float2 uv_Albedo;
        };

        // half _Glossiness;
        // half _Metallic;
        sampler2D _Albedo;
        float4 _Color;
        sampler2D _Normal;
        sampler2D _MaskMap;
        sampler2D _Emission;
        float4 _EmissionColor;
        float _EmissionIntensity;
        float _EmissionGlow;
        float _EmissionGlowDuration;

        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 albedo = tex2D (_Albedo, IN.uv_Albedo);
            fixed4 mask = tex2D (_MaskMap, IN.uv_Albedo);
            fixed3 normal = UnpackScaleNormal(tex2D(_Normal, IN.uv_Albedo), 1);
            fixed4 emission = tex2D (_Emission, IN.uv_Albedo);

            o.Albedo = albedo.rgb * _Color;
            o.Alpha = albedo.a;
            o.Normal = normal;
            o.Metallic = mask.r;
            o.Occlusion = mask.g;
            // o.DetailMask = mask.b; Only for close up of objects
            o.Smoothness = mask.a;

            // Metallic and smoothness come from slider variables
            // o.Metallic = _Metallic;
            // o.Smoothness = _Glossiness;
            // Determines colour of the emission texture.
            o.Emission = emission.rgb * _EmissionColor * (_EmissionIntensity + abs(frac(_Time.y * (1 / _EmissionGlowDuration)) - 0.5) * _EmissionGlow);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
