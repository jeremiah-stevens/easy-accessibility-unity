using UnityEngine;
using UnityEngine.Video;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Sidecar asset that pairs a <see cref="VideoClip"/> with its accessibility
    /// description.
    /// </summary>
    [CreateAssetMenu(
        fileName = "DescriptiveVideoClip",
        menuName = "Easy Accessibility/Descriptive Video Clip"
    )]
    public class DescriptiveVideoClipSO : DescriptiveMediaSO, ITranscribedAsset<VideoClip>
    {
        [SerializeField]
        VideoClip _clip;

        public VideoClip Clip => _clip;
        public override float Duration => _clip != null ? (float)_clip.length : 0f;
        public override UnityEngine.Object MediaAsset => _clip;

        // ITranscribedAsset<VideoClip>
        public VideoClip Asset => _clip;
        public TextAsset Transcript => VttTranscript;
    }
}
