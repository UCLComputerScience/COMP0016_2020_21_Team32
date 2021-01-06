Shader "GLTFUtility/Standard Transparent (Metallic)" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetallicGlossMap ("Metallic (B) Gloss (G)", 2D) = "white" {}
		_Roughness ("Roughness", Range(0,1)) = 1
		_Metallic ("Metallic", Range(0,1)) = 1
		[Normal] _BumpMap ("Normal", 2D) = "bump" {}
		_BumpScale("NormalScale", Float) = 1.0
		_OcclusionMap ("Occlusion (R)", 2D) = "white" {}
		_EmissionMap ("Emission", 2D) = "black" {}
		_EmissionColor ("Emission Color", Color) = (0,0,0,0)
	}
SubShader {
    Tags {"RenderType"="Transparent"}
    LOD 200

    // extra pass that renders to depth buffer only
    Pass {
        ZWrite On
        ColorMask 0

        Offset 0, 1
    }

    // paste in forward rendering passes from Transparent/Diffuse
    UsePass "Transparent/Diffuse/FORWARD"
}
//Fallback "Transparent/VertexLit"
}
