using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Sidecar asset that pairs an <see cref="AudioClip"/> with its accessibility
    /// description. Attach one of these to any clip that should generate captions via
    /// <see cref="DescriptiveAudioSource"/>.
    /// </summary>
    [CreateAssetMenu(
        fileName = "DescriptiveAudioClip",
        menuName = "Easy Accessibility/Descriptive Audio Clip"
    )]
    public class DescriptiveAudioClipSO : DescriptiveMediaSO, ITranscribedAsset<AudioClip>
    {
        [SerializeField]
        AudioClip _clip;

        [SerializeField]
        AudioCategory _category;

        public AudioClip Clip => _clip;
        public AudioCategory Category => _category;
        public override float Duration => _clip != null ? _clip.length : 0f;
        public override UnityEngine.Object MediaAsset => _clip;

        // ITranscribedAsset<AudioClip>
        public AudioClip Asset => _clip;
        public TextAsset Transcript => VttTranscript;
    }
}
