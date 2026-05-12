using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.InputIcons
{
    public class InputIconSetNintendoSwitchTests
    {
        InputIconSetNintendoSwitch m_switch;
        InputIconSetNintendoSwitch2 m_switch2;

        static readonly FieldInfo k_SwitchAtlas = typeof(InputIconSetNintendoSwitch).GetField(
            "m_atlas",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Switch2Atlas = typeof(InputIconSetNintendoSwitch2).GetField(
            "m_atlas",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_SwitchOverrides = typeof(InputIconSetNintendoSwitch).GetField(
            "m_overrides",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_SwitchSuffix = typeof(InputIconSetNintendoSwitch).GetField(
            "suffix",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        [SetUp]
        public void SetUp()
        {
            var atlas = LoadSwitchAtlas();
            m_switch = ScriptableObject.CreateInstance<InputIconSetNintendoSwitch>();
            m_switch2 = ScriptableObject.CreateInstance<InputIconSetNintendoSwitch2>();
            k_SwitchAtlas.SetValue(m_switch, atlas);
            k_Switch2Atlas.SetValue(m_switch2, atlas);
        }

        [TearDown]
        public void TearDown()
        {
            var overrides = k_SwitchOverrides.GetValue(m_switch) as InputIconSet;
            if (overrides != null)
                DestroyImmediate(overrides);
            DestroyImmediate(m_switch);
            DestroyImmediate(m_switch2);
        }

        #region Helpers

        static SpriteAtlas LoadSwitchAtlas()
        {
            var guids = AssetDatabase.FindAssets("TestAtlas_Switch t:SpriteAtlas");
            if (guids.Length == 0)
                return null;
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(
                AssetDatabase.GUIDToAssetPath(guids[0])
            );
        }

        Sprite MakeSprite() =>
            Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);

        #endregion

        #region Path Coverage

        static readonly string[] k_AllPaths =
        {
            // Face buttons
            "<Gamepad>/buttonSouth",
            "<Gamepad>/buttonEast",
            "<Gamepad>/buttonNorth",
            "<Gamepad>/buttonWest",
            // Shoulders and triggers
            "<Gamepad>/leftShoulder",
            "<Gamepad>/rightShoulder",
            "<Gamepad>/leftTrigger",
            "<Gamepad>/rightTrigger",
            // System
            "<Gamepad>/start",
            "<Gamepad>/select",
            // Left stick
            "<Gamepad>/leftStick",
            "<Gamepad>/leftStick/up",
            "<Gamepad>/leftStick/down",
            "<Gamepad>/leftStick/left",
            "<Gamepad>/leftStick/right",
            "<Gamepad>/leftStickPress",
            // Right stick
            "<Gamepad>/rightStick",
            "<Gamepad>/rightStick/up",
            "<Gamepad>/rightStick/down",
            "<Gamepad>/rightStick/left",
            "<Gamepad>/rightStick/right",
            "<Gamepad>/rightStickPress",
            // D-Pad
            "<Gamepad>/dpad",
            "<Gamepad>/dpad/up",
            "<Gamepad>/dpad/down",
            "<Gamepad>/dpad/left",
            "<Gamepad>/dpad/right",
        };

        [TestCaseSource(nameof(k_AllPaths))]
        public void GetIcon_ReturnsNonNull_Switch(string path)
        {
            Assert.IsNotNull(m_switch.GetIcon(path));
        }

        [TestCaseSource(nameof(k_AllPaths))]
        public void GetIcon_ReturnsNonNull_Switch2(string path)
        {
            Assert.IsNotNull(m_switch2.GetIcon(path));
        }

        #endregion

        #region Fallback and Overrides

        [Test]
        public void GetIcon_ReturnsNull_ForUnrecognizedPath()
        {
            Assert.IsNull(m_switch.GetIcon("<Keyboard>/a"));
        }

        [Test]
        public void GetIcon_ReturnsOverrideSprite_WhenOverrideSetContainsPath()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            k_SwitchAtlas.SetValue(m_switch, null);
            k_SwitchOverrides.SetValue(m_switch, overrides);

            Assert.AreSame(overrideSprite, m_switch.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_OverrideTakesPrecedence_OverAtlasSprite()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            k_SwitchOverrides.SetValue(m_switch, overrides);

            Assert.AreSame(overrideSprite, m_switch.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_FallsBackToBaseName_WhenSuffixedSpriteNotFound()
        {
            k_SwitchSuffix.SetValue(m_switch, "_dark");

            Assert.IsNotNull(m_switch.GetIcon("<Gamepad>/buttonSouth"));
        }

        #endregion

        private class TestIconSet : InputIconSet
        {
            public Sprite sprite;
            public string matchPath;

            public override Sprite GetIcon(string controlPath) =>
                controlPath == matchPath ? sprite : null;
        }
    }
}
