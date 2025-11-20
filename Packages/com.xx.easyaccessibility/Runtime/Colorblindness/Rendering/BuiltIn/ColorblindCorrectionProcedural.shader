//source: https://miko.art/labs/Color-Vision/Javascript/Color.Vision.Daltonize.js
Shader "EasyAccessibility/Colorblind Correction (Procedural)"
{   
    Properties
    {
        _Mode ("Color Mode", int) = 0
        _Amount ("Adjustment Amount", float) = 1
        _MainTex ("Texture", 2D) = "white" {}
    }

   SubShader
   {
       Cull Off ZWrite Off ZTest Always
       Tags { "Queue"="Transparent" "RenderType"="Transparent" }
       Zwrite On 
       Blend SrcAlpha OneMinusSrcAlpha

       Pass
       {
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #include "UnityCG.cginc"

           sampler2D _MainTex;
           int _Mode;
           float _Amount;

           struct appdata
           {
               float4 vertex : POSITION;
               float2 uv : TEXCOORD0;
           };
           
           struct v2f
           {
               float2 uv : TEXCOORD0;
               float4 vertex : SV_POSITION;
           };

           v2f vert (appdata v)
           {
               v2f o;
               o.vertex = UnityObjectToClipPos(v.vertex);
               o.uv = v.uv;
               return o;
           }

           half4 frag(v2f i) : SV_Target
           {
               half3 col_rgb = tex2D(_MainTex, i.uv);

               if(_Mode == 0)
               {
                   return half4(col_rgb.r, col_rgb.g, col_rgb.b, 1);
               }

               //convert to lms colorspace
               float3x3 rgb_to_lms =
               {
                    17.8824,  43.5161, 4.11935,
                    3.45565, 27.1554, 3.86714,
                    0.02996,  0.1843, 1.46709
               };
               half3 col_LMS = mul(rgb_to_lms, col_rgb);

               //isolate for difficult items to see
               half3 col_lms = half3(1,1,1);
               if(_Mode == 1)
               {
                   float3x3 protanope =
                   {
                        0.0, 2.02344, -2.52581,
                        0.0, 1.0,      0.0    ,
                        0.0, 0.0,      1.0    
                   };
                   col_lms = mul(protanope, col_LMS);
               }
               if(_Mode == 2)
               {
                   float3x3 deuteranope = 
                   {
                       1.0,      0.0,  0.0    ,
                       0.494207, 0.0,  1.24827,
                       0.0,      0.0,  1.0
                   };
                   col_lms = mul(deuteranope, col_LMS);
               }
               if(_Mode == 3)
               {
                   float3x3 tritanope = 
                   {
                       1.0,       0.0,      0.0,
                       0.0,       1.0,      0.0,
                      -0.395913,  0.801109, 0.0
                   };
                   col_lms = mul(tritanope, col_LMS);
               }

               //convert back to rgb space
               float3x3 lms_to_rgb = 
               {
                   0.0809444479,   -0.130504409,   0.116721066,
                  -0.0102485335,    0.0540193266, -0.113614708,
                  -0.000365296938, -0.00412161469, 0.693511405
               };
               half3 col_RGB = mul(lms_to_rgb, col_lms);

               //isolate invisible colors
               col_RGB = col_rgb - col_RGB;

               //shift toward visible spectrum
               float3x3 visibility_adjustment =
               {
                   0.0, 0.0, 0.0,
                   0.7, 1.0, 0.0,
                   0.7, 0.0, 1.0
               };
               half3 col_RRGGBB = mul(visibility_adjustment, col_RGB);

               //add compensation
               col_RGB = col_RRGGBB + col_rgb;

               // Modify the sampled color
               return lerp(half4(col_rgb.r, col_rgb.g, col_rgb.b, 1), half4(col_RGB.r, col_RGB.g, col_RGB.b, 1), _Amount);
           }

           ENDCG
       }
   }
}