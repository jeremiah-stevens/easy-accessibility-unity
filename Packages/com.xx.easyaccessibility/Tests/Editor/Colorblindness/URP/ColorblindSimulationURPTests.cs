using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using static EasyAccessibility.AccessibilitySettings.ColorblindSimulationMode;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.Colorblind
{
    public class ColorblindSimulationURPTests
    {
        ColorblindSimulationURP m_feature;
        AccessibilitySettings m_settings;
        List<UnityEngine.Object> m_cleanup;

        static readonly FieldInfo k_Instance = typeof(AccessibilitySettings).GetField(
            "m_instance",
            BindingFlags.NonPublic | BindingFlags.Static
        );
        static readonly FieldInfo k_Material = typeof(ColorblindSimulationURP).GetField(
            "material",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_OverrideSettings = typeof(ColorblindSimulationURP).GetField(
            "overrideSettings",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Mode = typeof(ColorblindSimulationURP).GetField(
            "mode",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Amount = typeof(ColorblindSimulationURP).GetField(
            "amount",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly MethodInfo k_AddRenderPasses = typeof(ColorblindSimulationURP).GetMethod(
            "AddRenderPasses",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<UnityEngine.Object>();
            m_feature = (ColorblindSimulationURP)
                ScriptableObject.CreateInstance(typeof(ColorblindSimulationURP));
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
                if (obj != null)
                    DestroyImmediate(obj);
        }

        #region Helpers

        T Track<T>(T obj)
            where T : UnityEngine.Object
        {
            m_cleanup.Add(obj);
            return obj;
        }

        Material MakeMaterial() =>
            Track(new Material(Shader.Find("Shader Graphs/ColorblindSimulation_URP")));

        void InvokeAddRenderPasses()
        {
            var rdType = k_AddRenderPasses.GetParameters()[1].ParameterType.GetElementType();
            object rd = Activator.CreateInstance(rdType);
            k_AddRenderPasses.Invoke(m_feature, new object[] { null, rd });
        }

        void InvokeAddRenderPassesSafe()
        {
            try
            {
                InvokeAddRenderPasses();
            }
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
        public void NormalPath_Keyword_MatchesSettings()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindSimulationMode = Deuteranopia;

            InvokeAddRenderPassesSafe();

            Assert.IsTrue(mat.IsKeywordEnabled("_MODE_DEUTERANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_PROTANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_TRITANOPIA"));
        }

        [Test]
        public void NormalPath_Amount_MatchesSettings()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindSimulationAmount = 0.75f;

            InvokeAddRenderPassesSafe();

            Assert.AreEqual(0.75f, mat.GetFloat("_Amount"), 1e-5f);
        }

        #endregion

        #region Override Path (overrideSettings = true, reads from local serialized fields)

        [Test]
        public void OverridePath_Keyword_UsesLocalField_NotSettings()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindSimulationMode = Protanopia;
            k_Mode.SetValue(m_feature, Tritanopia);
            k_OverrideSettings.SetValue(m_feature, true);

            InvokeAddRenderPassesSafe();

            Assert.IsTrue(mat.IsKeywordEnabled("_MODE_TRITANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_PROTANOPIA"));
        }

        [Test]
        public void OverridePath_Amount_UsesLocalField_NotSettings()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindSimulationAmount = 0.25f;
            k_Amount.SetValue(m_feature, 0.9f);
            k_OverrideSettings.SetValue(m_feature, true);

            InvokeAddRenderPassesSafe();

            Assert.AreEqual(0.9f, mat.GetFloat("_Amount"), 1e-5f);
        }

        #endregion

        #region Override Disabled

        [Test]
        public void OverrideDisabled_LocalFields_DoNotAffectMaterial()
        {
            var mat = MakeMaterial();
            k_Material.SetValue(m_feature, mat);
            m_settings.colorblindSimulationMode = Protanopia;
            m_settings.colorblindSimulationAmount = 0.5f;
            k_Mode.SetValue(m_feature, Tritanopia);
            k_Amount.SetValue(m_feature, 0.9f);
            // overrideSettings = false (default)

            InvokeAddRenderPassesSafe();

            Assert.IsTrue(mat.IsKeywordEnabled("_MODE_PROTANOPIA"));
            Assert.IsFalse(mat.IsKeywordEnabled("_MODE_TRITANOPIA"));
            Assert.AreEqual(0.5f, mat.GetFloat("_Amount"), 1e-5f);
        }

        #endregion
    }
}
