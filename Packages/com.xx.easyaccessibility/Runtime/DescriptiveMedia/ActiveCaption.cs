using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Represents a single active descriptive media caption being tracked by
    /// <see cref="DescriptiveMediaManager"/>.
    /// </summary>
    public class ActiveCaption
    {
        /// <summary>
        /// The current caption text. For VTT sources this advances as cues change;
        /// for string and localization sources it is set once on play.
        /// </summary>
        public string Text { get; set; }

        /// <summary>The source that produced this caption.</summary>
        public DescriptiveMediaSource Source { get; set; }

        /// <summary>
        /// Live world-space position of the media source. Null if the source has been
        /// destroyed. Spatial UI components should read this each frame rather than
        /// caching it.
        /// </summary>
        public Vector3? WorldPosition => Source != null ? Source.transform.position : null;
    }
}
