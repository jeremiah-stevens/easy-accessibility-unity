#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Object;
using static EasyAccessibility.AccessibilitySettings.ColorblindSimulationMode;

namespace EasyAccessibility.Tests.Colorblind
{
    public class ColorblindSimulationBuiltInTests
    {
        GameObject m_go;
        ColorblindSimulationBuiltIn m_component;
        AccessibilitySettings m_settings;
        List<Object> m_cleanup;

        static readonly FieldInfo k_Instance = typeof(AccessibilitySettings).GetField(
            "m_instance",
            BindingFlags.NonPublic | BindingFlags.Static
        );
        static readonly FieldInfo k_RenderMaterial = typeof(ColorblindSimulationBuiltIn).GetField(
            "m_renderMaterial",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_OverrideSettings = typeof(ColorblindSimulationBuiltIn).GetField(
            "overrideSettings",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Mode = typeof(ColorblindSimulationBuiltIn).GetField(
            "mode",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Amount = typeof(ColorblindSimulationBuiltIn).GetField(
            "amount",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly MethodInfo k_OnRenderImage = typeof(ColorblindSimulationBuiltIn).GetMethod(
            "OnRenderImage",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        [SetUp]
        public void SetUp()
        {
            m_cleanup = new List<Object>();
            m_go = new GameObject();
            m_component = m_go.AddComponent<ColorblindSimulationBuiltIn>();
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
            Track(new Material(Shader.Find("EasyAccessibility/BuiltIn/ColorblindSimulation")));

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
            m_settings.colorblindSimulationMode = Deuteranopia;

            InvokeOnRenderImage();

            Assert.AreEqual((int)Deuteranopia, mat.GetInt("_Mode"));
        }

        [Test]
        public void NormalPath_Amount_MatchesSettings()
        {
            var mat = MakeMaterial();
            k_RenderMaterial.SetValue(m_component, mat);
            m_settings.colorblindSimulationAmount = 0.75f;

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
            m_settings.colorblindSimulationMode = Protanopia;
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
            m_settings.colorblindSimulationAmount = 0.25f;
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
            m_settings.colorblindSimulationMode = Protanopia;
            m_settings.colorblindSimulationAmount = 0.5f;
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
#endif
