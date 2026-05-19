using System.Collections.Generic;
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Spawns a <see cref="CaptionBubble"/> for each active caption that has a known
    /// world position, and destroys it when the caption is removed. Bubble screen
    /// positions are updated every <c>LateUpdate</c> to track moving sources.
    /// </summary>
    public class WorldSpaceCaptionBubbles : CaptionDisplay
    {
        [SerializeField]
        CaptionBubble _bubblePrefab;

        [SerializeField]
        Camera _camera;

        private readonly Dictionary<ActiveCaption, CaptionBubble> m_bubbles = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            _camera ??= Camera.main;
        }

        public override void Refresh(IReadOnlyList<ActiveCaption> captions)
        {
            var active = new HashSet<ActiveCaption>();

            foreach (var caption in captions)
            {
                if (caption.WorldPosition == null)
                    continue;

                active.Add(caption);

                if (!m_bubbles.TryGetValue(caption, out var bubble))
                {
                    bubble = Instantiate(_bubblePrefab, transform);
                    m_bubbles[caption] = bubble;
                }

                bubble.SetText(caption.Text);
            }

            RemoveInactiveBubbles(active);
        }

        private void LateUpdate()
        {
            if (_camera == null)
                return;

            foreach (var kvp in m_bubbles)
            {
                var worldPos = kvp.Key.WorldPosition;
                if (worldPos == null)
                    continue;
                kvp.Value.UpdateScreenPosition(_camera, worldPos.Value);
            }
        }

        private void RemoveInactiveBubbles(HashSet<ActiveCaption> active)
        {
            var toRemove = new List<ActiveCaption>();
            foreach (var kvp in m_bubbles)
                if (!active.Contains(kvp.Key))
                {
                    Destroy(kvp.Value.gameObject);
                    toRemove.Add(kvp.Key);
                }
            foreach (var key in toRemove)
                m_bubbles.Remove(key);
        }
    }
}
