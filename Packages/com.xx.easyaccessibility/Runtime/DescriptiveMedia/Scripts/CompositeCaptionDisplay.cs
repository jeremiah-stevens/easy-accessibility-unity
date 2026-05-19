using System.Collections.Generic;
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Forwards caption updates to an array of child <see cref="CaptionDisplay"/>
    /// implementations. Use this to run multiple displays simultaneously (e.g. a static
    /// bar for ambient audio and world-space bubbles for dialogue).
    ///
    /// Child displays listed in <see cref="_displays"/> should have their component
    /// <b>disabled</b> so they do not subscribe to <see cref="DescriptiveMediaManager"/>
    /// independently and receive duplicate updates.
    /// </summary>
    public class CompositeCaptionDisplay : CaptionDisplay
    {
        [Tooltip(
            "Each display is driven by this composite. Disable the child CaptionDisplay "
                + "components directly to prevent them from double-subscribing."
        )]
        [SerializeField]
        CaptionDisplay[] _displays;

        public override void Refresh(IReadOnlyList<ActiveCaption> captions)
        {
            foreach (var display in _displays)
                display?.Refresh(captions);
        }
    }
}
