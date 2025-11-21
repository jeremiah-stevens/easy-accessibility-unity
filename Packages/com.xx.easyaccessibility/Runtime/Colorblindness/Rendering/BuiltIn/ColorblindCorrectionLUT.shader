//source: https://miko.art/labs/Color-Vision/Javascript/Color.Vision.Daltonize.js
Shader "EasyAccessibility/BuiltIn/ColorblindCorrectionLUT"
{   
    Properties
    {
        _Mode ("Color Mode", int) = 0
        _Amount ("Adjustment Amount", float) = 1
        _MainTex ("Texture", 2D) = "white" {}
        _LUT ("LUT", 3D) = "white" {}
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

           int _Mode;
           float _Amount;
           sampler2D _MainTex;
           sampler3D _LUT;

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

               //sample the look-up table based on the input color
               half3 col_adjusted = tex3D(_LUT, col_rgb);

               //apply the amount
               return lerp(half4(col_rgb.r, col_rgb.g, col_rgb.b, 1), half4(col_adjusted.r, col_adjusted.g, col_adjusted.b, 1), _Amount);
           }

           ENDCG
       }
   }
}