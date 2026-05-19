using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Implemented by sidecar ScriptableObjects that pair a typed Unity asset with a
    /// WebVTT transcript. Allows systems to treat audio and video descriptors uniformly.
    /// </summary>
    public interface ITranscribedAsset<T>
        where T : UnityEngine.Object
    {
        T Asset { get; }
        TextAsset Transcript { get; }
    }
}
