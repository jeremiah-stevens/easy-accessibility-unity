using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Manager for descriptive media captions. Tracks active media sources and exposes
    /// caption state to UI layers via <see cref="OnCaptionsChanged"/>.
    /// </summary>
    public class DescriptiveMediaManager : MonoBehaviour
    {
        private static DescriptiveMediaManager m_instance;

        public static DescriptiveMediaManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    var go = new GameObject("DescriptiveMediaManager");
                    m_instance = go.AddComponent<DescriptiveMediaManager>();
                }
                return m_instance;
            }
        }

        public static bool IsInitialized => m_instance != null;

        /// <summary>
        /// Fired whenever the set of active captions changes (a clip started, stopped, or
        /// advanced to a new VTT cue). UI components should subscribe here.
        /// </summary>
        public event Action<IReadOnlyList<ActiveCaption>> OnCaptionsChanged;

        private readonly List<ActiveCaption> m_activeCaptions = new();
        private readonly Dictionary<DescriptiveMediaSource, Coroutine> m_vttCoroutines = new();

#if UNITY_EDITOR
        [Serializable]
        private struct ActiveCaptionDebugInfo
        {
            public string sourceName;
            public string text;
        }

        [SerializeField]
        private List<ActiveCaptionDebugInfo> _debugCaptions = new();
#endif

        private void NotifyCaptionsChanged()
        {
#if UNITY_EDITOR
            _debugCaptions.Clear();
            foreach (var c in m_activeCaptions)
                _debugCaptions.Add(
                    new ActiveCaptionDebugInfo
                    {
                        sourceName = c.Source != null ? c.Source.gameObject.name : "(destroyed)",
                        text = c.Text,
                    }
                );
#endif
            OnCaptionsChanged?.Invoke(new ReadOnlyCollection<ActiveCaption>(m_activeCaptions));
        }

        /// <summary>
        /// Registers a clip play from a <see cref="DescriptiveMediaSource"/> and adds it to
        /// the active caption list if it passes the current category and priority filters.
        /// </summary>
        /// <param name="source">The source that began playing.</param>
        public void RegisterPlay(DescriptiveMediaSource source)
        {
            if (source == null || source.DescriptiveMedia == null)
                return;
            if (!PassesFilter(source.DescriptiveMedia))
                return;

            var caption = new ActiveCaption
            {
                Source = source,
                Text = source.DescriptiveMedia.GetDescription(),
            };

            m_activeCaptions.Add(caption);

            if (source.DescriptiveMedia.SourceType == DescriptionSourceType.VTT)
                m_vttCoroutines[source] = StartCoroutine(TickVttCaption(source, caption));

            NotifyCaptionsChanged();
        }

        /// <summary>
        /// Registers a one-shot clip play. The caption is removed automatically once the
        /// clip's duration has elapsed, since one-shots cannot be stopped individually.
        /// </summary>
        /// <param name="source">The source that fired the one-shot.</param>
        public void RegisterPlayOneShot(DescriptiveMediaSource source)
        {
            if (source == null || source.DescriptiveMedia == null)
                return;
            if (!PassesFilter(source.DescriptiveMedia))
                return;

            var caption = new ActiveCaption
            {
                Source = source,
                Text = source.DescriptiveMedia.GetDescription(),
            };

            m_activeCaptions.Add(caption);
            StartCoroutine(ExpireOneShotCaption(caption, source.DescriptiveMedia.Duration));
            NotifyCaptionsChanged();
        }

        /// <summary>
        /// Removes a source from the active caption list. Call this when the media stops
        /// or is interrupted.
        /// </summary>
        /// <param name="source">The source that stopped playing.</param>
        public void RegisterStop(DescriptiveMediaSource source)
        {
            if (source == null)
                return;

            StopVttCoroutine(source);
            m_activeCaptions.RemoveAll(c => c.Source == source);
            NotifyCaptionsChanged();
        }

        private void StopVttCoroutine(DescriptiveMediaSource source)
        {
            if (!m_vttCoroutines.TryGetValue(source, out var coroutine))
                return;
            if (coroutine != null)
                StopCoroutine(coroutine);
            m_vttCoroutines.Remove(source);
        }

        private IEnumerator TickVttCaption(DescriptiveMediaSource source, ActiveCaption caption)
        {
            var cues = VttParser.Parse(source.DescriptiveMedia.VttTranscript);
            if (cues.Count == 0)
                yield break;

            var lastCueIndex = -1;

            while (source != null && source.IsPlaying)
            {
                var cueIndex = FindActiveCueIndex(cues, source.MediaTime);

                if (cueIndex != lastCueIndex)
                {
                    caption.Text = cueIndex >= 0 ? cues[cueIndex].Text : string.Empty;
                    lastCueIndex = cueIndex;
                    NotifyCaptionsChanged();
                }

                yield return null;
            }

            // Media stopped naturally, clean up if not already removed by an explicit Stop call.
            if (m_activeCaptions.Exists(c => c.Source == source))
                RegisterStop(source);
        }

        private IEnumerator ExpireOneShotCaption(ActiveCaption caption, float duration)
        {
            yield return new WaitForSeconds(duration);
            m_activeCaptions.Remove(caption);
            NotifyCaptionsChanged();
        }

        private bool PassesFilter(DescriptiveMediaSO media)
        {
            if (media.Priority < AccessibilitySettings.Instance.minimumMediaPriority)
                return false;
            if (media is DescriptiveAudioClipSO audioClip)
                return AccessibilitySettings.Instance.enabledAudioCategories.HasFlag(
                    audioClip.Category
                );
            return true;
        }

        private static int FindActiveCueIndex(List<VttCue> cues, float time)
        {
            for (var i = 0; i < cues.Count; i++)
            {
                if (time >= cues[i].Start && time < cues[i].End)
                    return i;
            }
            return -1;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            if (m_instance != null)
                return;
            _ = Instance;
        }

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (m_instance == this)
                m_instance = null;
        }
    }
}