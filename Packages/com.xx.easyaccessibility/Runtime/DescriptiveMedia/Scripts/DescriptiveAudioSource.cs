using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Wrapper around <see cref="AudioSource"/> that notifies
    /// <see cref="DescriptiveMediaManager"/> when a clip starts or stops, enabling
    /// caption generation. Assign a <see cref="DescriptiveAudioClipSO"/> and call
    /// <see cref="Play"/> instead of calling <c>AudioSource.Play</c> directly.
    /// </summary>
    public class DescriptiveAudioSource : DescriptiveMediaSource
    {
        [SerializeField]
        AudioSource _audioSource;

        [SerializeField]
        DescriptiveAudioClipSO _descriptiveClip;

        public override DescriptiveMediaSO DescriptiveMedia => _descriptiveClip;
        public override bool IsPlaying => _audioSource != null && _audioSource.isPlaying;
        public override float MediaTime => _audioSource != null ? _audioSource.time : 0f;

        /// <summary>Typed convenience accessor (avoids casting <see cref="DescriptiveMedia"/>).</summary>
        public DescriptiveAudioClipSO DescriptiveClip => _descriptiveClip;

        /// <summary>
        /// Assigns the clip from <see cref="DescriptiveAudioClipSO"/> to the
        /// <see cref="AudioSource"/> and plays it, registering with
        /// <see cref="DescriptiveMediaManager"/> so a caption is generated.
        /// </summary>
        public void Play()
        {
            if (!ValidateReferences())
                return;
            _audioSource.clip = _descriptiveClip.Clip;
            _audioSource.Play();
            DescriptiveMediaManager.Instance.RegisterPlay(this);
        }

        /// <summary>
        /// Stops the <see cref="AudioSource"/> and removes the caption from
        /// <see cref="DescriptiveMediaManager"/>.
        /// </summary>
        public void Stop()
        {
            if (_audioSource == null)
                return;
            _audioSource.Stop();
            if (DescriptiveMediaManager.IsInitialized)
                DescriptiveMediaManager.Instance.RegisterStop(this);
        }

        /// <summary>
        /// Plays the clip as a one-shot, allowing it to overlap with the current clip.
        /// The manager cleans up the caption automatically once
        /// <see cref="AudioClip.length"/> has elapsed.
        /// </summary>
        public void PlayOneShot()
        {
            if (!ValidateReferences())
                return;
            _audioSource.PlayOneShot(_descriptiveClip.Clip);
            DescriptiveMediaManager.Instance.RegisterPlayOneShot(this);
        }

        // Set when playOnAwake is intercepted; Play() is deferred to Start().
        private bool _pendingAutoPlay;

        private void Awake()
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if (_audioSource != null && _audioSource.playOnAwake)
            {
                _audioSource.playOnAwake = false;
                _pendingAutoPlay = true;
            }
        }

        private void Start()
        {
            if (_pendingAutoPlay)
                Play();
        }

        private void OnDestroy()
        {
            if (DescriptiveMediaManager.IsInitialized)
                DescriptiveMediaManager.Instance.RegisterStop(this);
        }

        private bool ValidateReferences()
        {
            if (_audioSource == null)
            {
                Debug.LogWarning(
                    "DescriptiveAudioSource: no AudioSource found. Add one to this GameObject or assign it in the inspector.",
                    this
                );
                return false;
            }
            if (_descriptiveClip == null)
            {
                Debug.LogWarning(
                    "DescriptiveAudioSource: no DescriptiveAudioClipSO assigned.",
                    this
                );
                return false;
            }
            return true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_audioSource == null && GetComponent<AudioSource>() == null)
                Debug.LogWarning(
                    "DescriptiveAudioSource: no AudioSource on this GameObject.",
                    this
                );
            if (_descriptiveClip == null)
                Debug.LogWarning(
                    "DescriptiveAudioSource: no DescriptiveAudioClipSO assigned.",
                    this
                );
        }
#endif
    }
}