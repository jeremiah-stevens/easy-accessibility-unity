using UnityEngine;
using UnityEngine.Rendering.Universal;

/*TODO:
 * - Find some way to hide this from the feature list (make procedural the base?)
 */
namespace EasyAccessibility
{
    public abstract class ColorblindnessRendererFeature : ScriptableRendererFeature
    {
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
        }

        public override void Create()
        {
        }
    }
}