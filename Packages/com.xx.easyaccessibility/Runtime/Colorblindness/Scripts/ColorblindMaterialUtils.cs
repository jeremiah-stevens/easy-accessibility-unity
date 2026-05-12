using UnityEngine;

namespace EasyAccessibility
{
    public static class ColorblindMaterialUtils
    {
        public static void SetCorrectionKeywords(
            Material m,
            AccessibilitySettings.ColorblindCorrectionMode mode
        )
        {
            m.DisableKeyword("_MODE_PROTANOPIA");
            m.DisableKeyword("_MODE_DEUTERANOPIA");
            m.DisableKeyword("_MODE_TRITANOPIA");
            switch (mode)
            {
                case AccessibilitySettings.ColorblindCorrectionMode.Protanopia:
                    m.EnableKeyword("_MODE_PROTANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Deuteranopia:
                    m.EnableKeyword("_MODE_DEUTERANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Tritanopia:
                    m.EnableKeyword("_MODE_TRITANOPIA");
                    break;
            }
        }

        public static void SetSimulationKeywords(
            Material m,
            AccessibilitySettings.ColorblindSimulationMode mode
        )
        {
            m.DisableKeyword("_MODE_PROTANOPIA");
            m.DisableKeyword("_MODE_DEUTERANOPIA");
            m.DisableKeyword("_MODE_TRITANOPIA");
            m.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
            m.DisableKeyword("_MODE_ACHROMATOPSIA");
            switch (mode)
            {
                case AccessibilitySettings.ColorblindSimulationMode.Protanopia:
                    m.EnableKeyword("_MODE_PROTANOPIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Deuteranopia:
                    m.EnableKeyword("_MODE_DEUTERANOPIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Tritanopia:
                    m.EnableKeyword("_MODE_TRITANOPIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Monochromatism:
                    m.EnableKeyword("_MODE_CONE_MONOCHROMATISM");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Achromatopsia:
                    m.EnableKeyword("_MODE_ACHROMATOPSIA");
                    break;
            }
        }

        public static void SetLUT(
            Material m,
            AccessibilitySettings.ColorblindCorrectionMode mode,
            Texture proto,
            Texture deut,
            Texture trit
        )
        {
            Texture tex = null;
            switch (mode)
            {
                case AccessibilitySettings.ColorblindCorrectionMode.Protanopia:
                    tex = proto;
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Deuteranopia:
                    tex = deut;
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Tritanopia:
                    tex = trit;
                    break;
            }
            m.SetTexture("_LUT", tex);
        }
    }
}
