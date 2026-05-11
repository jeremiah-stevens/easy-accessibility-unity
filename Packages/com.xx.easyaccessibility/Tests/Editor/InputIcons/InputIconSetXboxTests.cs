using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests
{
    public class InputIconSetXboxTests
    {
        InputIconSetXbox360 m_xbox360;
        InputIconSetXboxOne m_xboxOne;
        InputIconSetXboxSeries m_xboxSeries;

        static readonly FieldInfo k_Atlas =
            typeof(InputIconSetXbox).GetField("m_atlas", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Overrides =
            typeof(InputIconSetXbox).GetField("m_overrides", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Suffix =
            typeof(InputIconSetXbox).GetField("suffix", BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            var atlas = LoadXboxAtlas();
            m_xbox360 = ScriptableObject.CreateInstance<InputIconSetXbox360>();
            m_xboxOne = ScriptableObject.CreateInstance<InputIconSetXboxOne>();
            m_xboxSeries = ScriptableObject.CreateInstance<InputIconSetXboxSeries>();
            k_Atlas.SetValue(m_xbox360, atlas);
            k_Atlas.SetValue(m_xboxOne, atlas);
            k_Atlas.SetValue(m_xboxSeries, atlas);
        }

        [TearDown]
        public void TearDown()
        {
            var overrides360 = k_Overrides.GetValue(m_xbox360) as InputIconSet;
            if (overrides360 != null) DestroyImmediate(overrides360);
            DestroyImmediate(m_xbox360);
            DestroyImmediate(m_xboxOne);
            DestroyImmediate(m_xboxSeries);
        }

        #region Helpers

        static SpriteAtlas LoadXboxAtlas()
        {
            var guids = AssetDatabase.FindAssets("TestAtlas_Xbox t:SpriteAtlas");
            if (guids.Length == 0) return null;
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        Sprite MakeSprite() =>
            Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);

        void InjectAtlas(ScriptableObject set, SpriteAtlas atlas) => k_Atlas.SetValue(set, atlas);
        void InjectOverrides(ScriptableObject set, InputIconSet overrides) => k_Overrides.SetValue(set, overrides);
        void InjectSuffix(ScriptableObject set, string suffix) => k_Suffix.SetValue(set, suffix);

        #endregion

        #region Shared Paths (all three types use Base="xbox")

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
        public void GetIcon_ReturnsNonNull_Xbox360_SharedPath(string path)
        {
            Assert.IsNotNull(m_xbox360.GetIcon(path));
        }

        [TestCaseSource(nameof(k_SharedPaths))]
        public void GetIcon_ReturnsNonNull_XboxOne_SharedPath(string path)
        {
            Assert.IsNotNull(m_xboxOne.GetIcon(path));
        }

        [TestCaseSource(nameof(k_SharedPaths))]
        public void GetIcon_ReturnsNonNull_XboxSeries_SharedPath(string path)
        {
            Assert.IsNotNull(m_xboxSeries.GetIcon(path));
        }

        #endregion

        #region Version-Specific System Buttons

        [Test]
        public void Xbox360_GetIcon_ReturnsNonNull_ForStart() =>
            Assert.IsNotNull(m_xbox360.GetIcon("<Gamepad>/start"));

        [Test]
        public void Xbox360_GetIcon_ReturnsNonNull_ForSelect() =>
            Assert.IsNotNull(m_xbox360.GetIcon("<Gamepad>/select"));

        [Test]
        public void XboxOne_GetIcon_ReturnsNonNull_ForStart() =>
            Assert.IsNotNull(m_xboxOne.GetIcon("<Gamepad>/start"));

        [Test]
        public void XboxOne_GetIcon_ReturnsNonNull_ForSelect() =>
            Assert.IsNotNull(m_xboxOne.GetIcon("<Gamepad>/select"));

        [Test]
        public void XboxSeries_GetIcon_ReturnsNonNull_ForStart() =>
            Assert.IsNotNull(m_xboxSeries.GetIcon("<Gamepad>/start"));

        [Test]
        public void XboxSeries_GetIcon_ReturnsNonNull_ForSelect() =>
            Assert.IsNotNull(m_xboxSeries.GetIcon("<Gamepad>/select"));

        [Test]
        public void XboxSeries_GetIcon_ReturnsNonNull_ForShareButton() =>
            Assert.IsNotNull(m_xboxSeries.GetIcon("<XboxGamepadWindows>/shareButton"));

        #endregion

        #region Fallback and Overrides

        [Test]
        public void GetIcon_ReturnsNull_ForUnrecognizedPath()
        {
            Assert.IsNull(m_xbox360.GetIcon("<Keyboard>/a"));
        }

        [Test]
        public void GetIcon_ReturnsOverrideSprite_WhenOverrideSetContainsPath()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            InjectAtlas(m_xbox360, null);
            InjectOverrides(m_xbox360, overrides);

            Assert.AreSame(overrideSprite, m_xbox360.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_OverrideTakesPrecedence_OverAtlasSprite()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Gamepad>/buttonSouth";
            InjectOverrides(m_xbox360, overrides);

            Assert.AreSame(overrideSprite, m_xbox360.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_FallsBackToBaseName_WhenSuffixedSpriteNotFound()
        {
            InjectSuffix(m_xbox360, "_dark");

            Assert.IsNotNull(m_xbox360.GetIcon("<Gamepad>/buttonSouth"));
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
