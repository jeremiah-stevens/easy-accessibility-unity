using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.InputIcons
{
    public class InputIconSetSteamDeckTests
    {
        InputIconSetSteamDeck m_steamDeck;

        static readonly FieldInfo k_Atlas = typeof(InputIconSetSteamDeck).GetField(
            "m_atlas",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Overrides = typeof(InputIconSetSteamDeck).GetField(
            "m_overrides",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_Suffix = typeof(InputIconSetSteamDeck).GetField(
            "suffix",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        [SetUp]
        public void SetUp()
        {
            m_steamDeck = ScriptableObject.CreateInstance<InputIconSetSteamDeck>();
            k_Atlas.SetValue(m_steamDeck, LoadSteamDeckAtlas());
        }

        [TearDown]
        public void TearDown()
        {
            var overrides = k_Overrides.GetValue(m_steamDeck) as InputIconSet;
            if (overrides != null)
                DestroyImmediate(overrides);
            DestroyImmediate(m_steamDeck);
        }

        #region Helpers

        static SpriteAtlas LoadSteamDeckAtlas()
        {
            var guids = AssetDatabase.FindAssets("TestAtlas_SteamDeck t:SpriteAtlas");
            if (guids.Length == 0)
                return null;
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(
                AssetDatabase.GUIDToAssetPath(guids[0])
            );
        }

        Sprite MakeSprite() =>
            Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);

        #endregion

        #region Standard Gamepad Paths

        static readonly string[] k_StandardPaths =
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

        [TestCaseSource(nameof(k_StandardPaths))]
        public void GetIcon_ReturnsNonNull_ForStandardGamepadPath(string path)
        {
            Assert.IsNotNull(m_steamDeck.GetIcon(path));
        }

        #endregion

        #region Back Grip Buttons

        static readonly string[] k_GripButtonPaths =
        {
            "<SteamDeckGamepad>/l4",
            "<SteamDeckGamepad>/l5",
            "<SteamDeckGamepad>/r4",
            "<SteamDeckGamepad>/r5",
        };

        [TestCaseSource(nameof(k_GripButtonPaths))]
        public void GetIcon_ReturnsNonNull_ForGripButton(string path)
        {
            Assert.IsNotNull(m_steamDeck.GetIcon(path));
        }

        #endregion

        #region Fallback and Overrides

        [Test]
        public void GetIcon_ReturnsNull_ForUnrecognizedPath()
        {
            Assert.IsNull(m_steamDeck.GetIcon("<Keyboard>/a"));
        }

        [Test]
        public void GetIcon_ReturnsOverrideSprite_WhenOverrideSetContainsPath()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            k_Atlas.SetValue(m_steamDeck, null);
            k_Overrides.SetValue(m_steamDeck, overrides);

            Assert.AreSame(overrideSprite, m_steamDeck.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_OverrideTakesPrecedence_OverAtlasSprite()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            k_Overrides.SetValue(m_steamDeck, overrides);

            Assert.AreSame(overrideSprite, m_steamDeck.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_FallsBackToBaseName_WhenSuffixedSpriteNotFound()
        {
            k_Suffix.SetValue(m_steamDeck, "_dark");

            Assert.IsNotNull(m_steamDeck.GetIcon("<Gamepad>/buttonSouth"));
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
