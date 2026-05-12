using System;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static EasyAccessibility.AccessibilitySettings.ColorblindCorrectionMode;

namespace EasyAccessibility.Tests.Colorblind
{
    
    public class DaltonizationLUTGeneratorTests
    {
        const string k_BasePath = "Packages/com.xx.easyaccessibility/Runtime/Colorblindness/Rendering";
        const int k_TestResolution = 4;

        [TearDown]
        public void TearDown()
        {
            foreach (var mode in new[] { Protanopia, Deuteranopia, Tritanopia })
            {
                var path = TempAssetPath(mode);
                if (AssetDatabase.LoadAssetAtPath<Texture3D>(path) != null)
                    AssetDatabase.DeleteAsset(path);
            }
        }

        static string TempAssetPath(AccessibilitySettings.ColorblindCorrectionMode mode) =>
            $"Assets/LUT_Temp_{Enum.GetName(typeof(AccessibilitySettings.ColorblindCorrectionMode), mode)}.asset";

        // data[r + g*res + b*res*res] = daltonized(Color(r/scale, g/scale, b/scale))
        static int LutIndex(int r, int g, int b) =>
            r + g * k_TestResolution + b * k_TestResolution * k_TestResolution;

        #region Guard Conditions

        [Test]
        public void GenerateLUT_None_LogsAndSkipsGeneration()
        {
            LogAssert.Expect(LogType.Log, new System.Text.RegularExpressions.Regex("No need to generate"));
            Assert.DoesNotThrow(() => DaltonizationLUTGenerator.GenerateLUT(None));
        }

        [Test]
        public void GenerateLUT_InvalidMode_LogsErrorAndSkipsGeneration()
        {
            var badMode = (AccessibilitySettings.ColorblindCorrectionMode)99;
            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex("No matrix for"));
            Assert.DoesNotThrow(() => DaltonizationLUTGenerator.GenerateLUT(badMode));
        }

        #endregion

        #region Asset Creation (resolution=4, writes to temp paths)

        [Test]
        public void GenerateLUT_Protanopia_CreatesAssetAtExpectedPath()
        {
            var path = TempAssetPath(Protanopia);
            DaltonizationLUTGenerator.GenerateLUT(Protanopia, k_TestResolution, path);
            AssetDatabase.Refresh();
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<Texture3D>(path));
        }

        [Test]
        public void GenerateLUT_Deuteranopia_CreatesAssetAtExpectedPath()
        {
            var path = TempAssetPath(Deuteranopia);
            DaltonizationLUTGenerator.GenerateLUT(Deuteranopia, k_TestResolution, path);
            AssetDatabase.Refresh();
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<Texture3D>(path));
        }

        [Test]
        public void GenerateLUT_Tritanopia_CreatesAssetAtExpectedPath()
        {
            var path = TempAssetPath(Tritanopia);
            DaltonizationLUTGenerator.GenerateLUT(Tritanopia, k_TestResolution, path);
            AssetDatabase.Refresh();
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<Texture3D>(path));
        }

        [Test]
        public void GenerateLUT_CreatedAsset_IsTexture3D_WithExpectedDimensions()
        {
            var path = TempAssetPath(Protanopia);
            DaltonizationLUTGenerator.GenerateLUT(Protanopia, k_TestResolution, path);
            AssetDatabase.Refresh();
            var lut = AssetDatabase.LoadAssetAtPath<Texture3D>(path);
            Assert.AreEqual(k_TestResolution, lut.width);
            Assert.AreEqual(k_TestResolution, lut.height);
            Assert.AreEqual(k_TestResolution, lut.depth);
        }

        #endregion

        #region Mathematical Invariants (in-memory via ComputeLUT, no disk I/O)

        [Test]
        public void GenerateLUT_BlackMapsToBlack_ForAllModes()
        {
            foreach (var mode in new[] { Protanopia, Deuteranopia, Tritanopia })
            {
                var data = DaltonizationLUTGenerator.ComputeLUT(mode, k_TestResolution);
                Assert.AreEqual(Color.black, data[LutIndex(0, 0, 0)],
                    $"Expected black→black for {mode}");
            }
        }

        // RR = 0*R + 0*G + 0*B, so final red output is always 0 + original_r = original_r.
        [Test]
        public void GenerateLUT_RedChannelPreserved_ForAllModes()
        {
            float scale = k_TestResolution - 1;
            foreach (var mode in new[] { Protanopia, Deuteranopia, Tritanopia })
            {
                var data = DaltonizationLUTGenerator.ComputeLUT(mode, k_TestResolution);
                for (int r = 0; r < k_TestResolution; r++)
                {
                    Assert.AreEqual(r / scale, data[LutIndex(r, 0, 0)].r, 1e-5f,
                        $"Red channel not preserved at r={r} for {mode}");
                }
            }
        }

        [Test]
        public void GenerateLUT_OutputClampedToUnitRange()
        {
            var data = DaltonizationLUTGenerator.ComputeLUT(Protanopia, k_TestResolution);
            foreach (var p in data)
            {
                Assert.GreaterOrEqual(p.r, 0f); Assert.LessOrEqual(p.r, 1f);
                Assert.GreaterOrEqual(p.g, 0f); Assert.LessOrEqual(p.g, 1f);
                Assert.GreaterOrEqual(p.b, 0f); Assert.LessOrEqual(p.b, 1f);
            }
        }

        [Test]
        public void GenerateLUT_DifferentModesProduceDifferentOutput()
        {
            var modes = new[] { Protanopia, Deuteranopia, Tritanopia };
            int mid = k_TestResolution / 2;
            int idx = LutIndex(mid, mid, mid);
            var pixels = new Color[3];
            for (int i = 0; i < modes.Length; i++)
                pixels[i] = DaltonizationLUTGenerator.ComputeLUT(modes[i], k_TestResolution)[idx];

            Assert.IsFalse(
                pixels[0] == pixels[1] && pixels[1] == pixels[2],
                "Expected different CVD matrices to produce different output at mid-point");
        }

        #endregion

        // #region GenerateLUTS (SLOW: full resolution=256, overwrites committed assets)

        // [Test]
        // public void GenerateLUTS_CreatesAssetsForAllNonNoneModes()
        // {
        //     DaltonizationLUTGenerator.GenerateLUTS();
        //     AssetDatabase.Refresh();
        //     Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<Texture3D>($"{k_BasePath}/LUT_Protanopia.asset"));
        //     Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<Texture3D>($"{k_BasePath}/LUT_Deuteranopia.asset"));
        //     Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<Texture3D>($"{k_BasePath}/LUT_Tritanopia.asset"));
        // }

        // [Test]
        // public void GenerateLUTS_DoesNotCreateAssetForNoneMode()
        // {
        //     DaltonizationLUTGenerator.GenerateLUTS();
        //     Assert.IsNull(AssetDatabase.LoadAssetAtPath<Texture3D>($"{k_BasePath}/LUT_None.asset"));
        // }

        // #endregion
    }
}