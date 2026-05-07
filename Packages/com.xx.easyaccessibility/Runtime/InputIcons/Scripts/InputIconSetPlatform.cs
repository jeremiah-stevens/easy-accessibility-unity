using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    [CreateAssetMenu(fileName = "InputIconSetPlatform", menuName = "Easy Accessibility/Input Icon Set Platform")]
    public class InputIconSetPlatform : InputIconSet
    {
        [SerializeField] private List<DeviceIconEntry> m_entries;
        [SerializeField] private InputIconSet m_fallback;

        public override Sprite GetIcon(string controlPath) {
            var device = RebindableInputManager.Instance?.LastUsedDevice;
            if (device != null) {
                foreach (var entry in m_entries) {
                    if (InputSystem.IsFirstLayoutBasedOnSecond(device.layout, entry.deviceLayout))
                        return entry.iconSet?.GetIcon(controlPath);
                }
            }
            return m_fallback?.GetIcon(controlPath);
        }

        [Serializable]
        public struct DeviceIconEntry {
            public string deviceLayout; // e.g. "DualSenseGamepadHID", "Keyboard"
            public InputIconSet iconSet;
        }
    }
}