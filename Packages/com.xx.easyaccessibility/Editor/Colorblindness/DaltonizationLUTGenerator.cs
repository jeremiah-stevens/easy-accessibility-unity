using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// Generates look-up tables for use in LUT-based approaches.
    /// </summary>
    public class DaltonizationLUTGenerator : EditorWindow
    {
        static double[] Protanope = {
                0.0, 2.02344, -2.52581,
                0.0, 1.0,      0.0,
                0.0, 0.0,      1.0
            };

        static double[] Deuteranope = {
		        1.0,      0.0, 0.0,
                0.494207, 0.0, 1.24827,
                0.0,      0.0, 1.0
            };

        static double[] Tritanope = {
                1.0,       0.0,      0.0,
                0.0,       1.0,      0.0,
                -0.395913, 0.801109, 0.0
            };

        [MenuItem("Window/Easy Accessibility/Config/Generate LUTS")]
        public static void GenerateLUTS()
        {
            foreach (var curr in Enum.GetValues(typeof(AccessibilitySettings.ColorblindCorrectionMode)).Cast<AccessibilitySettings.ColorblindCorrectionMode>())
            {
                if (curr == AccessibilitySettings.ColorblindCorrectionMode.None) continue; //skip basic
                GenerateLUT(curr);
            }
        }

        //source: https://miko.art/labs/Color-Vision/Javascript/Color.Vision.Daltonize.js
        // Returns null and logs an error if mode is unrecognised.
        // data[r + g*resolution + b*resolution*resolution] = daltonized(Color(r/scale, g/scale, b/scale))
        public static Color[] ComputeLUT(
            AccessibilitySettings.ColorblindCorrectionMode mode,
            int resolution = 256
        )
        {
            double[] cvdMatrix;
            switch(mode)
            {
                case AccessibilitySettings.ColorblindCorrectionMode.Protanopia:
                    cvdMatrix = Protanope;
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Deuteranopia:
                    cvdMatrix = Deuteranope;
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Tritanopia:
                    cvdMatrix = Tritanope;
                    break;
                default:
                    Debug.LogError($"No matrix for {mode}.");
                    return null;
            }

            var data = new Color[resolution * resolution * resolution];
            float scale = resolution - 1;

            int i = 0;
            for(float b = 0; b < resolution; b++)
            {
                for (float g = 0; g < resolution; g++)
                {
                    for (float r = 0; r < resolution; r++)
                    {
                        data[i++] = new Color(r / scale, g / scale, b / scale);
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
                        float r = currCol.r * scale;
                        float g = currCol.g * scale;
                        float b = currCol.b * scale;

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

                        R = Mathf.Clamp((float)R, 0, scale);
                        G = Mathf.Clamp((float)G, 0, scale);
                        B = Mathf.Clamp((float)B, 0, scale);

                        data[id++] = new Color((float)R / scale, (float)G / scale, (float)B / scale);
                    }
                }
            }

            return data;
        }

        public static void GenerateLUT(
            AccessibilitySettings.ColorblindCorrectionMode mode = AccessibilitySettings.ColorblindCorrectionMode.Protanopia,
            int resolution = 256,
            string outputPath = null
        )
        {
            if(mode == AccessibilitySettings.ColorblindCorrectionMode.None)
            {
                Debug.Log("No need to generate a LUT for identity/normal vision.");
                return;
            }

            var data = ComputeLUT(mode, resolution);
            if (data == null) return;

            //TODO: write to Texture2D would be more performant, but runs into rendering issues right now

            var assetPath = outputPath ?? $"Packages/com.xx.easyaccessibility/Runtime/Colorblindness/Rendering/LUT_{Enum.GetName(typeof(AccessibilitySettings.ColorblindCorrectionMode), mode)}.asset";
            var output = new Texture3D(resolution, resolution, resolution, TextureFormat.RGBA32, false);
            output.SetPixels(data);
            output.Apply();
            AssetDatabase.CreateAsset(output, assetPath);
        }
    }
}