//source: https://miko.art/labs/Color-Vision/Javascript/Color.Vision.Daltonize.js

Shader "EasyAccessibility/ColorblindnessLUT"
{   
    Properties
    {
        _Mode ("Color Mode", int) = 0
        _Amount ("Adjustment Amount", float) = 1
        _LUT ("LUT", 3D) = "white" {}
    }

   SubShader
   {
       Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
       ZWrite Off Cull Off
       Pass
       {
           Name "ColorblindnessPass"

           HLSLPROGRAM

           #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
           #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

           #pragma vertex Vert
           #pragma fragment Frag




           int _Mode;
           float _Amount;
           sampler3D _LUT;

           float4 Frag(Varyings input) : SV_Target0
           {
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

               float2 uv = input.texcoord.xy;
               half4 col_rgb = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);


               if(_Mode == 0)
               {
                   return float4(col_rgb.r, col_rgb.g, col_rgb.b, 1);
               }

               half4 col_corrected = tex3D(_LUT, half3(col_rgb.r, col_rgb.g, col_rgb.b));

               return (1 - _Amount) * col_rgb + _Amount * col_corrected;
           }

           ENDHLSL
       }
   }
}
