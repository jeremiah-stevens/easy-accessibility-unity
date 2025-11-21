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
    public class ColorblindCorrectionLUTHDRP : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public MaterialParameter materialParameter = new MaterialParameter(null);

        public BoolParameter overrideSettings = new BoolParameter(false);
        public VolumeParameter<AccessibilitySettings.ColorblindMode> mode = new VolumeParameter<AccessibilitySettings.ColorblindMode>();
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
        public TextureParameter textureProtanopia = new TextureParameter(null, TextureDimension.Tex3D);
        public TextureParameter textureDeutranopia = new TextureParameter(null, TextureDimension.Tex3D);
        public TextureParameter textureTritanopia = new TextureParameter(null, TextureDimension.Tex3D);




        public bool IsActive() => materialParameter.value != null && amount.value > 0f;

        private void SetMode(AccessibilitySettings.ColorblindMode mode)
        {
            Texture tex = null;
            switch (mode)
            {
                case AccessibilitySettings.ColorblindMode.Protanopia:
                    tex = textureProtanopia.value;
                    break;
                case AccessibilitySettings.ColorblindMode.Deuteranopia:
                    tex = textureDeutranopia.value;
                    break;
                case AccessibilitySettings.ColorblindMode.Tritanopia:
                    tex = textureTritanopia.value;
                    break;
                default:
                    tex = null;
                    break;
            }
            materialParameter.value.SetTexture("_LUT", tex);
        }




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