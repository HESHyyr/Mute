Shader "Unlit/Blob"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseFreq ("Noise Freq", Range(0,1)) = 0.7
        _NoiseAmp ("Noise Amp", Range(0,1)) = 0.7
        _SpikeAmp ("Spike Amp", Range(0,0.0002)) = 0.7
        [HideInInspector] _SpikePow ("Spike Pow", Range(0,20)) = 0.7
        [HideInInspector] _SpikeDensity ("Spike Density", Range(0,100)) = 0.7
        _Size ("Size", Range(0,0.5)) = 1
        _EnvCubeRotation ("Env Cube Rotation", Vector) = (0,0,0,0)
        _EnvCube ("Environment Cube", CUBE) = "" {}
        _ReflectionIntensity ("Reflection Intensity", Range(0,5)) = 0
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
                float4 normal : NORMAL;
                float3 viewDir : TEXCOORD1;
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

            v2f vert (appdata v)
            {
                v2f o;

                float4 distortedVert = v.vertex;

                distortedVert = _Size * v.normal;
                float4 noiseInput = _NoiseFreq * v.normal;
                noiseInput.x += 0.5 * _Time.y;
                distortedVert += _NoiseAmp * snoise(noiseInput) * v.normal;

                float2 surfaceCoords = float2(atan(v.vertex.x/ v.vertex.y), atan(v.vertex.z/1.));
                surfaceCoords = _SpikeDensity * surfaceCoords;

                distortedVert += _SpikeAmp * (1 + 0.2 * sin(_Time.y + surfaceCoords.y)) * pow(cos(surfaceCoords.y) + sin(surfaceCoords.x), _SpikePow) * v.normal;

                o.viewDir = normalize(ObjSpaceViewDir(distortedVert));
                o.vertex = UnityObjectToClipPos(distortedVert);
                o.normal = v.normal;
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 reflectionDir = reflect(-i.viewDir, i.normal);
                reflectionDir = normalize(rotate_vector(reflectionDir, _EnvCubeRotation));

                float4 envSample = texCUBE(_EnvCube, reflectionDir);
                fixed4 reflectionCol = _ReflectionIntensity * envSample;

                fixed4 col = reflectionCol;
                return col;
            }
            ENDCG
        }
    }
}
