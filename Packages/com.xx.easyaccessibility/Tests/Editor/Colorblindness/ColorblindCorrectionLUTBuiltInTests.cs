using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Object;
using static EasyAccessibility.AccessibilitySettings.ColorblindCorrectionMode;

namespace EasyAccessibility.Tests.Colorblind
{
    public class ColorblindCorrectionLUTBuiltInTests
    {
        GameObject m_go;
        ColorblindCorrectionLUTBuiltIn m_component;
        AccessibilitySettings m_settings;
        List<Object> m_cleanup;

        static readonly FieldInfo k_Instance =
            typeof(AccessibilitySettings).GetField("m_instance", BindingFlags.NonPublic | BindingFlags.Static);
        static readonly FieldInfo k_RenderMaterial =
            typeof(ColorblindCorrectionLUTBuiltIn).GetField("m_renderMaterial", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_OverrideSettings =
            typeof(ColorblindCorrectionLUTBuiltIn).GetField("overrideSettings", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Mode =
            typeof(ColorblindCorrectionLUTBuiltIn).GetField("mode", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Amount =
            typeof(ColorblindCorrectionLUTBuiltIn).GetField("amount", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_TextureProtanopia =
            typeof(ColorblindCorrectionLUTBuiltIn).GetField("textureProtanopia", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_TextureDeutranopia =
            typeof(ColorblindCorrectionLUTBuiltIn).GetField("textureDeutranopia", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_TextureTritanopia =
            typeof(ColorblindCorrectionLUTBuiltIn).GetField("textureTritanopia", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly MethodInfo k_OnRenderImage =
            typeof(ColorblindCorrectionLUTBuiltIn).GetMethod("OnRenderImage", BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<Object>();
            m_go = new GameObject();
            m_component = m_go.AddComponent<ColorblindCorrectionLUTBuiltIn>();
            m_settings = ScriptableObject.CreateInstance<AccessibilitySettings>();
            k_Instance.SetValue(null, m_settings);
        }

        [TearDown]
        public void TearDown()
        {
            k_Instance.SetValue(null, null);
            DestroyImmediate(m_go);
            DestroyImmediate(m_settings);
            foreach (var obj in m_cleanup)
                if (obj != null) DestroyImmediate(obj);
        }

        #region Helpers

        T Track<T>(T obj) where T : Object { m_cleanup.Add(obj); return obj; }

        Material MakeMaterial() => Track(new Material(Shader.Find("EasyAccessibility/BuiltIn/ColorblindCorrectionLUT")));

        Texture3D MakeTexture3D() => Track(new Texture3D(1, 1, 1, TextureFormat.RGBA32, false));

        void InvokeOnRenderImage()
        {
            var src = new RenderTexture(1, 1, 0);
            var dst = new RenderTexture(1, 1, 0);
            src.Create();
            dst.Create();
            k_OnRenderImage.Invoke(m_component, new object[] { src, dst });
            src.Release();
            dst.Release();
            DestroyImmediate(src);
            DestroyImmediate(dst);
        }

        #endregion

        #region Guard Conditions

        [Test]
        public void OnRenderImage_NullMaterial_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => InvokeOnRenderImage());
        }

        #endregion

        #region Normal Path (overrideSettings = false, reads from AccessibilitySettings)

        [Test]
        public void NormalPath_Mode_MatchesSettings()
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            m_settings.colorblindCorrectionMode = Deuteranopia;

            InvokeOnRenderImage();

            Assert.AreEqual((int)Deuteranopia, mat.GetInt("_Mode"));
        }

        [Test]
        public void NormalPath_Amount_MatchesSettings()
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            m_settings.colorblindCorrectionAmount = 0.75f;

            InvokeOnRenderImage();

            Assert.AreEqual(0.75f, mat.GetFloat("_Amount"), 1e-5f);
        }

        [Test]
        [TestCase(Protanopia)]
        [TestCase(Deuteranopia)]
        [TestCase(Tritanopia)]
        [TestCase(None)]
        public void NormalPath_LUT_MatchesSettingsMode(AccessibilitySettings.ColorblindCorrectionMode mode)
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            var texProto = MakeTexture3D();
            var texDeut  = MakeTexture3D();
            var texTrit  = MakeTexture3D();
            k_TextureProtanopia.SetValue(m_component, texProto);
            k_TextureDeutranopia.SetValue(m_component, texDeut);
            k_TextureTritanopia.SetValue(m_component, texTrit);
            m_settings.colorblindCorrectionMode = mode;

            InvokeOnRenderImage();

            Texture3D expected;
            switch (mode)
            {
                case Protanopia:   expected = texProto; break;
                case Deuteranopia: expected = texDeut;  break;
                case Tritanopia:   expected = texTrit;  break;
                default:           expected = null;     break;
            }
            Assert.AreSame(expected, mat.GetTexture("_LUT"));
        }

        #endregion

        #region Override Path (overrideSettings = true, reads from local serialized fields)

        [Test]
        public void OverridePath_Mode_UsesLocalField_NotSettings()
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            m_settings.colorblindCorrectionMode = Protanopia;
            k_Mode.SetValue(m_component, Tritanopia);
            k_OverrideSettings.SetValue(m_component, true);

            InvokeOnRenderImage();

            Assert.AreEqual((int)Tritanopia, mat.GetInt("_Mode"));
        }

        [Test]
        public void OverridePath_Amount_UsesLocalField_NotSettings()
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            m_settings.colorblindCorrectionAmount = 0.25f;
            k_Amount.SetValue(m_component, 0.9f);
            k_OverrideSettings.SetValue(m_component, true);

            InvokeOnRenderImage();

            Assert.AreEqual(0.9f, mat.GetFloat("_Amount"), 1e-5f);
        }

        [Test]
        public void OverridePath_LUT_UsesLocalModeTexture_NotSettingsMode()
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            var texProto = MakeTexture3D();
            var texDeut  = MakeTexture3D();
            k_TextureProtanopia.SetValue(m_component, texProto);
            k_TextureDeutranopia.SetValue(m_component, texDeut);
            m_settings.colorblindCorrectionMode = Protanopia;
            k_Mode.SetValue(m_component, Deuteranopia);
            k_OverrideSettings.SetValue(m_component, true);

            InvokeOnRenderImage();

            Assert.AreSame(texDeut, mat.GetTexture("_LUT"));
        }

        #endregion

        #region Override Disabled

        [Test]
        public void OverrideDisabled_LocalFields_DoNotAffectMaterial()
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            m_settings.colorblindCorrectionMode = Protanopia;
            m_settings.colorblindCorrectionAmount = 0.5f;
            k_Mode.SetValue(m_component, Tritanopia);
            k_Amount.SetValue(m_component, 0.9f);
            // overrideSettings = false (default)

            InvokeOnRenderImage();

            Assert.AreEqual((int)Protanopia, mat.GetInt("_Mode"));
            Assert.AreEqual(0.5f, mat.GetFloat("_Amount"), 1e-5f);
        }

        #endregion
    }
}