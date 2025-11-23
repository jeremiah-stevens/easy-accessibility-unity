#if EA_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;


namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind simulation using the procedural approach (Universal Render Pipeline).
    /// NOTE: should only be used for testing purposes. Use colorblind correction for accessibility
    ///       support.
    /// </summary>
    public class ColorblindSimulationURP : ColorblindnessRendererFeature
    {
        [SerializeField]Material material;

        [Header("Override")]
        [SerializeField] bool overrideSettings;
        [SerializeField] AccessibilitySettings.ColorblindSimulationMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;

        //state
        ColorblindSimulationRenderPassProcedural m_pass;




        private void SwapMode(AccessibilitySettings.ColorblindSimulationMode mode)
        {
            switch(mode)
            {
                case AccessibilitySettings.ColorblindSimulationMode.Protanopia:
                    material.EnableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    material.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    material.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Deuteranopia:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.EnableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    material.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    material.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Tritanopia:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.EnableKeyword("_MODE_TRITANOPIA");
                    material.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    material.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Monochromatism:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    material.EnableKeyword("_MODE_CONE_MONOCHROMATISM");
                    material.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.Achromatopsia:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    material.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    material.EnableKeyword("_MODE_ACHROMATOPSIA");
                    break;
                case AccessibilitySettings.ColorblindSimulationMode.None:
                default:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    material.DisableKeyword("_MODE_CONE_MONOCHROMATISM");
                    material.DisableKeyword("_MODE_ACHROMATOPSIA");
                    break;
            }
        }


        public override void Create()
        {
            m_pass = new ColorblindSimulationRenderPassProcedural();
            m_pass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (material == null) return;

            SwapMode(AccessibilitySettings.Instance.colorblindSimulationMode);
            material.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindSimulationAmount);

            if(overrideSettings)
            {
                SwapMode(mode);
                material.SetFloat("_Amount", amount);
            }

            m_pass.Setup(material);
            renderer.EnqueuePass(m_pass);
        }
    }

    public class ColorblindSimulationRenderPassProcedural : ScriptableRenderPass
    {
        const string m_PassName = "ColorblindSimulationPass";
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