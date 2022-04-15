Shader "Learning/Unlit/FogOfWar"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        albedo("MainTexture",2D) = "white"{}
        noiseMap("DistorsionTexture",2D)="black"{}
        distorsion("Disturbed Factor",Range(0,0.1))=0.0
        speed("Speed",vector)=(0.0,0.0,0.0,0.0)
        mainColor("MainColor",Color)=(1,1,1,1)
        darknessColor("DarknessColor",Color)=(0,0,0,0)
        radius("Fog of War Radius",Range(0.0,7.0)) = 0.0
        darkness("Fog of War Darkness",Range(1.0,0.0)) = 0.0
    }
    
    SubShader
    {
		Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D albedo;
			sampler2D noiseMap;
			float darkness,radius, distorsion;
			float4 speed;
			float3 worldSpace_PlayerPos;
			float4 mainColor,darknessColor;
			
			struct vertexInput
            {
                float4 vertex : POSITION;
			    float2 uv: TEXCOORD0;
            };
			
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv: TEXCOORD0;
                float4 vertexWorldSpace : TEXCOORD1;
            };

            v2f vert (vertexInput v)
            {
                v2f o;
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.vertexWorldSpace = mul(unity_ObjectToWorld,v.vertex);
                o.uv=v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 tex = tex2D(albedo,i.uv);
                
                float dist = distance(worldSpace_PlayerPos,i.vertexWorldSpace);
                if(dist<=radius)
                {
                    tex +=mainColor;
                }
                else
                {
                    float offset = speed.xy* _Time.y;
                    float2 distorsionTexture = tex2D(noiseMap,i.uv + offset).rg;
                    distorsionTexture*=distorsion;
                    tex=tex2D(albedo,distorsionTexture+i.uv);
                }
                return tex; 
            }
            
            ENDHLSL
        }
    }
}
