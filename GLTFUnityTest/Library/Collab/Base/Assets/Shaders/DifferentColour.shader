//Relied heavily on these resources and examples to create this shader:
//https://github.com/Dandarawy/Unity3DCrossSectionShader/blob/master/Assets/Cross%20Section%20Shader/Shaders/OnePlaneBSP.shader
//https://www.youtube.com/watch?v=zCkC5e_Pkz4&list=PLX2vGYjWbI0RS_lkb68ApE2YPcZMC4Ohz
//https://forum.unity.com/threads/transparent-depth-shader-good-for-ghosts.149511/
//GLTFUTILITY/Materials/Standard Transparent (Metallic)


Shader "Custom/DifferentColour" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MetallicGlossMap ("Metallic (B) Gloss (G)", 2D) = "white" {}
		_Metallic("Metallic", Range(0,1)) = 1
		_Roughness("Roughness", Range(0,1))=1
		_PlaneNormal("PlaneNormal",Vector) = (0,1,0,0)
		_PlanePosition("PlanePosition",Vector) = (0,0,0,1)

		[Normal] _BumpMap ("Normal", 2D) = "bump" {}
        BumpScale("NormalScale", Float) = 1.0

	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200

		Pass{
			ColorMask 0
			offset 0, 1
		}

		Cull Back
		// Cull Back
			CGPROGRAM
			
			// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows //alpha:fade

			// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_MetallicGlossMap;
			float3 worldPos;
			float4 color : COLOR;
		};

		half _Roughness;
		half _Metallic;
		half _BumpScale;
		fixed4 _Color;

		//half _Glossiness;
		fixed3 _PlaneNormal;
		fixed3 _PlanePosition;

		void surf(Input IN, inout SurfaceOutputStandard o){
			float dist = dot(IN.worldPos - _PlanePosition, _PlaneNormal);
			fixed4 c;
			if(dist > 0)c = (0,0,0,0); //sets everything that won't be drawn by the gpu to black
			else c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * IN.color;
			o.Alpha = c.a;

			fixed4 m = tex2D (_MetallicGlossMap, IN.uv_MetallicGlossMap);
			o.Metallic = m.b * _Metallic;
		
			o.Smoothness = 1 - (m.g * _Roughness);

			o.Normal = UnpackScaleNormal(tex2D (_BumpMap, IN.uv_BumpMap), _BumpScale);
		}
		ENDCG
		
		

	// 	Cull Front
	// 	CGPROGRAM
	// #pragma surface surf NoLighting  noambient //alpha:fade

	// 	struct Input {
	// 		half2 uv_MainTex;
	// 		float3 worldPos;

	// 	};
	// 	sampler2D _MainTex;
	// 	fixed4 _Color;
	// 	fixed4 _CrossColor;
	// 	fixed3 _PlaneNormal;
	// 	fixed3 _PlanePosition;
	// 	bool checkVisability(fixed3 worldPos)
	// 	{
	// 		float dotProd1 = dot(worldPos - _PlanePosition, _PlaneNormal);
	// 		return dotProd1 >0 ;
	// 	}
	// 	fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
	// 	{
	// 		fixed4 c;
	// 		c.rgb = s.Albedo;
	// 		c.a = s.Alpha;
	// 		return c;
	// 	}

	// 	void surf(Input IN, inout SurfaceOutput o)
	// 	{
	// 		if (checkVisability(IN.worldPos))discard;
	// 		o.Albedo = _CrossColor;

	// 	}
	// 		ENDCG
		
	}
	FallBack "Standard"
}

//used and modified: https://github.com/Dandarawy/Unity3DCrossSectionShader/blob/master/Assets/Cross%20Section%20Shader/Shaders/OnePlaneBSP.shader


// Shader "Custom/CrossSection" {
// 	Properties{
// 		_Color("Color", Color) = (1,1,1,1)
// 		_CrossColor("Cross Section Color", Color) = (1,1,1,1)
// 		_MainTex("Albedo (RGB)", 2D) = "white" {}
// 		_Metallic("Metallic", Range(0,1)) = 1
// 		[Normal] _BumpMap("Normal", 2D) = "Bump"{}
// 		_PlaneNormal("PlaneNormal",Vector) = (0,1,0,0)
// 		_PlanePosition("PlanePosition",Vector) = (0,0,0,1)
// 		_StencilMask("Stencil Mask", Range(0, 255)) = 255
		
