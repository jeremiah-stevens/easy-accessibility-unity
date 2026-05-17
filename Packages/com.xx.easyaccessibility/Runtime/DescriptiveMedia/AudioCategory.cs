using System;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Classifies the role of an audio clip. Used by <see cref="DescriptiveAudioClipSO"/> and
    /// by accessibility settings to filter which categories of audio generate captions.
    /// </summary>
    [Flags]
    public enum AudioCategory
    {
        None = 0,
        Ambient = 1,
        Dialogue = 2,
        SFX = 4,
        Music = 8,
        All = Ambient | Dialogue | SFX | Music,
    }
}