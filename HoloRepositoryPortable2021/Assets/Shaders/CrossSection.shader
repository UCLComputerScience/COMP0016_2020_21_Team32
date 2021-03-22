//Relied heavily on these resources and examples to create this shader:
//https://www.youtube.com/watch?v=zCkC5e_Pkz4&list=PLX2vGYjWbI0RS_lkb68ApE2YPcZMC4Ohz
//https://github.com/Dandarawy/Unity3DCrossSectionShader/blob/master/Assets/Cross%20Section%20Shader/Shaders/OnePlaneBSP.shader 
//https://forum.unity.com/threads/transparent-depth-shader-good-for-ghosts.149511/
//GLTFUTILITY/Materials/Standard Transparent (Metallic)
//https://docs.unity3d.com/Manual/SL-SurfaceShaders.html

Shader "Custom/Clipping" {

   //Properties are the same as those in the GLTFUTILITY/Materials/Standard Transparent (Metallic) shader, with the addition of the 
   //PlaneNormal and PlanePosition properties

   Properties {
	  _Color ("Color", Color) = (0.5,0.5,0.5,1)  
    _MainTex ("Main Texture", 2D) = "white" {}
	  _MetallicGlossMap ("Metallic (B) Gloss (G)", 2D) = "white" {}          
    _Metallic ("Metallic", Range(0,1)) = 1.0
	  _Roughness ("Roughness", Range(0,1)) = 1
	 [Normal] _BumpMap ("Normal", 2D) = "bump" {}
	 _BumpScale("NormalScale", Float) = 1.0

	 _PlaneNormal("PlaneNormal",Vector) = (0,1,0,0)
	 _PlanePosition("PlanePosition",Vector) = (0,0,0,1)
    }
    

    SubShader { 
       Tags {  "RenderType"="Transparent" "RenderQueue"= "Transparent" }
       LOD 200
       Cull Off

       Pass //Pass causes the geometry to be rendered once only
       {
         ZWrite On //despite being renderered as transparent, ZWrite is set to on to force the gpu to write to the z-buffer. 
         //This prevents the strange effects that occur when changing the opacity of multiple complex meshes over the top of one another
         ColorMask 0

		     offset 0, 1
 
         CGPROGRAM
         #pragma vertex vert alpha:fade //sets the function "vert" to be used as the vertex function
         #pragma fragment frag alpha:fade //sets the function "frag" to be used as the fragment function
         #include "UnityCG.cginc"
     
         float3 _PlaneNormal;
		     float3 _PlanePosition;
 
        //v2f - "vertex to fragment" 
         struct v2f {
           float4 pos : SV_POSITION; //Vertex position in object space
           float4 worldPos : world ; //position in world space
           half4 col : COLOR;
         };
        //vertex function - runs on each vertex of the model
         v2f vert (appdata_base v)
         {
           v2f o;      
           o.worldPos =  mul(unity_ObjectToWorld, v.vertex); //matrix multiplication - transforms object space position (v.vertex) to world space
             o.pos = UnityObjectToClipPos (v.vertex); //transforms object space position to "clip space"
           return o;
         }

         //fragment function - runs on each pixel on the screen
         half4 frag (v2f i) : COLOR
         {

		        float dist = dot(i.worldPos - _PlanePosition, _PlaneNormal);
            //calculates the dot product between:
            // - vector between the plane's centre and the current pixel on our model in world space
            // - the plane's normal
            //if this value is positive then the pixel is below the plane, otherwise it's above.
		        clip(-dist); //discards the current pixel if dist is positive, ie does not draw pixel currently being processed if it's ABOVE the plane
         
           return i.col; 
         }
         ENDCG  
       }
    CGPROGRAM
 
    #pragma surface surf Standard alpha:fade
    #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;

    //struct containing data from the model's mesh
    struct Input {
      float2 uv_MainTex; 
      float2 uv_BumpMap;
      float2 uv_MetallicGlossMap;
      float2 uv_Roughness;       
      float3 worldPos;  
      float4 color : COLOR;
         
    };  

    half _Glossiness;
    half _Metallic;
    half _Roughness;
    half _BumpScale;
    float4 _Color;

    float3 _PlaneNormal;
    float3 _PlanePosition;

    //SurfaceOutputStandard is a struct that describes the properties of the surface.
    //The surf function is required for all surface shaders, which changes the properties of the
    //surface based on the values of the input we pass (IN). IN has type Input (defined above).
    //The surf function here is very similar to the one used iN the 
    //GLTFUTILITY/Materials/Standard Transparent (Metallic) shader.
       
    void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			//o.Albedo = c.rgb * IN.color;
			o.Alpha = c.a;
      o.Albedo = c.rgb;
			fixed4 m = tex2D (_MetallicGlossMap, IN.uv_MetallicGlossMap);
			o.Metallic = m.b * _Metallic;
			
			o.Smoothness = 1 - (m.g * _Roughness);

			o.Normal = UnpackScaleNormal(tex2D (_BumpMap, IN.uv_BumpMap), _BumpScale);

			float dist = dot(IN.worldPos - _PlanePosition, _PlaneNormal); 
		  clip(-dist); //discards any pixels above the plane
    }
    ENDCG    
    }
    Fallback "Standard"
  }

