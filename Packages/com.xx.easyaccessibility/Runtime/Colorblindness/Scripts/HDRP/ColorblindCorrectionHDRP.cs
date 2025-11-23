#if EA_HDRP
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

//source: Unity HDRP Fullscreen Sample
namespace EasyAccessibility
{
    public abstract class ColorblindCorrectionHDRP : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public virtual bool IsActive() { return true; }
    }
}
#endif