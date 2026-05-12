using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// A generic implementation of InputIconSet that allows definition of a list of control paths and their corresponding icons.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Generic")]
    public class InputIconSetGeneric : InputIconSet
    {
        [Serializable]
        public struct IconEntry
        {
            public string controlPath; // e.g. "<Keyboard>/w"
            public Sprite icon;
        }

        [SerializeField]
        private List<IconEntry> m_icons;
        private Dictionary<string, Sprite> m_lookup;

        public override Sprite GetIcon(string controlPath)
        {
            m_lookup ??= m_icons.ToDictionary(e => e.controlPath, e => e.icon);
            return m_lookup.TryGetValue(controlPath, out var sprite) ? sprite : null;
        }
    }
}
