using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;
using static Colorblindness;


namespace EasyAccessibility
{
    public class ColorblindnessRendererFeatureProcedural : ColorblindnessRendererFeature
    {
        [SerializeField] Material material;
        [SerializeField] ColorblindSettings.ColorblindMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;

        ColorblindnessRenderPassProcedural m_pass;







        public override void Create()
        {
            m_pass = new ColorblindnessRenderPassProcedural();
            m_pass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (material == null)
            {
                Debug.LogWarning(this.name + " material is null and will be skipped.");
                return;
            }

            material.SetInt("_Mode", (int)mode);

            switch(mode)
            {
                case ColorblindSettings.ColorblindMode.Protanopia:
                    material.EnableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case ColorblindSettings.ColorblindMode.Deuteranopia:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.EnableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    break;
                case ColorblindSettings.ColorblindMode.Tritanopia:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.EnableKeyword("_MODE_TRITANOPIA");
                    break;
                case ColorblindSettings.ColorblindMode.None:
                default:
                    material.DisableKeyword("_MODE_PROTANOPIA");
                    material.DisableKeyword("_MODE_DEUTERANOPIA");
                    material.DisableKeyword("_MODE_TRITANOPIA");
                    break;
            }

            material.SetFloat("_Amount", amount);
            m_pass.Setup(material);
            renderer.EnqueuePass(m_pass);
        }
    }

    public class ColorblindnessRenderPassProcedural : ScriptableRenderPass
    {
        const string m_PassName = "ColorblindnessPass";
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