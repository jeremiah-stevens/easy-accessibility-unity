using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests
{
    public class InputIconSetPlayStationTests
    {
        InputIconSetPlayStation3 m_ps3;
        InputIconSetPlayStation4 m_ps4;
        InputIconSetPlayStation5 m_ps5;

        static readonly FieldInfo k_Atlas =
            typeof(InputIconSetPlayStation).GetField("m_atlas", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Overrides =
            typeof(InputIconSetPlayStation).GetField("m_overrides", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Suffix =
            typeof(InputIconSetPlayStation).GetField("suffix", BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            var atlas = LoadPlayStationAtlas();
            m_ps3 = ScriptableObject.CreateInstance<InputIconSetPlayStation3>();
            m_ps4 = ScriptableObject.CreateInstance<InputIconSetPlayStation4>();
            m_ps5 = ScriptableObject.CreateInstance<InputIconSetPlayStation5>();
            k_Atlas.SetValue(m_ps3, atlas);
            k_Atlas.SetValue(m_ps4, atlas);
            k_Atlas.SetValue(m_ps5, atlas);
        }

        [TearDown]
        public void TearDown()
        {
            var overrides3 = k_Overrides.GetValue(m_ps3) as InputIconSet;
            if (overrides3 != null) DestroyImmediate(overrides3);
            DestroyImmediate(m_ps3);
            DestroyImmediate(m_ps4);
            DestroyImmediate(m_ps5);
        }

        #region Helpers

        static SpriteAtlas LoadPlayStationAtlas()
        {
            var guids = AssetDatabase.FindAssets("TestAtlas_PlayStation t:SpriteAtlas");
            if (guids.Length == 0) return null;
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        Sprite MakeSprite() =>
            Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);

        void InjectAtlas(ScriptableObject set, SpriteAtlas atlas) => k_Atlas.SetValue(set, atlas);
        void InjectOverrides(ScriptableObject set, InputIconSet overrides) => k_Overrides.SetValue(set, overrides);
        void InjectSuffix(ScriptableObject set, string suffix) => k_Suffix.SetValue(set, suffix);

        #endregion

        #region Shared Paths (face, triggers, sticks, dpad : all three versions)

        static readonly string[] k_SharedPaths =
        {
            "<Gamepad>/buttonSouth", "<Gamepad>/buttonEast",
            "<Gamepad>/buttonNorth", "<Gamepad>/buttonWest",
            "<Gamepad>/leftShoulder",  "<Gamepad>/rightShoulder",
            "<Gamepad>/leftTrigger",   "<Gamepad>/rightTrigger",
            "<Gamepad>/leftStick",       "<Gamepad>/leftStick/up",   "<Gamepad>/leftStick/down",
            "<Gamepad>/leftStick/left",  "<Gamepad>/leftStick/right", "<Gamepad>/leftStickPress",
            "<Gamepad>/rightStick",      "<Gamepad>/rightStick/up",  "<Gamepad>/rightStick/down",
            "<Gamepad>/rightStick/left", "<Gamepad>/rightStick/right", "<Gamepad>/rightStickPress",
            "<Gamepad>/dpad",      "<Gamepad>/dpad/up",   "<Gamepad>/dpad/down",
            "<Gamepad>/dpad/left", "<Gamepad>/dpad/right",
        };

        [TestCaseSource(nameof(k_SharedPaths))]
        public void GetIcon_ReturnsNonNull_PS3_SharedPath(string path)
        {
            Assert.IsNotNull(m_ps3.GetIcon(path));
        }

        [TestCaseSource(nameof(k_SharedPaths))]
        public void GetIcon_ReturnsNonNull_PS4_SharedPath(string path)
        {
            Assert.IsNotNull(m_ps4.GetIcon(path));
        }

        [TestCaseSource(nameof(k_SharedPaths))]
        public void GetIcon_ReturnsNonNull_PS5_SharedPath(string path)
        {
            Assert.IsNotNull(m_ps5.GetIcon(path));
        }

        #endregion

        #region Version-Specific System Buttons

        [Test]
        public void PS3_GetIcon_ReturnsNonNull_ForStart() =>
            Assert.IsNotNull(m_ps3.GetIcon("<Gamepad>/start"));

        [Test]
        public void PS3_GetIcon_ReturnsNonNull_ForSelect() =>
            Assert.IsNotNull(m_ps3.GetIcon("<Gamepad>/select"));

        [Test]
        public void PS4_GetIcon_ReturnsNonNull_ForStart() =>
            Assert.IsNotNull(m_ps4.GetIcon("<Gamepad>/start"));

        [Test]
        public void PS4_GetIcon_ReturnsNonNull_ForSelect() =>
            Assert.IsNotNull(m_ps4.GetIcon("<Gamepad>/select"));

        [Test]
        public void PS5_GetIcon_ReturnsNonNull_ForStart() =>
            Assert.IsNotNull(m_ps5.GetIcon("<Gamepad>/start"));

        [Test]
        public void PS5_GetIcon_ReturnsNonNull_ForSelect() =>
            Assert.IsNotNull(m_ps5.GetIcon("<Gamepad>/select"));

        #endregion

        #region Device-Specific Paths

        [Test]
        public void PS4_GetIcon_ReturnsNonNull_ForTouchpadButton() =>
            Assert.IsNotNull(m_ps4.GetIcon("<DualShock4GamepadHID>/touchpadButton"));

        [Test]
        public void PS4_GetIcon_ReturnsNonNull_ForTouchpadTap() =>
            Assert.IsNotNull(m_ps4.GetIcon("<DualShock4GamepadHID>/touchpadTap"));

        [Test]
        public void PS5_GetIcon_ReturnsNonNull_ForTouchpadButton() =>
            Assert.IsNotNull(m_ps5.GetIcon("<DualSenseGamepadHID>/touchpadButton"));

        [Test]
        public void PS5_GetIcon_ReturnsNonNull_ForTouchpadTap() =>
            Assert.IsNotNull(m_ps5.GetIcon("<DualSenseGamepadHID>/touchpadTap"));

        [Test]
        public void PS5_GetIcon_ReturnsNonNull_ForMicButton() =>
            Assert.IsNotNull(m_ps5.GetIcon("<DualSenseGamepadHID>/micButton"));

        #endregion

        #region Fallback and Overrides

        [Test]
        public void GetIcon_ReturnsNull_ForUnrecognizedPath()
        {
            Assert.IsNull(m_ps3.GetIcon("<Keyboard>/a"));
        }

        [Test]
        public void GetIcon_ReturnsOverrideSprite_WhenOverrideSetContainsPath()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            InjectAtlas(m_ps3, null);
            InjectOverrides(m_ps3, overrides);

            Assert.AreSame(overrideSprite, m_ps3.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_OverrideTakesPrecedence_OverAtlasSprite()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            InjectOverrides(m_ps3, overrides);

            Assert.AreSame(overrideSprite, m_ps3.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_FallsBackToBaseName_WhenSuffixedSpriteNotFound()
        {
            InjectSuffix(m_ps3, "_dark");

            Assert.IsNotNull(m_ps3.GetIcon("<Gamepad>/buttonSouth"));
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
