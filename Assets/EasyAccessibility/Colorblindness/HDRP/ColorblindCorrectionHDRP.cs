using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EasyAccessibility
{
    [Serializable]
    [VolumeComponentMenu("Easy Accessibility/Colorblind Correction (LUT)")]
    //source: Unity HDRP Fullscreen Sample
    public class ColorblindCorrectionLUT : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public VolumeParameter<ColorblindSettings.ColorblindMode> mode = new VolumeParameter<ColorblindSettings.ColorblindMode>();
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
        public TextureParameter textureProtanopia = new TextureParameter(null, TextureDimension.Tex3D);
        public TextureParameter textureDeutranopia = new TextureParameter(null, TextureDimension.Tex3D);
        public TextureParameter textureTritanopia = new TextureParameter(null, TextureDimension.Tex3D);
        public MaterialParameter materialParameter = new MaterialParameter(null);





        public bool IsActive() => materialParameter.value != null && amount.value > 0f;

        public override void Setup()
        {                
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (materialParameter.value == null) return;

            //materialParameter.value.SetInt("_Mode", (int)mode.value);
            materialParameter.value.SetFloat("_Amount", amount.value);
            Texture tex = null;
            switch (mode.value)
            {
                case ColorblindSettings.ColorblindMode.Protanopia:
                    tex = textureProtanopia.value;
                    break;
                case ColorblindSettings.ColorblindMode.Deuteranopia:
                    tex = textureDeutranopia.value;
                    break;
                default:
                case ColorblindSettings.ColorblindMode.Tritanopia:
                    tex = textureTritanopia.value;
                    break;
            }
            materialParameter.value.SetTexture("_LUT", tex);
            HDUtils.DrawFullScreen(cmd, materialParameter.value, destination);
        }
    }

    [Serializable]
    [VolumeComponentMenu("Easy Accessibility/Colorblind Correction (Procedural)")]
    public class ColorblindCorrectionProcedural : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public VolumeParameter<ColorblindSettings.ColorblindMode> mode = new VolumeParameter<ColorblindSettings.ColorblindMode>();
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
        public MaterialParameter materialParameter = new MaterialParameter(null);





        public bool IsActive() => materialParameter.value != null && amount.value > 0f;

        public override void Setup()
        {
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (materialParameter.value == null) return;

            switch (mode.value)
            {
                case ColorblindSettings.ColorblindMode.Protanopia:
                    materialParameter.value.EnableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case ColorblindSettings.ColorblindMode.Deuteranopia:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.EnableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case ColorblindSettings.ColorblindMode.Tritanopia:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.EnableKeyword("_MODE_TRITANOPIA");
                    break;
                case ColorblindSettings.ColorblindMode.None:
                default:
                    materialParameter.value.DisableKeyword("_MODE_PROTANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_DEUTERANOPIA");
                    materialParameter.value.DisableKeyword("_MODE_TRITANOPIA");
                    break;
            }

            materialParameter.value.SetFloat("Amount", amount.value);
            HDUtils.DrawFullScreen(cmd, materialParameter.value, destination);
        }
    }
}