#if EA_HDRP
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind correction using the procedural approach (High-Definition Render Pipeline).
    /// </summary>
    [Serializable]
    [VolumeComponentMenu("Easy Accessibility/Colorblind Correction (Procedural)")]
    public class ColorblindCorrectionProceduralHDRP : ColorblindCorrectionHDRP
    {
        public MaterialParameter materialParameter = new MaterialParameter(null);
        public BoolParameter overrideSettings = new BoolParameter(false);
        public VolumeParameter<AccessibilitySettings.ColorblindCorrectionMode> mode = new VolumeParameter<AccessibilitySettings.ColorblindCorrectionMode>();
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
        



        private void SetMode(AccessibilitySettings.ColorblindCorrectionMode mode)
        {
            switch (mode)
            {
                case AccessibilitySettings.ColorblindCorrectionMode.Protanopia:
                    materialParameter.value.EnableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Deuteranopia:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.EnableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Tritanopia:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.EnableKeyword("_MODE_TRITANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.None:
                default:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    break;
            }
        }




        public override bool IsActive() => materialParameter.value != null;

        public override void Setup()
        {
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (materialParameter.value == null) return;

            SetMode(AccessibilitySettings.Instance.colorblindCorrectionMode);
            materialParameter.value.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindCorrectionAmount);

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