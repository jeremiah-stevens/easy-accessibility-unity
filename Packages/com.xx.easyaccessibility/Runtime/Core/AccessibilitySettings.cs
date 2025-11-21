using UnityEngine;

namespace EasyAccessibility
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Easy Accessibility/Settings")]
    public class AccessibilitySettings : ScriptableObject
    {
        [Header("Colorblind")]
        public ColorblindMode colorblindCorrectionMode;
        [Range(0f, 1f)] public float colorblindCorrectionAmount;


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







        public enum ColorblindMode
        {
            None = 0,
            Protanopia = 1,
            Deuteranopia = 2,
            Tritanopia = 3,
        };
    }
}