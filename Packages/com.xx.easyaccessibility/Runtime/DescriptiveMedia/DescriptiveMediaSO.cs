#if UNITY_LOCALIZATION
using UnityEngine.Localization.Settings;
#endif
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Abstract base for sidecar ScriptableObjects that pair a time-based media asset
    /// with an accessibility description. Derive from this for audio, video, or any
    /// other media type. Concrete types should also implement
    /// <see cref="ITranscribedAsset{T}"/> for the specific asset type.
    /// </summary>
    public abstract class DescriptiveMediaSO : ScriptableObject
    {
        [SerializeField]
        int _priority;

        [SerializeField]
        DescriptionSourceType _sourceType;

        // Shown in inspector when _sourceType == String
        [SerializeField]
        string _description;

        // Shown in inspector when _sourceType == LocalizationKey
        [SerializeField]
        string _localizationTable;

        [SerializeField]
        string _localizationKey;

        // Shown in inspector when _sourceType == VTT
        [SerializeField]
        TextAsset _vttTranscript;

        public int Priority => _priority;
        public DescriptionSourceType SourceType => _sourceType;
        public TextAsset VttTranscript => _vttTranscript;

        /// <summary>
        /// The duration of the media asset in seconds. Used by
        /// <see cref="DescriptiveMediaManager"/> to expire one-shot captions.
        /// </summary>
        public abstract float Duration { get; }

        /// <summary>
        /// The underlying media asset. Used by the auditor to match descriptors against
        /// project assets without needing to know the concrete type.
        /// </summary>
        public abstract UnityEngine.Object MediaAsset { get; }

        /// <summary>
        /// Resolves the description text based on <see cref="SourceType"/>. Returns an
        /// empty string for <see cref="DescriptionSourceType.VTT"/> sources since cue
        /// sequencing is handled by <see cref="DescriptiveMediaManager"/>.
        /// </summary>
        public string GetDescription()
        {
            return _sourceType switch
            {
                DescriptionSourceType.String => _description,
#if UNITY_LOCALIZATION
                DescriptionSourceType.LocalizationKey => LocalizationSettings
                    .StringDatabase
                    .GetLocalizedString(_localizationTable, _localizationKey),
#endif
                DescriptionSourceType.VTT => string.Empty,
                _ => _description,
            };
        }
    }
}
