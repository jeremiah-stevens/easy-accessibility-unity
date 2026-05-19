using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Appends an entry to a scrolling list whenever a source's caption text changes.
    /// Useful for dense audio environments where captions overlap. Old entries are
    /// pruned once <see cref="_maxEntries"/> is exceeded.
    /// </summary>
    public class ScrollingCaptionLog : CaptionDisplay
    {
        [SerializeField]
        TMP_Text _entryPrefab;

        [SerializeField]
        Transform _container;

        [SerializeField]
        ScrollRect _scrollRect;

        [SerializeField]
        int _maxEntries = 20;

        // Tracks the last displayed text per source to detect changes.
        private readonly Dictionary<DescriptiveMediaSource, string> m_lastText = new();

        public override void Refresh(IReadOnlyList<ActiveCaption> captions)
        {
            var activeSources = new HashSet<DescriptiveMediaSource>();
            var appended = false;

            foreach (var caption in captions)
            {
                activeSources.Add(caption.Source);

                if (string.IsNullOrEmpty(caption.Text))
                    continue;

                m_lastText.TryGetValue(caption.Source, out var last);
                if (caption.Text == last)
                    continue;

                m_lastText[caption.Source] = caption.Text;
                var entry = Instantiate(_entryPrefab, _container);
                entry.text = caption.Text;
                appended = true;
            }

            RemoveStoppedSources(activeSources);

            if (!appended)
                return;

            TrimEntries();
            Canvas.ForceUpdateCanvases();
            _scrollRect.normalizedPosition = Vector2.zero;
        }

        private void RemoveStoppedSources(HashSet<DescriptiveMediaSource> activeSources)
        {
            var stopped = new List<DescriptiveMediaSource>();
            foreach (var key in m_lastText.Keys)
                if (!activeSources.Contains(key))
                    stopped.Add(key);
            foreach (var key in stopped)
                m_lastText.Remove(key);
        }

        private void TrimEntries()
        {
            while (_container.childCount > _maxEntries)
                Destroy(_container.GetChild(0).gameObject);
        }
    }
}
