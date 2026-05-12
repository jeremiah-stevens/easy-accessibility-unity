#if EA_HDRP && UNITY_EDITOR
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
        



        public bool IsActive() => materialParameter.value != null;

        public override void Setup()
        {
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (materialParameter.value == null) return;

            ColorblindMaterialUtils.SetSimulationKeywords(materialParameter.value, AccessibilitySettings.Instance.colorblindSimulationMode);
            materialParameter.value.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindSimulationAmount);

            if(overrideSettings.value)
            {
                ColorblindMaterialUtils.SetSimulationKeywords(materialParameter.value, mode.value);
                materialParameter.value.SetFloat("_Amount", amount.value);
            }

            HDUtils.DrawFullScreen(cmd, materialParameter.value, destination);
        }
    }
}
#endif