// 		_MetallicGlossMap("Metallic (B) Gloss (G)", 2D) = "white"{}
// 		_Roughness("Roughness", Range(0, 1)) = 1
//         BumpScale("NormalScale", Float) = 1.0
// 		_OcclusionMap ("Occlusion (R)", 2D) = "white" {}
// 		_EmissionMap ("Emission", 2D) = "black" {}
// 		_EmissionColor ("Emission Color", Color) = (0,0,0,0)
// 	}
// 	SubShader {
// 		Tags { "RenderType"="Transparent"}
// 		//LOD 200
// 		Pass {
// 			ZWrite On
// 			ColorMask 0

// 			Offset 0, 1
// 		}
// 		Stencil
// 		{
// 			Ref [_StencilMask]
// 			CompBack Always
// 			PassBack Replace

// 			CompFront Always
// 			PassFront Zero
// 		}
// 		Cull Back
// 			CGPROGRAM
// 			// Physically based Standard lighting model, and enable shadows on all light types
// #pragma surface surf Standard fullforwardshadows

// 			// Use shader model 3.0 target, to get nicer looking lighting
// #pragma target 3.0

// 			sampler2D _MainTex;
// 			sampler2D _MetallicGlossMap;
// 			sampler2D _BumpMap;
// 			sampler2D _OcclusionMap;
// 			sampler2D _EmissionMap;

// 		struct Input {
// 			float2 uv_MainTex;
// 			float2 uv_BumpMap;
// 			float2 uv_MetallicGlossMap;
// 			float2 uv_EmissionMap;
// 			float2 uv_OcclusionMap;
// 			float4 color: COLOR;

// 			float3 worldPos;
// 		};

// 		//half _Glossiness;
// 		half _Metallic;
// 		fixed4 _Color;
// 		fixed4 _CrossColor;
// 		fixed3 _PlaneNormal;
// 		fixed3 _PlanePosition;

// 		half _Roughness;
// 		half _EmissionColor;
// 		half _BumpScale;
// 		half _AlphaCutoff;
// 		bool checkVisability(fixed3 worldPos)
// 		{
// 			float dotProd1 = dot(worldPos - _PlanePosition, _PlaneNormal);
// 			return dotProd1 > 0  ;
// 		}
// 		void surf(Input IN, inout SurfaceOutputStandard o) {
// 			if (checkVisability(IN.worldPos))discard;
// 			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
// 			o.Albedo = c.rgb * IN.color;
// 			o.Alpha = c.a;
// 			// Metallic and smoothness come from slider variables
// 			fixed4 m = tex2D(_MetallicGlossMap, IN.uv_MetallicGlossMap);
// 			o.Metallic = m.b* _Metallic;

// 			o.Smoothness = 1 - (m.g * _Roughness);

// 			o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_BumpMap), _BumpScale);
			
// 			o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap);

// 			o.Emission = tex2D(_EmissionMap, IN.uv_EmissionMap) *_EmissionColor;
// 		}
// 		ENDCG
		
// 			Cull Front
// 			CGPROGRAM
// #pragma surface surf NoLighting  noambient

// 		struct Input {
// 			half2 uv_MainTex;
// 			float3 worldPos;

// 		};
// 		sampler2D _MainTex;
// 		fixed4 _Color;
// 		fixed4 _CrossColor;
// 		fixed3 _PlaneNormal;
// 		fixed3 _PlanePosition;
// 		bool checkVisability(fixed3 worldPos)
// 		{
// 			float dotProd1 = dot(worldPos - _PlanePosition, _PlaneNormal);
// 			return dotProd1 >0 ;
// 		}
// 		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
// 		{
// 			fixed4 c;
// 			c.rgb = s.Albedo;
// 			c.a = s.Alpha;
// 			return c;
// 		}

// 		void surf(Input IN, inout SurfaceOutput o)
// 		{
// 			if (checkVisability(IN.worldPos))discard;
// 			o.Albedo = _CrossColor;

// 		}
// 			ENDCG
		
// 	}
// 	//FallBack "Diffuse"
// } 