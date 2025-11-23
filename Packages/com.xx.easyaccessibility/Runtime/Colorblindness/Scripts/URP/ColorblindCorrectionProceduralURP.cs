#if EA_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;


namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind correction using the procedural approach (Universal Render Pipeline).
    /// </summary>
    public class ColorblindCorrectionProceduralURP : ColorblindnessRendererFeature
    {
        [SerializeField]Material material;

        [Header("Override")]
        [SerializeField] bool overrideSettings;
        [SerializeField] AccessibilitySettings.ColorblindCorrectionMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;

        //state
        ColorblindCorrectionRenderPassProcedural m_pass;




        private void SwapMode(AccessibilitySettings.ColorblindCorrectionMode mode)
        {
            switch(mode)
            {
                case AccessibilitySettings.ColorblindCorrectionMode.Protanopia:
                    material.EnableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Deuteranopia:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.EnableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.Tritanopia:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.EnableKeyword("_MODE_TRITANOPIA");
                    break;
                case AccessibilitySettings.ColorblindCorrectionMode.None:
                default:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    break;
            }
        }


        public override void Create()
        {
            m_pass = new ColorblindCorrectionRenderPassProcedural();
            m_pass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (material == null) return;

            SwapMode(AccessibilitySettings.Instance.colorblindCorrectionMode);
            material.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindCorrectionAmount);

            if(overrideSettings)
            {
                SwapMode(mode);
                material.SetFloat("_Amount", amount);
            }

            m_pass.Setup(material);
            renderer.EnqueuePass(m_pass);
        }
    }

    public class ColorblindCorrectionRenderPassProcedural : ScriptableRenderPass
    {
        const string m_PassName = "ColorblindCorrectionPass";
        Material material;




        public void Setup(Material mat)
        {
            material = mat;
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
            RenderGraphUtils.BlitMaterialParameters para = new(source, destination, material, 0);
            renderGraph.AddBlitPass(para, passName: m_PassName);
            resourceData.cameraColor = destination;
        }
    }
}
#endif