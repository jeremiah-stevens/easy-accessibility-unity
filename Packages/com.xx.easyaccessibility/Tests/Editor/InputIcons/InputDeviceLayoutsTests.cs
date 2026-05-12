using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace EasyAccessibility.Tests.InputIcons
{
    public class InputDeviceLayoutsTests
    {
        #region Constant values

        [Test]
        [TestCase(InputDeviceLayouts.Keyboard,  "Keyboard")]
        [TestCase(InputDeviceLayouts.Mouse,     "Mouse")]
        [TestCase(InputDeviceLayouts.Gamepad,   "Gamepad")]
        [TestCase(InputDeviceLayouts.XInput,    "XInputController")]
        [TestCase(InputDeviceLayouts.PS3,       "DualShock3GamepadHID")]
        [TestCase(InputDeviceLayouts.PS4,       "DualShock4GamepadHID")]
        [TestCase(InputDeviceLayouts.PS5,       "DualSenseGamepadHID")]
        [TestCase(InputDeviceLayouts.SwitchPro, "SwitchProControllerHID")]
        [TestCase(InputDeviceLayouts.SteamDeck, "SteamDeckGamepad")]
        public void ConstantValue_MatchesExpected(string actual, string expected)
        {
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Uniqueness

        [Test]
        public void AllConstants_AreUnique()
        {
            var fields = typeof(InputDeviceLayouts).GetFields(BindingFlags.Public | BindingFlags.Static);
            var seen = new HashSet<string>();
            foreach (var f in fields)
                Assert.IsTrue(seen.Add((string)f.GetRawConstantValue()), $"Duplicate value for '{f.Name}'");
        }

        #endregion
    }
}
