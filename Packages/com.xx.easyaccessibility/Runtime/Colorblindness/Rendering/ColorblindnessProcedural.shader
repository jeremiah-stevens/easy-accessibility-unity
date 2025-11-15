//source: https://miko.art/labs/Color-Vision/Javascript/Color.Vision.Daltonize.js
Shader "EasyAccessibility/ColorblindnessProcedural"
{   
    Properties
    {
        _Mode ("Color Mode", int) = 0
        _Amount ("Adjustment Amount", float) = 1
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

           const float protanope[9] =
           {
                0.0, 2.02344, -2.52581,
                0.0, 1.0,      0.0    ,
                0.0, 0.0,      1.0    
           };

           const float deutranope[9] = 
           {
               1.0,      0.0,  0.0    ,
               0.494207, 0.0,  1.24827,
               0.0,      0.0,  1.0
           };

           const float tritanope[9] = 
           {
               1.0,       0.0,      0.0,
               0.0,       1.0,      0.0,
               -0.395913, 0.801109, 0.0
           };




           float4 _ColorShift;
           Texture2D _LUT;
           int _Mode;
           float _Amount;

           float4 Frag(Varyings input) : SV_Target0
           {
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

               float2 uv = input.texcoord.xy;
               half4 col_rgb = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);

               if(_Mode == 0)
               {
                   return col_rgb;
               }

               half4 mod = col_rgb * 256;

               half4 col_LMS = half4(
                     (17.8824    * mod.r) + (43.5161   * mod.g) + (4.11935 * mod.b),
                     ( 3.45565   * mod.r) + (27.1554   * mod.g) + (3.86714 * mod.b),
                     ( 0.0299566 * mod.r) + ( 0.184309 * mod.g) + (1.46709 * mod.b),
                     col_rgb.a
                   );

               //TODO: something seems to be going wrong with these matrices; items are isolated but colors aren't behaving how I'd expect
               half4 col_lms = half4(1,1,1,1);
               if(_Mode == 1) //protanopia
               {
                    col_lms = half4(
                         (protanope[0] * col_LMS.r) + (protanope[1] * col_LMS.g) + (protanope[2] * col_LMS.b),
                         (protanope[3] * col_LMS.r) + (protanope[4] * col_LMS.g) + (protanope[5] * col_LMS.b),
                         (protanope[6] * col_LMS.r) + (protanope[7] * col_LMS.g) + (protanope[8] * col_LMS.b),
                         col_rgb.a
                       );
               }
               if(_Mode == 2) //deutranopia
               {
                    col_lms = half4(
                         (deutranope[0] * col_LMS.r) + (deutranope[1] * col_LMS.g) + (deutranope[2] * col_LMS.b),
                         (deutranope[3] * col_LMS.r) + (deutranope[4] * col_LMS.g) + (deutranope[5] * col_LMS.b),
                         (deutranope[6] * col_LMS.r) + (deutranope[7] * col_LMS.g) + (deutranope[8] * col_LMS.b),
                         col_rgb.a
                       );
               }
               else if(_Mode == 3) //tritanopia
               {
                    col_lms = half4(
                         (tritanope[0] * col_LMS.r) + (tritanope[1] * col_LMS.g) + (tritanope[2] * col_LMS.b),
                         (tritanope[3] * col_LMS.r) + (tritanope[4] * col_LMS.g) + (tritanope[5] * col_LMS.b),
                         (tritanope[6] * col_LMS.r) + (tritanope[7] * col_LMS.g) + (tritanope[8] * col_LMS.b),
                         col_rgb.a
                       );
               }

               half4 col_RGB = half4(
                     ( 0.0809444479   * col_lms.r) + (-0.130504409   * col_lms.g) + ( 0.116721066 * col_lms.b),
                     (-0.0102485335   * col_lms.r) + ( 0.0540193266  * col_lms.g) + (-0.113614708 * col_lms.b),
                     (-0.000365296938 * col_lms.r) + (-0.00412161469 * col_lms.g) + ( 0.693511405 * col_lms.b),
                     col_rgb.a
                   );

               //isolate invisible colors
               col_RGB = mod - col_RGB;


               //shift toward visible spectrum
               half4 col_RRGGBB = half4(
                     (0.0 * col_RGB.r) + (0.0 * col_RGB.g) + (0.0 * col_RGB.b),
                     (0.7 * col_RGB.r) + (1.0 * col_RGB.g) + (0.0 * col_RGB.b),
                     (0.7 * col_RGB.r) + (0.0 * col_RGB.g) + (1.0 * col_RGB.b),
                     col_rgb.a
                   );

               //add compensation
               col_RGB = col_RRGGBB + mod;
               col_RGB = half4(
                    max(0, min(255, col_RGB.r)),
                    max(0, min(255, col_RGB.g)),
                    max(0, min(255, col_RGB.b)),
                    max(0, min(255, col_RGB.a))
                    );

               col_RGB /= 256;

               // Modify the sampled color
               return (1 - _Amount) * col_rgb + _Amount * col_RGB;
           }

           ENDHLSL
       }
   }
}
