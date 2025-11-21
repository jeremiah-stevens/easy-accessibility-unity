#if EA_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

/*TODO:
 * - Set up URP support as conditional on if the package is installed or not
 * - Figure out how we want to handle colorblindness simulation (separate toggle? Don't want devs to use on accident in-game)
 */
namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind correction using a look-up table approach (Universal Render Pipeline).
    /// </summary>
    public class ColorblindnessRendererFeatureLUT : ColorblindnessRendererFeature
    {
        [SerializeField] Material material;

        [Header("Override")]
        [SerializeField] bool overrideSettings;
        [SerializeField] AccessibilitySettings.ColorblindMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;
        [SerializeField] Texture3D textureProtanopia;
        [SerializeField] Texture3D textureDeutranopia;
        [SerializeField] Texture3D textureTritanopia;

        ColorblindnessRenderPassLUT m_pass;




        private void SetLUT(AccessibilitySettings.ColorblindMode mode)
        {
            Texture3D tex = null;
            switch(mode)
            {
                case AccessibilitySettings.ColorblindMode.Protanopia:
                    tex = textureProtanopia;
                    break;
                case AccessibilitySettings.ColorblindMode.Deuteranopia:
                    tex = textureDeutranopia;
                    break;
                case AccessibilitySettings.ColorblindMode.Tritanopia:
                    tex = textureTritanopia;
                    break;
            }
            material.SetTexture("_LUT", tex);
        }




        public override void Create()
        {
            m_pass = new ColorblindnessRenderPassLUT();
            m_pass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (material == null) return;

            SetLUT(AccessibilitySettings.Instance.colorblindCorrectionMode);
            material.SetInt("_Mode", (int)AccessibilitySettings.Instance.colorblindCorrectionMode);
            material.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindCorrectionAmount);

            if(overrideSettings)
            {
                SetLUT(mode);
                material.SetInt("_Mode", (int)mode);
                material.SetFloat("_Amount", amount);
            }

            m_pass.Setup(material);
            renderer.EnqueuePass(m_pass);
        }
    }

    public class ColorblindnessRenderPassLUT : ScriptableRenderPass
    {
        const string m_PassName = "ColorblindnessPass";
        Material m_material;




        public void Setup(Material mat)
        {
            m_material = mat;
            requiresIntermediateTexture = true;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();
            var source = resourceData.activeColorTexture;

            var destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = $"CameraColor-{m_PassName}";
            destinationDesc.clearBuffer = false;
            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);
            RenderGraphUtils.BlitMaterialParameters para = new(source, destination, m_material, 0);
            renderGraph.AddBlitPass(para, passName: m_PassName);
            resourceData.cameraColor = destination;
        }
    }
}
#endif