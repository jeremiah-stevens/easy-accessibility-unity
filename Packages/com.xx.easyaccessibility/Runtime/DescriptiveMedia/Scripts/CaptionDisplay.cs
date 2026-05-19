using System.Collections.Generic;
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// Abstract base for all caption UI components. Subscribes to
    /// <see cref="DescriptiveMediaManager.OnCaptionsChanged"/> automatically when enabled.
    /// Derive from this and implement <see cref="Refresh"/> to build custom caption views.
    /// </summary>
    public abstract class CaptionDisplay : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            DescriptiveMediaManager.Instance.OnCaptionsChanged += Refresh;
        }

        protected virtual void OnDisable()
        {
            if (DescriptiveMediaManager.IsInitialized)
                DescriptiveMediaManager.Instance.OnCaptionsChanged -= Refresh;
        }

        /// <summary>
        /// Called whenever the active caption list changes. Update your UI here.
        /// </summary>
        public abstract void Refresh(IReadOnlyList<ActiveCaption> captions);
    }
}
