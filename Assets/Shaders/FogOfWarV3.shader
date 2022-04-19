Shader "Learning/Unlit/FogOfWarV3"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Main Texture",2D) = "white"{}
        //_Normal("Normal",2D) = "white"{}
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
			float3 _PlayerPos;
			float4 _Albedo_ST;
			float _Radius, _GSPower,  _Threshold;
			
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

            v2f vert (vertexInput v)
            {
                v2f o;
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.vertexWorldSpace = mul(unity_ObjectToWorld,v.vertex);
                o.uv=v.uv;
                //o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //half ReNormFactor = 1.0 / length(i.normal);
                float4 Color = float4(0,0,0,0);
                float4 Texture = tex2D(_Albedo, float2((i.uv.x+_Albedo_ST.z)*_Albedo_ST.x,(i.uv.y+_Albedo_ST.w)*_Albedo_ST.y ));

                
                
                float dist = distance(_PlayerPos, i.vertexWorldSpace);
                float4 temp = lerp(Texture, (Texture.r+Texture.g+Texture.b)/3, clamp(dist * dist * _GSPower, 0, 1));
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
