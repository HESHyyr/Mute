Shader "Unlit/Blob"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseFreq ("Noise Freq", Range(0,1)) = 0.967
        _NoiseAmp ("Noise Amp", Range(0,1)) = 0.127
        _SpikeAmp ("Spike Amp", Range(0,0.05)) = 0.
        [HideInInspector] _SpikePow ("Spike Pow", Range(0,20)) = 4
        [HideInInspector] _SpikeDensity ("Spike Density", Range(0,100)) = 28.1
        _Size ("Size", Range(0,0.5)) = 0.5
        _EnvCubeRotation ("Env Cube Rotation", Vector) = (0,0,0,0)
        _EnvCube ("Environment Cube", CUBE) = "" {}
        _ReflectionIntensity ("Reflection Intensity", Range(0,5)) = 0
        _FresnelPow ("Fresnel Pow", Range(0,10)) = 4
        _FresnelBias ("Fresnel Bias", Range(0,5)) = 0
        _FresnelIntensity ("Fresnel Intensity", Range(0,10)) = 2.8
        _HitAmount ("Hit Amount", Range(0,1)) = 0
        _HitColor ("Hit Color", Color) = (0,0,0,0)
        _GlowSpeed ("Glow Speed", Range(0,10)) = 10
        _VerticalGlow ("Glow", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            #include "simplexnoise.cginc"
            struct appdata
            {
                float4 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 lvert : TEXCOORD3;
                float4 normal : NORMAL;
                float3 viewDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            sampler2D _MainTex;
            samplerCUBE _EnvCube;
            float _NoiseFreq;
            float _NoiseAmp;

            float _SpikeAmp;
            float _SpikePow;
            float _SpikeDensity;

            float _Size;

            float4 _EnvCubeRotation;
            float _ReflectionIntensity;

            float _FresnelBias;
            float _FresnelPow;
            float _FresnelIntensity;

            float _HitAmount;
            float4 _HitColor;
            float _VerticalGlow;
            float _GlowSpeed;

            //Global params
            float _Saturation;
            float3 Grayscale(float3 inputColor)
            {
                float gray = 1.5*pow(dot(inputColor.rgb, float3(0.2126, 0.7152, 0.0722 )),4);
                return float3(gray, gray, gray);
            }
            void GrayscaleAmount(inout float4 inputColor, float amount)
            {
                inputColor.rgb = lerp(inputColor.rgb, Grayscale(inputColor), amount);
            }
            v2f vert (appdata v)
            {
                v2f o;

                float4 distortedVert = v.vertex;
                distortedVert = v.vertex - (0.5 - _Size) * v.normal;
                float4 noiseInput = _NoiseFreq * v.normal;
                noiseInput.x += 0.5 * _Time.y;
                distortedVert += _NoiseAmp * snoise(noiseInput) * v.normal;

                float2 surfaceCoords = float2(atan(v.vertex.x/ v.vertex.y), atan(v.vertex.z/1.));
                surfaceCoords = _SpikeDensity * surfaceCoords;

                distortedVert += _SpikeAmp * (1 + 0.2 * sin(_Time.y + surfaceCoords.y)) * pow(cos(surfaceCoords.y) + sin(surfaceCoords.x), 6) * v.normal;


                // if saturation is 0.5 to 0.7, apply a camera shake
                float transitionAmt = 1. - clamp(abs(_Saturation-0.5)/0.2, 0, 1.);
                distortedVert += transitionAmt * ( float4(1,0,0,0) + sin(v.normal.x) );

                o.lvert = distortedVert;
                o.viewDir = normalize(ObjSpaceViewDir(distortedVert));
                o.vertex = UnityObjectToClipPos(distortedVert);
                o.normal = v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                //REFLECTION
                float3 reflectionDir = reflect(-i.viewDir, i.normal);
                reflectionDir = normalize(rotate_vector(reflectionDir, _EnvCubeRotation));
                float4 envSample = texCUBE(_EnvCube, reflectionDir);
                fixed4 reflectionCol = _ReflectionIntensity * envSample;

                //FRESNEL
                fixed fresnel = _FresnelBias + _FresnelIntensity * pow(saturate(1 - dot(i.viewDir, i.normal)), _FresnelPow);
                fixed4 col = fresnel * reflectionCol;

                //GLOW 
                float objnoise = snoise(i.lvert);
                fixed glowfactor = _VerticalGlow * 10 * pow(sin(i.lvert.x),2)/ (0.9 * i.lvert.y - 0.2);
                col = lerp(col, _HitColor, clamp(glowfactor * (1 + objnoise * 0.5* sin(0.05 * i.vertex.x + _GlowSpeed * _Time.y)),0,1));

                //HIT 
                col = lerp(col, _HitColor, _HitAmount * (1 + 0.5*sin(0.01 * i.vertex.x * i.normal.z + 10 * _Time.y)));

                UNITY_APPLY_FOG(i.fogCoord, col);

                //MUTEMODE
                GrayscaleAmount(col, _Saturation);
                col += 0.2 * _Saturation * noise(0.6 * i.vertex.xy);

                return col;
            }
            ENDCG
        }
    }
}
