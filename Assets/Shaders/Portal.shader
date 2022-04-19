Shader "Learning/Unlit/Portal"
{
    Properties
    {
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        //_Albedo("Main Texture",2D) = "white"{}
        //_Normal("Normal",2D) = "white"{}

        _TP("TwirlPower", Float) = 1

        _Distortion("Distortion Texture", 2D) = "white"{}
        _Radius("Fog of War Radius",Range(0.0,7.0)) = 0.0
        _GSPower("Gayscale power",Range(0,1)) = 0.0
        _Threshold("Alpha Threshold", Float) = 0.0
    }

    SubShader
    {
        Pass
        {

            name "FogOfWarV2"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D _Albedo, _Distortion; //_Normal;
            float3 worldSpace_PlayerPos;
            float4 _Albedo_ST;
            float _Radius, _GSPower, _Threshold, _TP;

            struct vertexInput
            {
                float4 vertex : POSITION;
                float2 uv: TEXCOORD0;
                //float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv: TEXCOORD0;
                float4 vertexWorldSpace : TEXCOORD1;
                //float3 normal : TEXTCOORD1;
                float4 color : COLOR;
            };

            v2f vert(vertexInput v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.vertexWorldSpace = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                //o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            void Unity_Twirl_float(float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out)
            {
                float2 delta = UV - Center;
                float angle = Strength * length(delta);
                float x = cos(angle) * delta.x - sin(angle) * delta.y;
                float y = sin(angle) * delta.x + cos(angle) * delta.y;
                Out = float2(x + Center.x + Offset.x, y + Center.y + Offset.y);
            }

            inline float unity_noise_randomValue(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
            }

            inline float unity_noise_interpolate(float a, float b, float t)
            {
                return (1.0 - t)*a + (t*b);
            }

            inline float unity_valueNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3 - 0));
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3 - 1));
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3 - 2));
                t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

                Out = t;
            }

            void Unity_Remap_float4(float4 In, float2 InMinMax, float2 OutMinMax, out float4 Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            float4 frag(v2f i) : SV_Target
            {
                //half ReNormFactor = 1.0 / length(i.normal);
                float4 Color = float4(0, 0, 0, 0);

                float2 newUV;
                Unity_Twirl_float(i.uv, (0.5, 0.5), _TP, (0, 0), newUV);
                


                float4 Texture = tex2D(_Albedo, float2((i.uv.x + _Albedo_ST.z) * _Albedo_ST.x,
                                                       (i.uv.y + _Albedo_ST.w) * _Albedo_ST.y));


                float dist = distance(worldSpace_PlayerPos, i.vertexWorldSpace);
                float4 temp = lerp(Texture, (Texture.r + Texture.g + Texture.b) / 3,
                                   clamp(dist * dist * _GSPower, 0, 1));
                Color = lerp(temp, float4(0,0,0,0), clamp((dist-_GSPower)/(_Radius-_GSPower), 0, 1));

                if(Color.w = 0)
                {
                    discard;
                }

                return Color;
            }
            ENDHLSL
        }
    }
}