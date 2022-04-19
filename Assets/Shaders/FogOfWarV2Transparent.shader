Shader "Learning/Unlit/FogOfWarV2Transparent"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Main Texture",2D) = "white"{}
        _Distortion("Distortion Texture", 2D) = "white"{}
        _Radius("Fog of War Radius",Range(0.0,7.0)) = 0.0
        _DiscolorationDarkness("Fog of War Darkness",Range(1.0,0.0)) = 0.0
        _Threshold("Alpha Threshold", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull front 
        LOD 100
        
        Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D _Albedo, _Distortion;
			float3 worldSpace_PlayerPos;
			float _Radius, _DiscolorationDarkness,  _Threshold;
			
			struct vertexInput
            {
                float4 vertex : POSITION;
			    float2 uv: TEXCOORD0;
			    float3 normal : NORMAL;
            };
			
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv: TEXCOORD0;
                float4 vertexWorldSpace : TEXCOORD1;
                float4 Color : COLOR;
            };

            v2f vert (vertexInput v)
            {
                v2f o;
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.vertexWorldSpace = mul(unity_ObjectToWorld,v.vertex);
                o.
                o.uv=v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 Color = (0,0,0,0);
                float4 Texture = tex2D(_Albedo, i.uv);
                
                float dist = distance(worldSpace_PlayerPos,i.vertexWorldSpace);
                if(dist<=_Radius)
                {
                    Color = Texture;
                }
                else
                {
                                       
                    if (tex2D(_Distortion, i.uv).r >= _Threshold)
                    {
                        float4 Tex = tex2D(_Distortion, i.uv * 5);
                        
                        Color = (Tex.r+Tex.g+Tex.b) * _DiscolorationDarkness /3;
                    }
                    else
                    {
                        Color = (Texture.r+Texture.g+Texture.b) * _DiscolorationDarkness /3;
                    }
                    
                    
                }
                return Color; 
            }
            
            ENDHLSL
        }
    }
}
