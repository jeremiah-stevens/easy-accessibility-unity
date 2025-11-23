Shader "EasyAccessibility/BuiltIn/ColorblindSimulation"
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

               //sRGB -> Linear
               half3 col_linear = pow(col_rgb, 1 / 2.2);

               half3 col_modified = half3(0,0,0);
               if(_Mode < 4) //protanopia, deuteranopia, tritanopia
               {
                    //linear -> LMS
                    float3x3 linear_to_lms =
                    {
                     0.3139902,  0.1553724,  0.01775239,
                     0.639513,   0.7578945,  0.1094421,
                     0.04649755, 0.08670142, 0.8725692
                    };
                    half3 col_LMS = mul(col_linear, linear_to_lms);

                    if(_Mode == 1)
                    {
                        float3x3 mod_protanopia =
                        {
                             0,       0, 0,
                             1.05118, 1, 0,
                            -0.05116, 0, 1
                        };
                        col_modified = mul(col_LMS, mod_protanopia);
                    }
                    if(_Mode == 2)
                    {
                        float3x3 mod_deuteranopia =
                        {
                             1, 0.9513, 0,
                             0, 0,      0,
                             0, 0.0487, 1
                        };
                        col_modified = mul(col_LMS, mod_deuteranopia);
                    }
                    if(_Mode == 3)
                    {
                        float3x3 mod_tritanopia =
                        {
                             1, 0, -0.86744,
                             0, 1,  1.86727,
                             0, 0,  0
                        };
                        col_modified = mul(col_LMS, mod_tritanopia);
                    }

                    //LMS -> linear
                    float3x3 lms_to_linear =
                    {
                         5.472212, -1.125242,  0.0298,
                        -4.64196,   2.293171, -0.19318,
                         0.16963,  -0.167895,  1.16364,
                    };
                    col_modified = mul(col_modified, lms_to_linear);
               }
               if(_Mode == 4) //monochromatism
               {
                    half3 mod_monochromatism = half3(0.01, 0.1, 0.87);

                    col_modified = dot(col_linear, mod_monochromatism);
               }
               if(_Mode == 5) //achromatopsia
               {
                    half3 mod_achromatopsia = half3(0.21, 0.71, 0.07);

                    col_modified = dot(col_linear, mod_achromatopsia);
               }


               col_modified = pow(col_modified, 2.2);

               //apply the amount
               return lerp(half4(col_rgb.r, col_rgb.g, col_rgb.b, 1), half4(col_modified.r, col_modified.g, col_modified.b, 1), _Amount);
           }

           ENDCG
       }
   }
}