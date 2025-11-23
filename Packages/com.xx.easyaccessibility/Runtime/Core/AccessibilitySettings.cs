using UnityEngine;

namespace EasyAccessibility
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Easy Accessibility/Settings")]
    public class AccessibilitySettings : ScriptableObject
    {
        [Header("Colorblind")]
        public ColorblindCorrectionMode colorblindCorrectionMode;
        [Range(0f, 1f)] public float colorblindCorrectionAmount;
#if UNITY_EDITOR //TODO: come up with a better way of warning the user against using colorblind simulation as their correction method
        public ColorblindSimulationMode colorblindSimulationMode;
        [Range(0f, 1f)] public float colorblindSimulationAmount;
#endif


        private static AccessibilitySettings m_instance;
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
        /// Available modes for simulating colorblindness. SHOULD ONLY BE USED FOR TESTING.
        /// </summary>
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