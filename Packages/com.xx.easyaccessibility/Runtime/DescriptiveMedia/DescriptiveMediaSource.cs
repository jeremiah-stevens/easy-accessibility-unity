using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Abstract base for wrapper components that notify <see cref="DescriptiveMediaManager"/>
    /// when a media asset starts or stops playing. Derive from this for audio
    /// (<see cref="DescriptiveAudioSource"/>) or video sources.
    /// </summary>
    public abstract class DescriptiveMediaSource : MonoBehaviour
    {
        /// <summary>The sidecar descriptor for the currently assigned media asset.</summary>
        public abstract DescriptiveMediaSO DescriptiveMedia { get; }

        /// <summary>Whether the underlying media asset is currently playing.</summary>
        public abstract bool IsPlaying { get; }

        /// <summary>Current playback position in seconds.</summary>
        public abstract float MediaTime { get; }
    }
}