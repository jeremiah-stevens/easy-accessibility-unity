using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;


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
            material.SetFloat("_Amount", amount);
            m_pass.Setup(material);
            renderer.EnqueuePass(m_pass);
        }
    }

    public class ColorblindnessRenderPassProcedural : ScriptableRenderPass
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