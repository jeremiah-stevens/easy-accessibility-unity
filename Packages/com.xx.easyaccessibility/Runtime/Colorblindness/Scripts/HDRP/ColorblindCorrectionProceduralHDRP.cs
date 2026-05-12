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
        



        public override bool IsActive() => materialParameter.value != null;

        public override void Setup()
        {
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (materialParameter.value == null) return;

            ColorblindMaterialUtils.SetCorrectionKeywords(materialParameter.value, AccessibilitySettings.Instance.colorblindCorrectionMode);
            materialParameter.value.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindCorrectionAmount);

            if(overrideSettings.value)
            {
                ColorblindMaterialUtils.SetCorrectionKeywords(materialParameter.value, mode.value);
                materialParameter.value.SetFloat("_Amount", amount.value);
            }

            HDUtils.DrawFullScreen(cmd, materialParameter.value, destination);
        }
    }
}
#endif