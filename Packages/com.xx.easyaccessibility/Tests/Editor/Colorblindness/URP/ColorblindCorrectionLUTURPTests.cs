using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Object;
using static EasyAccessibility.AccessibilitySettings.ColorblindCorrectionMode;

namespace EasyAccessibility.Tests.Colorblind
{
    public class ColorblindCorrectionLUTURPTests
    {
        ColorblindCorrectionLUTURP m_feature;
        AccessibilitySettings m_settings;
        List<UnityEngine.Object> m_cleanup;

        static readonly FieldInfo k_Instance =
            typeof(AccessibilitySettings).GetField("m_instance", BindingFlags.NonPublic | BindingFlags.Static);
        static readonly FieldInfo k_Material =
            typeof(ColorblindCorrectionLUTURP).GetField("material", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_OverrideSettings =
            typeof(ColorblindCorrectionLUTURP).GetField("overrideSettings", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Mode =
            typeof(ColorblindCorrectionLUTURP).GetField("mode", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Amount =
            typeof(ColorblindCorrectionLUTURP).GetField("amount", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_TextureProtanopia =
            typeof(ColorblindCorrectionLUTURP).GetField("textureProtanopia", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_TextureDeutranopia =
            typeof(ColorblindCorrectionLUTURP).GetField("textureDeutranopia", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_TextureTritanopia =
            typeof(ColorblindCorrectionLUTURP).GetField("textureTritanopia", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly MethodInfo k_AddRenderPasses =
            typeof(ColorblindCorrectionLUTURP).GetMethod("AddRenderPasses", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<UnityEngine.Object>();
            m_feature = (ColorblindCorrectionLUTURP)ScriptableObject.CreateInstance(typeof(ColorblindCorrectionLUTURP));
            m_settings = ScriptableObject.CreateInstance<AccessibilitySettings>();
            k_Instance.SetValue(null, m_settings);
            k_Material.SetValue(m_feature, null);
        }

        [TearDown]
        public void TearDown()
        {
            k_Instance.SetValue(null, null);
            DestroyImmediate(m_feature);
            DestroyImmediate(m_settings);
            foreach (var obj in m_cleanup)
                if (obj != null) DestroyImmediate(obj);
        }

        #region Helpers

        T Track<T>(T obj) where T : UnityEngine.Object { m_cleanup.Add(obj); return obj; }

        Material MakeMaterial() => Track(new Material(Shader.Find("Shader Graphs/ColorblindCorrectionLUT_URP")));

        Texture3D MakeTexture3D() => Track(new Texture3D(1, 1, 1, TextureFormat.RGBA32, false));

        void InvokeAddRenderPasses()
        {
            var rdType = k_AddRenderPasses.GetParameters()[1].ParameterType.GetElementType();
            object rd = Activator.CreateInstance(rdType);
            k_AddRenderPasses.Invoke(m_feature, new object[] { null, rd });
        }

        void InvokeAddRenderPassesSafe()
        {
            try { InvokeAddRenderPasses(); }
            catch (TargetInvocationException) { }
        }

        #endregion

        #region Guard Conditions

        [Test]
        public void AddRenderPasses_NullMaterial_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => InvokeAddRenderPasses());
        }

        #endregion

        #region Normal Path (overrideSettings = false, reads from AccessibilitySettings)

        [Test]
        public void NormalPath_Mode_MatchesSettings()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindCorrectionMode = Deuteranopia;

            InvokeAddRenderPassesSafe();

            Assert.AreEqual((int)Deuteranopia, mat.GetInt("_Mode"));
        }

        [Test]
        public void NormalPath_Amount_MatchesSettings()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindCorrectionAmount = 0.75f;

            InvokeAddRenderPassesSafe();

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
            k_Material.SetValue(m_feature, mat);
            var texProto = MakeTexture3D();
            var texDeut  = MakeTexture3D();
            var texTrit  = MakeTexture3D();
            k_TextureProtanopia.SetValue(m_feature, texProto);
            k_TextureDeutranopia.SetValue(m_feature, texDeut);
            k_TextureTritanopia.SetValue(m_feature, texTrit);
            m_settings.colorblindCorrectionMode = mode;

            InvokeAddRenderPassesSafe();

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
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindCorrectionMode = Protanopia;
            k_Mode.SetValue(m_feature, Tritanopia);
            k_OverrideSettings.SetValue(m_feature, true);

            InvokeAddRenderPassesSafe();

            Assert.AreEqual((int)Tritanopia, mat.GetInt("_Mode"));
        }

        [Test]
        public void OverridePath_Amount_UsesLocalField_NotSettings()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindCorrectionAmount = 0.25f;
            k_Amount.SetValue(m_feature, 0.9f);
            k_OverrideSettings.SetValue(m_feature, true);

            InvokeAddRenderPassesSafe();

            Assert.AreEqual(0.9f, mat.GetFloat("_Amount"), 1e-5f);
        }

        [Test]
        public void OverridePath_LUT_UsesLocalModeTexture_NotSettingsMode()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            var texProto = MakeTexture3D();
            var texDeut  = MakeTexture3D();
            k_TextureProtanopia.SetValue(m_feature, texProto);
            k_TextureDeutranopia.SetValue(m_feature, texDeut);
            m_settings.colorblindCorrectionMode = Protanopia;
            k_Mode.SetValue(m_feature, Deuteranopia);
            k_OverrideSettings.SetValue(m_feature, true);

            InvokeAddRenderPassesSafe();

            Assert.AreSame(texDeut, mat.GetTexture("_LUT"));
        }

        #endregion

        #region Override Disabled

        [Test]
        public void OverrideDisabled_LocalFields_DoNotAffectMaterial()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindCorrectionMode = Protanopia;
            m_settings.colorblindCorrectionAmount = 0.5f;
            k_Mode.SetValue(m_feature, Tritanopia);
            k_Amount.SetValue(m_feature, 0.9f);
            // overrideSettings = false (default)

            InvokeAddRenderPassesSafe();

            Assert.AreEqual((int)Protanopia, mat.GetInt("_Mode"));
            Assert.AreEqual(0.5f, mat.GetFloat("_Amount"), 1e-5f);
        }

        #endregion
    }
}