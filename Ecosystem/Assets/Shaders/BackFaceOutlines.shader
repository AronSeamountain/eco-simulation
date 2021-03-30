Shader "Outlines/BackFaceOutlines"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _Thickness("Thickness", Float) = 1
        _Color("Color", Color) = (1, 1, 1, 1)
        //If enabled, this shader will use "smoothed" normals stored in TEXCOORD1 to extrude along
        [Toggle(USE_PRECALCULATED_OUTLINE_NORMALS)]_PrecalculateNormals("Use UV1 normals", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}

        Pass
        {
            Name "Outlines"
            
            Cull Front 
            
            HLSLPROGRAM
            //Standard URP requirements
            #pragma prefer_hlslcc gles
            #pragma exclude_rederers d3d11_9x

             // Register our material keywords
            #pragma shader_feature USE_PRECALCULATED_OUTLINE_NORMALS

            //Register our functions
            #pragma vertex Vertex
            #pragma fragment Fragment

            //Include our logic file
            #include "BackFaceOutlines.hlsl"
      
            ENDHLSL
        }
    }
}
