using System;
using System.IO;
using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// Class for maintaining the accessibility settings across Easy Accessibility.
    /// </summary>
    [CreateAssetMenu(fileName = "Settings", menuName = "Easy Accessibility/Settings")]
    public class AccessibilitySettings : ScriptableObject
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "accessibility_settings.json");

        /// <summary>
        /// What type of colorblind correction to apply.
        /// </summary>
        [Header("Colorblind")]
        public ColorblindCorrectionMode colorblindCorrectionMode;
        /// <summary>
        /// The intensity of colorblind correction to apply.
        /// </summary>
        [Range(0f, 1f)] public float colorblindCorrectionAmount;
#if UNITY_EDITOR //TODO: come up with a better way of warning the user against using colorblind simulation as their correction method
        /// <summary>
        /// What type of colorblind simulation to apply.
        /// </summary>
        /// <remarks>Should only be used for development purposes.</remarks>
        public ColorblindSimulationMode colorblindSimulationMode;
        /// <summary>
        /// The intensity of colorblind simulation to apply.
        /// </summary>
        /// <remarks>Should only be used for development purposes.</remarks>
        [Range(0f, 1f)] public float colorblindSimulationAmount;
#endif

        /// <summary>
        /// JSON representation of keybind overrides.
        /// </summary>
        [Header("Rebindable Keys")]
        public string rebindableKeys;


        private static AccessibilitySettings m_instance;
        /// <summary>
        /// Active instance of settings. Loads from file storage.
        /// </summary>
        public static AccessibilitySettings Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = Resources.Load<AccessibilitySettings>("EasyAccessibility/AccessibilitySettings");
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        public void Save()
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(this));
        }

        /// <summary>
        /// Loads the settings from file.
        /// </summary>
        public void Load()
        {
            if (!File.Exists(SavePath)) return;
            try { JsonUtility.FromJsonOverwrite(File.ReadAllText(SavePath), this); }
            catch (Exception e) { Debug.LogWarning($"[EasyAccessibility] Failed to load settings: {e.Message}"); }
        }






        /// <summary>
        /// Available modes for colorblind correction.
        /// </summary>
        public enum ColorblindCorrectionMode
        {
            None = 0,
            Protanopia = 1,
            Deuteranopia = 2,
            Tritanopia = 3,
        };

        /// <summary>
        /// Available modes for simulating colorblindness.
        /// </summary>
        /// <remarks>Should only be used for developer purposes.</remarks>
        public enum ColorblindSimulationMode
        {
            None = 0,
            Protanopia = 1,
            Deuteranopia = 2,
            Tritanopia = 3,
            Monochromatism = 4,
            Achromatopsia = 5,
        }
    }
}