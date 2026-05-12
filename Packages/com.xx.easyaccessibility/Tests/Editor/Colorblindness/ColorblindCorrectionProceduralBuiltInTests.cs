using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Object;
using static EasyAccessibility.AccessibilitySettings.ColorblindCorrectionMode;

namespace EasyAccessibility.Tests.Colorblind
{
    public class ColorblindCorrectionProceduralBuiltInTests
    {
        GameObject m_go;
        ColorblindCorrectionProceduralBuiltIn m_component;
        AccessibilitySettings m_settings;
        List<Object> m_cleanup;

        static readonly FieldInfo k_Instance =
            typeof(AccessibilitySettings).GetField("m_instance", BindingFlags.NonPublic | BindingFlags.Static);
        static readonly FieldInfo k_RenderMaterial =
            typeof(ColorblindCorrectionProceduralBuiltIn).GetField("m_renderMaterial", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_OverrideSettings =
            typeof(ColorblindCorrectionProceduralBuiltIn).GetField("overrideSettings", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Mode =
            typeof(ColorblindCorrectionProceduralBuiltIn).GetField("mode", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Amount =
            typeof(ColorblindCorrectionProceduralBuiltIn).GetField("amount", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly MethodInfo k_OnRenderImage =
            typeof(ColorblindCorrectionProceduralBuiltIn).GetMethod("OnRenderImage", BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<Object>();
            m_go = new GameObject();
            m_component = m_go.AddComponent<ColorblindCorrectionProceduralBuiltIn>();
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

        Material MakeMaterial() => Track(new Material(Shader.Find("EasyAccessibility/BuiltIn/ColorblindCorrectionProcedural")));

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