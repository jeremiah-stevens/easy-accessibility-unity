#if EA_HDRP
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind simulation using the procedural approach (High-Definition Render Pipeline).
    /// NOTE: should only be used for testing purposes. Use colorblind correction for accessibility
    ///       support.
    /// </summary>
    [Serializable]
    [VolumeComponentMenu("Easy Accessibility/Colorblind Simulation")]
    public class ColorblindSimulationHDRP : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public MaterialParameter materialParameter = new MaterialParameter(null);
        public BoolParameter overrideSettings = new BoolParameter(false);
        public VolumeParameter<AccessibilitySettings.ColorblindSimulationMode> mode = new VolumeParameter<AccessibilitySettings.ColorblindSimulationMode>();
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
        



        private void SetMode(AccessibilitySettings.ColorblindSimulationMode mode)
        {
            switch (mode)
            {
                case AccessibilitySettings.ColorblindSimulationMode.Protanopia:
                    materialParameter.value.EnableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    materialParameter.value.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Deuteranopia:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.EnableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    materialParameter.value.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Tritanopia:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.EnableKeyword("_MODE_TRITANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    materialParameter.value.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Monochromatism:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    materialParameter.value.EnableKeyword("_MODE_CONE_MONOCHROMATISM");
                    materialParameter.value.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Achromatopsia:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    materialParameter.value.EnableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.None:
                default:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    materialParameter.value.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
            }
        }




        public bool IsActive() => materialParameter.value != null;

        public override void Setup()
        {
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (materialParameter.value == null) return;

            SetMode(AccessibilitySettings.Instance.colorblindSimulationMode);
            materialParameter.value.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindSimulationAmount);

            if(overrideSettings.value)
            {
                SetMode(mode.value);
                materialParameter.value.SetFloat("_Amount", amount.value);
            }


            HDUtils.DrawFullScreen(cmd, materialParameter.value, destination);
        }
    }
}
#endif