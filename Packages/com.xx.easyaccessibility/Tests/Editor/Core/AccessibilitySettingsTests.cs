using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace EasyAccessibility.Tests
{
    public class AccessibilitySettingsTests
    {
        AccessibilitySettings m_settings;
        string m_savePath;
        string m_savedBackup;

        static readonly FieldInfo k_InstanceField =
            typeof(AccessibilitySettings).GetField("m_instance", BindingFlags.NonPublic | BindingFlags.Static);

        [SetUp]
        public void SetUp()
        {
            m_settings = ScriptableObject.CreateInstance<AccessibilitySettings>();
            m_savePath = Path.Combine(Application.persistentDataPath, "accessibility_settings.json");
            m_savedBackup = File.Exists(m_savePath) ? File.ReadAllText(m_savePath) : null;
        }

        [TearDown]
        public void TearDown()
        {
            k_InstanceField.SetValue(null, null);
            Object.DestroyImmediate(m_settings);

            if (m_savedBackup != null)
                File.WriteAllText(m_savePath, m_savedBackup);
            else if (File.Exists(m_savePath))
                File.Delete(m_savePath);
        }

        #region Save/Load

        [Test]
        public void Load_WhenNoFileExists_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => m_settings.Load());
        }

        [Test]
        public void Load_WhenFileIsEmpty_DoesNotThrow()
        {
            File.WriteAllText(m_savePath, string.Empty);
            Assert.DoesNotThrow(() => m_settings.Load());
        }

        [Test]
        public void Load_WhenFileIsCorrupted_DoesNotThrow()
        {
            File.WriteAllText(m_savePath, "{ not valid json {{{{");
            Assert.DoesNotThrow(() => m_settings.Load());
        }

        [Test]
        public void Save_CreatesFileAtPersistentDataPath()
        {
            m_settings.Save();
            Assert.IsTrue(File.Exists(m_savePath));
        }

        [Test]
        [TestCase(AccessibilitySettings.ColorblindCorrectionMode.None)]
        [TestCase(AccessibilitySettings.ColorblindCorrectionMode.Protanopia)]
        [TestCase(AccessibilitySettings.ColorblindCorrectionMode.Deuteranopia)]
        [TestCase(AccessibilitySettings.ColorblindCorrectionMode.Tritanopia)]
        public void SaveLoad_RoundTrips_ColorblindCorrectionMode(AccessibilitySettings.ColorblindCorrectionMode mode)
        {
            m_settings.colorblindCorrectionMode = mode;
            m_settings.Save();
            m_settings.colorblindCorrectionMode = AccessibilitySettings.ColorblindCorrectionMode.None;

            m_settings.Load();

            Assert.AreEqual(mode, m_settings.colorblindCorrectionMode);
        }

        [Test]
        [TestCase(0f)]
        [TestCase(0.5f)]
        [TestCase(1f)]
        public void SaveLoad_RoundTrips_ColorblindCorrectionAmount(float amount)
        {
            m_settings.colorblindCorrectionAmount = amount;
            m_settings.Save();
            m_settings.colorblindCorrectionAmount = 0f;

            m_settings.Load();

            Assert.AreEqual(amount, m_settings.colorblindCorrectionAmount, delta: 0.0001f);
        }

        [Test]
        [TestCase(AccessibilitySettings.ColorblindSimulationMode.None)]
        [TestCase(AccessibilitySettings.ColorblindSimulationMode.Protanopia)]
        [TestCase(AccessibilitySettings.ColorblindSimulationMode.Deuteranopia)]
        [TestCase(AccessibilitySettings.ColorblindSimulationMode.Tritanopia)]
        [TestCase(AccessibilitySettings.ColorblindSimulationMode.Monochromatism)]
        [TestCase(AccessibilitySettings.ColorblindSimulationMode.Achromatopsia)]
        public void SaveLoad_RoundTrips_ColorblindSimulationMode(AccessibilitySettings.ColorblindSimulationMode mode)
        {
            m_settings.colorblindSimulationMode = mode;
            m_settings.Save();
            m_settings.colorblindSimulationMode = AccessibilitySettings.ColorblindSimulationMode.None;

            m_settings.Load();

            Assert.AreEqual(mode, m_settings.colorblindSimulationMode);
        }

        [Test]
        [TestCase(0f)]
        [TestCase(0.5f)]
        [TestCase(1f)]
        public void SaveLoad_RoundTrips_ColorblindSimulationAmount(float amount)
        {
            m_settings.colorblindSimulationAmount = amount;
            m_settings.Save();
            m_settings.colorblindSimulationAmount = 0f;

            m_settings.Load();

            Assert.AreEqual(amount, m_settings.colorblindSimulationAmount, delta: 0.0001f);
        }

        [Test]
        public void SaveLoad_RoundTrips_RebindableKeys()
        {
            const string json = "{\"bindings\":[{\"id\":\"abc\",\"path\":\"<Keyboard>/w\"}]}";
            m_settings.rebindableKeys = json;
            m_settings.Save();
            m_settings.rebindableKeys = string.Empty;

            m_settings.Load();

            Assert.AreEqual(json, m_settings.rebindableKeys);
        }

        #endregion

        #region Singleton

        [Test]
        public void Instance_IsNonNull_AfterFirstAccess()
        {
            k_InstanceField.SetValue(null, m_settings);

            Assert.IsNotNull(AccessibilitySettings.Instance);
        }

        [Test]
        public void Instance_ReturnsSameReference_OnRepeatedCalls()
        {
            k_InstanceField.SetValue(null, m_settings);

            var first = AccessibilitySettings.Instance;
            var second = AccessibilitySettings.Instance;

            Assert.AreSame(first, second);
        }

        #endregion
    }
}
