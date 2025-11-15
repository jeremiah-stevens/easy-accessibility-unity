using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyAccessibility
{
    public class DaltonizationLUTGenerator : EditorWindow
    {
        static double[] Protanope = {
                0.0, 2.02344, -2.52581,
                0.0, 1.0,      0.0,
                0.0, 0.0,      1.0
            };

        static double[] Deutranope = { // greens are greatly reduced (1% men)
		        1.0,      0.0, 0.0,
                0.494207, 0.0, 1.24827,
                0.0,      0.0, 1.0
            };

        static double[] Tritanope = {
                1.0,       0.0,      0.0,
                0.0,       1.0,      0.0,
                -0.395913, 0.801109, 0.0
            };

        [MenuItem("Window/Easy Accessibility/Generate LUTS")]
        public static void GenerateLUTS()
        {
            foreach (var curr in Enum.GetValues(typeof(ColorblindSettings.ColorblindMode)).Cast<ColorblindSettings.ColorblindMode>())
            {
                if (curr == ColorblindSettings.ColorblindMode.None) continue; //skip basic
                GenerateLUT(curr);
            }
        }

        
        //source: https://miko.art/labs/Color-Vision/Javascript/Color.Vision.Daltonize.js
        public static void GenerateLUT(ColorblindSettings.ColorblindMode mode = ColorblindSettings.ColorblindMode.Protanopia)
        {
            int resolution = 256;
            var data = new Color[resolution * resolution * resolution];

            if(mode == ColorblindSettings.ColorblindMode.None)
            {
                Debug.Log("No need to generate a CLUT for identity/normal vision.");
                return;
            }

            double[] cvdMatrix;
            switch(mode)
            {
                case ColorblindSettings.ColorblindMode.Protanopia:
                    cvdMatrix = Protanope;
                    break;
                case ColorblindSettings.ColorblindMode.Deutranopia:
                    cvdMatrix = Deutranope;
                    break;
                case ColorblindSettings.ColorblindMode.Tritanopia:
                    cvdMatrix = Tritanope;
                    break;
                default:
                    Debug.LogError($"No matrix for {mode}.");
                    return;
            }

            int i = 0;
            for(float b = 0; b < resolution; b++)
            {
                for (float g = 0; g < resolution; g++)
                {
                    for (float r = 0; r < resolution; r++)
                    {
                        data[i++] = new Color(r / 255, g / 255, b / 255);
                    }
                }
            }


            int id = 0;
            for (int rIndex = 0; rIndex < resolution; rIndex++)
            {
                for (int gIndex = 0; gIndex < resolution; gIndex++)
                {
                    for (int bIndex = 0; bIndex < resolution; bIndex++)
                    {
                        Color currCol = data[id];
                        float r = currCol.r * 255;
                        float g = currCol.g * 255;
                        float b = currCol.b * 255;

                        //RGB to LMS matrix conversion
                        var L = (17.8824 * r) + (43.5161 * g) + (4.11935 * b);
                        var M = (3.45565 * r) + (27.1554 * g) + (3.86714 * b);
                        var S = (0.0299566 * r) + (0.184309 * g) + (1.46709 * b);
                        //simulate color blindness
                        var l = (cvdMatrix[0] * L) + (cvdMatrix[1] * M) + (cvdMatrix[2] * S);
                        var m = (cvdMatrix[3] * L) + (cvdMatrix[4] * M) + (cvdMatrix[5] * S);
                        var s = (cvdMatrix[6] * L) + (cvdMatrix[7] * M) + (cvdMatrix[8] * S);
                        // LMS to RGB matrix conversion
                        var R = (0.0809444479 * l) + (-0.130504409 * m) + (0.116721066 * s);
                        var G = (-0.0102485335 * l) + (0.0540193266 * m) + (-0.113614708 * s);
                        var B = (-0.000365296938 * l) + (-0.00412161469 * m) + (0.693511405 * s);
                        // Isolate invisible colors to color vision deficiency (calculate error matrix)
                        R = r - R;
                        G = g - G;
                        B = b - B;
                        // Shift colors towards visible spectrum (apply error modifications)
                        var RR = (0.0 * R) + (0.0 * G) + (0.0 * B);
                        var GG = (0.7 * R) + (1.0 * G) + (0.0 * B);
                        var BB = (0.7 * R) + (0.0 * G) + (1.0 * B);
                        // Add compensation to original values
                        R = RR + r;
                        G = GG + g;
                        B = BB + b;

                        R = Mathf.Clamp((float)R, 0, 255);
                        G = Mathf.Clamp((float)G, 0, 255);
                        B = Mathf.Clamp((float)B, 0, 255);

                        data[id++] = new Color((float)R / 255, (float)G / 255, (float)B / 255);
                    }
                }
            }


            //TODO: better to write to a Texture2D and convert format for compression settings, but not working properly
            //var output = new Texture2D(4096, 4096, TextureFormat.RGBA32, false);
            //output.SetPixels(data);
            //output.Apply();
            //byte[] bytes = output.EncodeToPNG();
            //File.WriteAllBytes(Application.dataPath + "/../Packages/com.xx.easyaccessibility/Runtime/Colorblindness/LUT_{Enum.GetName(typeof(ColorblindSettings.ColorblindMode), mode)}.png", bytes);
            ////AssetDatabase.CreateAsset(output, "Packages/com.xx.easyaccessibility/clut.png");

            var output = new Texture3D(resolution, resolution, resolution, TextureFormat.RGBA32, false);
            output.SetPixels(data);
            output.Apply();
            AssetDatabase.CreateAsset(output, $"Packages/com.xx.easyaccessibility/Runtime/Colorblindness/LUT_{Enum.GetName(typeof(ColorblindSettings.ColorblindMode), mode)}.asset");
        }
    }
}