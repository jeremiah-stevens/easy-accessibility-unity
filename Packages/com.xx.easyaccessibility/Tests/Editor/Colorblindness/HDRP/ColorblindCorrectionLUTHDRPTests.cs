using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Object;
using static EasyAccessibility.AccessibilitySettings.ColorblindCorrectionMode;

namespace EasyAccessibility.Tests.Colorblind
{
    public class ColorblindCorrectionLUTHDRPTests
    {
        ColorblindCorrectionLUTHDRP m_component;
        AccessibilitySettings m_settings;
        List<Object> m_cleanup;

        static readonly FieldInfo k_Instance =
            typeof(AccessibilitySettings).GetField("m_instance", BindingFlags.NonPublic | BindingFlags.Static);
        static readonly MethodInfo k_Render =
            typeof(ColorblindCorrectionLUTHDRP).GetMethod("Render", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<Object>();
            m_component = ScriptableObject.CreateInstance<ColorblindCorrectionLUTHDRP>();
            m_settings = ScriptableObject.CreateInstance<AccessibilitySettings>();
            k_Instance.SetValue(null, m_settings);
        }

        [TearDown]
        public void TearDown()
        {
            k_Instance.SetValue(null, null);
            DestroyImmediate(m_component);
            DestroyImmediate(m_settings);
            foreach (var obj in m_cleanup)
                if (obj != null) DestroyImmediate(obj);
        }

        #region Helpers

        T Track<T>(T obj) where T : Object { m_cleanup.Add(obj); return obj; }

        Material MakeMaterial() => Track(new Material(Shader.Find("Shader Graphs/ColorblindCorrectionLUT_HDRP")));

        Texture3D MakeTexture3D() => Track(new Texture3D(1, 1, 1, TextureFormat.RGBA32, false));

        void InvokeRender()
        {
            k_Render.Invoke(m_component, new object[] { null, null, null, null });
        }

        void InvokeRenderSafe()
        {
            try { InvokeRender(); }
            catch (TargetInvocationException) { }
        }

        #endregion

        #region IsActive

        [Test]
        public void IsActive_NullMaterial_ReturnsFalse()
        {
            Assert.IsFalse(m_component.IsActive());
        }

        [Test]
        public void IsActive_WithMaterial_ReturnsTrue()
        {
            m_component.materialParameter.value = MakeMaterial();
            Assert.IsTrue(m_component.IsActive());
        }

        #endregion

        #region Guard Conditions

        [Test]
        public void Render_NullMaterial_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => InvokeRender());
        }

        #endregion

        #region Normal Path (overrideSettings = false, reads from AccessibilitySettings)

        [Test]
        public void NormalPath_Amount_MatchesSettings()
        {
            m_component.materialParameter.value = MakeMaterial();
            m_settings.colorblindCorrectionAmount = 0.75f;

            InvokeRenderSafe();

            Assert.AreEqual(0.75f, m_component.materialParameter.value.GetFloat("_Amount"), 1e-5f);
        }

        [Test]
        [TestCase(Protanopia)]
        [TestCase(Deuteranopia)]
        [TestCase(Tritanopia)]
        [TestCase(None)]
        public void NormalPath_LUT_MatchesSettingsMode(AccessibilitySettings.ColorblindCorrectionMode mode)
        {
            m_component.materialParameter.value = MakeMaterial();
            var texProto = MakeTexture3D();
            var texDeut  = MakeTexture3D();
            var texTrit  = MakeTexture3D();
            m_component.textureProtanopia.value  = texProto;
            m_component.textureDeutranopia.value = texDeut;
            m_component.textureTritanopia.value  = texTrit;
            m_settings.colorblindCorrectionMode = mode;

            InvokeRenderSafe();

            Texture3D expected;
            switch (mode)
            {
                case Protanopia:   expected = texProto; break;
                case Deuteranopia: expected = texDeut;  break;
                case Tritanopia:   expected = texTrit;  break;
                default:           expected = null;     break;
            }
            Assert.AreSame(expected, m_component.materialParameter.value.GetTexture("_LUT"));
        }

        #endregion

        #region Override Path (overrideSettings = true, reads from local VolumeParameters)

        [Test]
        public void OverridePath_Amount_UsesLocalField_NotSettings()
        {
            m_component.materialParameter.value = MakeMaterial();
            m_settings.colorblindCorrectionAmount = 0.25f;
            m_component.amount.value = 0.9f;
            m_component.overrideSettings.value = true;

            InvokeRenderSafe();

            Assert.AreEqual(0.9f, m_component.materialParameter.value.GetFloat("_Amount"), 1e-5f);
        }

        [Test]
        public void OverridePath_LUT_UsesLocalModeTexture_NotSettingsMode()
        {
            m_component.materialParameter.value = MakeMaterial();
            var texProto = MakeTexture3D();
            var texDeut  = MakeTexture3D();
            m_component.textureProtanopia.value  = texProto;
            m_component.textureDeutranopia.value = texDeut;
            m_settings.colorblindCorrectionMode = Protanopia;
            m_component.mode.value = Deuteranopia;
            m_component.overrideSettings.value = true;

            InvokeRenderSafe();

            Assert.AreSame(texDeut, m_component.materialParameter.value.GetTexture("_LUT"));
        }

        #endregion

        #region Override Disabled

        [Test]
        public void OverrideDisabled_LocalFields_DoNotAffectMaterial()
        {
            m_component.materialParameter.value = MakeMaterial();
            m_settings.colorblindCorrectionAmount = 0.5f;
            m_component.amount.value = 0.9f;
            var texProto = MakeTexture3D();
            var texDeut  = MakeTexture3D();
            m_component.textureProtanopia.value  = texProto;
            m_component.textureDeutranopia.value = texDeut;
            m_settings.colorblindCorrectionMode = Protanopia;
            m_component.mode.value = Deuteranopia;
            // overrideSettings = false (default)

            InvokeRenderSafe();

            Assert.AreEqual(0.5f, m_component.materialParameter.value.GetFloat("_Amount"), 1e-5f);
            Assert.AreSame(texProto, m_component.materialParameter.value.GetTexture("_LUT"));
        }

        #endregion
    }
}