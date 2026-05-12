using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using static EasyAccessibility.AccessibilitySettings.ColorblindCorrectionMode;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.Colorblind
{
    public class ColorblindCorrectionProceduralHDRPTests
    {
        ColorblindCorrectionProceduralHDRP m_component;
        AccessibilitySettings m_settings;
        List<Object> m_cleanup;

        static readonly FieldInfo k_Instance = typeof(AccessibilitySettings).GetField(
            "m_instance",
            BindingFlags.NonPublic | BindingFlags.Static
        );
        static readonly MethodInfo k_Render = typeof(ColorblindCorrectionProceduralHDRP).GetMethod(
            "Render",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<Object>();
            m_component = ScriptableObject.CreateInstance<ColorblindCorrectionProceduralHDRP>();
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
                if (obj != null)
                    DestroyImmediate(obj);
        }

        #region Helpers

        T Track<T>(T obj)
            where T : Object
        {
            m_cleanup.Add(obj);
            return obj;
        }

        Material MakeMaterial() =>
            Track(new Material(Shader.Find("Shader Graphs/ColorblindCorrectionProcedural_HDRP")));

        void InvokeRender()
        {
            k_Render.Invoke(m_component, new object[] { null, null, null, null });
        }

        void InvokeRenderSafe()
        {
            try
            {
                InvokeRender();
            }
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
        public void NormalPath_Keyword_MatchesSettings()
        {
            m_component.materialParameter.value = MakeMaterial();
            m_settings.colorblindCorrectionMode = Deuteranopia;

            InvokeRenderSafe();

            var mat = m_component.materialParameter.value;
            Assert.IsTrue(mat.IsKeywordEnabled("_MODE_DEUTERANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_PROTANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_TRITANOPIA"));
        }

        [Test]
        public void NormalPath_Amount_MatchesSettings()
        {
            m_component.materialParameter.value = MakeMaterial();
            m_settings.colorblindCorrectionAmount = 0.75f;

            InvokeRenderSafe();

            Assert.AreEqual(0.75f, m_component.materialParameter.value.GetFloat("_Amount"), 1e-5f);
        }

        #endregion

        #region Override Path (overrideSettings = true, reads from local VolumeParameters)

        [Test]
        public void OverridePath_Keyword_UsesLocalField_NotSettings()
        {
            m_component.materialParameter.value = MakeMaterial();
            m_settings.colorblindCorrectionMode = Protanopia;
            m_component.mode.value = Tritanopia;
            m_component.overrideSettings.value = true;

            InvokeRenderSafe();

            var mat = m_component.materialParameter.value;
            Assert.IsTrue(mat.IsKeywordEnabled("_MODE_TRITANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_PROTANOPIA"));
        }

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

        #endregion

        #region Override Disabled

        [Test]
        public void OverrideDisabled_LocalFields_DoNotAffectMaterial()
        {
            m_component.materialParameter.value = MakeMaterial();
            m_settings.colorblindCorrectionMode = Protanopia;
            m_settings.colorblindCorrectionAmount = 0.5f;
            m_component.mode.value = Tritanopia;
            m_component.amount.value = 0.9f;
            // overrideSettings = false (default)

            InvokeRenderSafe();

            var mat = m_component.materialParameter.value;
            Assert.IsTrue(mat.IsKeywordEnabled("_MODE_PROTANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_TRITANOPIA"));
            Assert.AreEqual(0.5f, mat.GetFloat("_Amount"), 1e-5f);
        }

        #endregion
    }
}
