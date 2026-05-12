#if EA_HDRP
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

//source: Unity HDRP Fullscreen Sample
namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind correction using a look-up table approach (High-Definition Render Pipeline).
    /// </summary>
    [Serializable]
    [VolumeComponentMenu("Easy Accessibility/Colorblind Correction (LUT)")]
    public class ColorblindCorrectionLUTHDRP : ColorblindCorrectionHDRP
    {
        public MaterialParameter materialParameter = new MaterialParameter(null);

        public BoolParameter overrideSettings = new BoolParameter(false);
        public VolumeParameter<AccessibilitySettings.ColorblindCorrectionMode> mode = new VolumeParameter<AccessibilitySettings.ColorblindCorrectionMode>();
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
        public TextureParameter textureProtanopia = new TextureParameter(null, TextureDimension.Tex3D);
        public TextureParameter textureDeutranopia = new TextureParameter(null, TextureDimension.Tex3D);
        public TextureParameter textureTritanopia = new TextureParameter(null, TextureDimension.Tex3D);




        public override bool IsActive() => materialParameter.value != null;

        public override void Setup()
        {
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (materialParameter.value == null) return;

            var activeMode = AccessibilitySettings.Instance.colorblindCorrectionMode;
            ColorblindMaterialUtils.SetLUT(materialParameter.value, activeMode, textureProtanopia.value, textureDeutranopia.value, textureTritanopia.value);
            materialParameter.value.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindCorrectionAmount);

            if(overrideSettings.value)
            {
                ColorblindMaterialUtils.SetLUT(materialParameter.value, mode.value, textureProtanopia.value, textureDeutranopia.value, textureTritanopia.value);
                materialParameter.value.SetFloat("_Amount", amount.value);
            }

            HDUtils.DrawFullScreen(cmd, materialParameter.value, destination);
        }
    }
}
#endif