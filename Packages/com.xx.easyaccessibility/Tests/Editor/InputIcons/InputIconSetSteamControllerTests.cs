using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.InputIcons
{
    public class InputIconSetSteamControllerTests
    {
        InputIconSetSteamController m_steamController;

        static readonly FieldInfo k_Atlas =
            typeof(InputIconSetSteamController).GetField("m_atlas", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Overrides =
            typeof(InputIconSetSteamController).GetField("m_overrides", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Suffix =
            typeof(InputIconSetSteamController).GetField("suffix", BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            m_steamController = ScriptableObject.CreateInstance<InputIconSetSteamController>();
            k_Atlas.SetValue(m_steamController, LoadSteamControllerAtlas());
        }

        [TearDown]
        public void TearDown()
        {
            var overrides = k_Overrides.GetValue(m_steamController) as InputIconSet;
            if (overrides != null) DestroyImmediate(overrides);
            DestroyImmediate(m_steamController);
        }

        #region Helpers

        static SpriteAtlas LoadSteamControllerAtlas()
        {
            var guids = AssetDatabase.FindAssets("TestAtlas_SteamController t:SpriteAtlas");
            if (guids.Length == 0) return null;
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        Sprite MakeSprite() =>
            Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);

        #endregion

        #region Path Coverage

        static readonly string[] k_FaceAndShoulderPaths =
        {
            "<Gamepad>/buttonSouth", "<Gamepad>/buttonEast",
            "<Gamepad>/buttonNorth", "<Gamepad>/buttonWest",
            "<Gamepad>/leftShoulder",  "<Gamepad>/rightShoulder",
            "<Gamepad>/leftTrigger",   "<Gamepad>/rightTrigger",
            "<Gamepad>/start", "<Gamepad>/select",
        };

        static readonly string[] k_StickPaths =
        {
            "<Gamepad>/leftStick",       "<Gamepad>/leftStick/up",    "<Gamepad>/leftStick/down",
            "<Gamepad>/leftStick/left",  "<Gamepad>/leftStick/right", "<Gamepad>/leftStickPress",
        };

        static readonly string[] k_TrackpadRightPaths =
        {
            "<Gamepad>/rightStick",      "<Gamepad>/rightStick/up",   "<Gamepad>/rightStick/down",
            "<Gamepad>/rightStick/left", "<Gamepad>/rightStick/right", "<Gamepad>/rightStickPress",
        };

        static readonly string[] k_TrackpadLeftPaths =
        {
            "<Gamepad>/dpad",      "<Gamepad>/dpad/up",   "<Gamepad>/dpad/down",
            "<Gamepad>/dpad/left", "<Gamepad>/dpad/right",
        };

        [TestCaseSource(nameof(k_FaceAndShoulderPaths))]
        public void GetIcon_ReturnsNonNull_ForFaceAndShoulderPath(string path)
        {
            Assert.IsNotNull(m_steamController.GetIcon(path));
        }

        [TestCaseSource(nameof(k_StickPaths))]
        public void GetIcon_ReturnsNonNull_ForLeftStickPath(string path)
        {
            Assert.IsNotNull(m_steamController.GetIcon(path));
        }

        [TestCaseSource(nameof(k_TrackpadRightPaths))]
        public void GetIcon_ReturnsNonNull_ForRightTrackpadPath(string path)
        {
            Assert.IsNotNull(m_steamController.GetIcon(path));
        }

        [TestCaseSource(nameof(k_TrackpadLeftPaths))]
        public void GetIcon_ReturnsNonNull_ForLeftTrackpadPath(string path)
        {
            Assert.IsNotNull(m_steamController.GetIcon(path));
        }

        #endregion

        #region Fallback and Overrides

        [Test]
        public void GetIcon_ReturnsNull_ForUnrecognizedPath()
        {
            Assert.IsNull(m_steamController.GetIcon("<Keyboard>/a"));
        }

        [Test]
        public void GetIcon_ReturnsOverrideSprite_WhenOverrideSetContainsPath()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            k_Atlas.SetValue(m_steamController, null);
            k_Overrides.SetValue(m_steamController, overrides);

            Assert.AreSame(overrideSprite, m_steamController.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_OverrideTakesPrecedence_OverAtlasSprite()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            k_Overrides.SetValue(m_steamController, overrides);

            Assert.AreSame(overrideSprite, m_steamController.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_FallsBackToBaseName_WhenSuffixedSpriteNotFound()
        {
            k_Suffix.SetValue(m_steamController, "_dark");

            Assert.IsNotNull(m_steamController.GetIcon("<Gamepad>/buttonSouth"));
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
