namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Determines how <see cref="DescriptiveMediaSO"/> resolves its description text.
    /// </summary>
    public enum DescriptionSourceType
    {
        /// <summary>A plain string authored directly on the asset.</summary>
        String,

        /// <summary>
        /// A key into a Unity Localization string table. Requires the
        /// com.unity.localization package.
        /// </summary>
        LocalizationKey,

        /// <summary>
        /// A WebVTT file whose timed cues are sequenced by
        /// <see cref="DescriptiveMediaManager"/> against the source's playback time.
        /// </summary>
        VTT,
    }
}