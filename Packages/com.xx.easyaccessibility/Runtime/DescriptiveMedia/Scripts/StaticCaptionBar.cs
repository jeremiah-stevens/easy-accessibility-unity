using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Displays all active captions as a stacked text block, typically anchored to the
    /// bottom of the screen. Hides itself when there are no active captions.
    /// </summary>
    public class StaticCaptionBar : CaptionDisplay
    {
        [SerializeField]
        TMP_Text _label;

        [SerializeField]
        string _separator = "\n";

        public override void Refresh(IReadOnlyList<ActiveCaption> captions)
        {
            var hasText = false;
            var sb = new StringBuilder();

            for (var i = 0; i < captions.Count; i++)
            {
                if (string.IsNullOrEmpty(captions[i].Text))
                    continue;
                if (hasText)
                    sb.Append(_separator);
                sb.Append(captions[i].Text);
                hasText = true;
            }

            gameObject.SetActive(hasText);
            if (hasText)
                _label.text = sb.ToString();
        }
    }
}
