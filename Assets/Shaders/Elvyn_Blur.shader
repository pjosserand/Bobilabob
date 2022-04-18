Shader "Learning/Unlit/Elvyn_Blur"
{
    Properties
    {
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        
        _RenderTexture("RenderTexture",2D)="black"{}
        _distorsion("Disturbed Factor",Range(0.1, 3))=0.1

        _radius("Fog of War Radius",Range(0.0,7.0)) = 0.0

    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _RenderTexture;
            float _Intensity, _distorsion, _radius;


            struct vertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 projected : TEXCOORD1;
            };

            v2f vert(vertexInput v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.projected = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 Color = (0,0,0,0);

                float2 centeruv = (0.5, 0.5);

                if (distance(centeruv, i.uv) < _radius/10)
                {
                    Color = tex2D(_RenderTexture, i.uv);
                }
                else
                {
                    float Offsets[11] =
                    {
                        -5,
                        -4,
                        -3,
                        -2,
                        -1,
                        0,
                        1,
                        2,
                        3,
                        4,
                        5,
                    };

                    float Weights[11][11] =
                    {
                        {
                            0.007959, 0.008049, 0.00812, 0.008171, 0.008202, 0.008212, 0.008202, 0.008171, 0.00812,
                            0.008049, 0.007959
                        },
                        {
                            0.008049, 0.00814, 0.008212, 0.008263, 0.008294, 0.008305, 0.008294, 0.008263, 0.008212,
                            0.00814, 0.008049
                        },
                        {
                            0.00812, 0.008212, 0.008284, 0.008336, 0.008367, 0.008378, 0.008367, 0.008336, 0.008284,
                            0.008212, 0.00812
                        },
                        {
                            0.008171, 0.008263, 0.008336, 0.008388, 0.00842, 0.00843, 0.00842, 0.008388, 0.008336,
                            0.008263, 0.008171
                        },
                        {
                            0.008202, 0.008294, 0.008367, 0.00842, 0.008451, 0.008462, 0.008451, 0.00842, 0.008367,
                            0.008294, 0.008202
                        },
                        {
                            0.008212, 0.008305, 0.008378, 0.00843, 0.008462, 0.008473, 0.008462, 0.00843, 0.008378,
                            0.008305, 0.008212
                        },
                        {
                            0.008202, 0.008294, 0.008367, 0.00842, 0.008451, 0.008462, 0.008451, 0.00842, 0.008367,
                            0.008294, 0.008202
                        },
                        {
                            0.008171, 0.008263, 0.008336, 0.008388, 0.00842, 0.00843, 0.00842, 0.008388, 0.008336,
                            0.008263, 0.008171
                        },
                        {
                            0.00812, 0.008212, 0.008284, 0.008336, 0.008367, 0.008378, 0.008367, 0.008336, 0.008284,
                            0.008212, 0.00812
                        },
                        {
                            0.008049, 0.00814, 0.008212, 0.008263, 0.008294, 0.008305, 0.008294, 0.008263, 0.008212,
                            0.00814, 0.008049
                        },
                        {
                            0.007959, 0.008049, 0.00812, 0.008171, 0.008202, 0.008212, 0.008202, 0.008171, 0.00812,
                            0.008049, 0.007959
                        },
                    };


                    float PixelWidth = 1.0f / (1920*_distorsion);
                    float PixelHeight = 1.0f / (1080*_distorsion);

                    float2 Blur;

                    for (int x = 0; x < 11; x++)
                    {
                        Blur.x = i.uv.x + Offsets[x] * PixelWidth;
                        for (int y = 0; y < 11; y++)
                        {
                            Blur.y = i.uv.y + Offsets[y] * PixelHeight;
                            Color += tex2D(_RenderTexture, Blur) * Weights[x][y];
                        }
                    }
                }


                return Color;
            }
            ENDHLSL
        }
    }
}