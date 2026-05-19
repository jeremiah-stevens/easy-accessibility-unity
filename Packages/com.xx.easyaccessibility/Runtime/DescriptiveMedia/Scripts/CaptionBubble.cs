using TMPro;
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// A single world-space caption bubble. Managed by
    /// <see cref="WorldSpaceCaptionBubbles"/>; do not add this to scenes directly.
    /// Assumes a Screen Space Overlay canvas parent.
    /// </summary>
    public class CaptionBubble : MonoBehaviour
    {
        [SerializeField]
        TMP_Text _label;

        [SerializeField]
        RectTransform _rectTransform;

        public void SetText(string text) => _label.text = text;

        /// <summary>
        /// Projects <paramref name="worldPosition"/> to screen space and repositions the
        /// bubble. Hides the bubble when the position is behind or outside the viewport.
        /// </summary>
        public void UpdateScreenPosition(Camera cam, Vector3 worldPosition)
        {
            var viewportPoint = cam.WorldToViewportPoint(worldPosition);

            var isVisible =
                viewportPoint.z > 0
                && viewportPoint.x is > 0 and < 1
                && viewportPoint.y is > 0 and < 1;

            gameObject.SetActive(isVisible);

            if (isVisible)
                _rectTransform.position = cam.WorldToScreenPoint(worldPosition);
        }
    }
}
