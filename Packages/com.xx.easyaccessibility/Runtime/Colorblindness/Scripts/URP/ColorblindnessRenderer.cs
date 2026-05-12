#if EA_URP
using UnityEngine;
using UnityEngine.Rendering.Universal;

/*TODO:
 * - Find some way to hide this from the feature list (make procedural the base?)
 */
namespace EasyAccessibility
{
    /// <summary>
    /// Base class for colorblind-related renderer features (Universal Render Pipeline).
    /// </summary>
    public abstract class ColorblindnessRendererFeature : ScriptableRendererFeature
    {
        public override void AddRenderPasses(
            ScriptableRenderer renderer,
            ref RenderingData renderingData
        ) { }

        public override void Create() { }
    }
}
#endif
