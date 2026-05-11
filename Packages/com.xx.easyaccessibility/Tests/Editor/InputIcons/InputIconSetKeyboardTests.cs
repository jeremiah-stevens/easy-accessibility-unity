using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests
{
    public class InputIconSetKeyboardTests
    {
        InputIconSetKeyboard m_iconSet;

        static readonly FieldInfo k_Atlas =
            typeof(InputIconSetKeyboard).GetField("m_atlas", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Overrides =
            typeof(InputIconSetKeyboard).GetField("m_overrides", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo k_Suffix =
            typeof(InputIconSetKeyboard).GetField("suffix", BindingFlags.NonPublic | BindingFlags.Instance);

        [SetUp]
        public void SetUp()
        {
            m_iconSet = ScriptableObject.CreateInstance<InputIconSetKeyboard>();
            InjectAtlas(LoadKeyboardAtlas());
        }

        [TearDown]
        public void TearDown()
        {
            var overrides = k_Overrides.GetValue(m_iconSet) as InputIconSet;
            if (overrides != null) DestroyImmediate(overrides);
            DestroyImmediate(m_iconSet);
            // atlas is a project asset loaded via AssetDatabase : do not destroy
        }

        #region Helpers

        static SpriteAtlas LoadKeyboardAtlas()
        {
            var guids = AssetDatabase.FindAssets("TestAtlas_Keyboard t:SpriteAtlas");
            if (guids.Length == 0) return null;
            return AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        Sprite MakeSprite() =>
            Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);

        void InjectAtlas(SpriteAtlas atlas) => k_Atlas.SetValue(m_iconSet, atlas);
        void InjectOverrides(InputIconSet overrides) => k_Overrides.SetValue(m_iconSet, overrides);
        void InjectSuffix(string suffix) => k_Suffix.SetValue(m_iconSet, suffix);

        #endregion

        #region Key Coverage

        static readonly string[] k_LetterKeys =
        {
            "<Keyboard>/a", "<Keyboard>/b", "<Keyboard>/c", "<Keyboard>/d", "<Keyboard>/e",
            "<Keyboard>/f", "<Keyboard>/g", "<Keyboard>/h", "<Keyboard>/i", "<Keyboard>/j",
            "<Keyboard>/k", "<Keyboard>/l", "<Keyboard>/m", "<Keyboard>/n", "<Keyboard>/o",
            "<Keyboard>/p", "<Keyboard>/q", "<Keyboard>/r", "<Keyboard>/s", "<Keyboard>/t",
            "<Keyboard>/u", "<Keyboard>/v", "<Keyboard>/w", "<Keyboard>/x", "<Keyboard>/y",
            "<Keyboard>/z"
        };

        static readonly string[] k_DigitKeys =
        {
            "<Keyboard>/digit0", "<Keyboard>/digit1", "<Keyboard>/digit2", "<Keyboard>/digit3",
            "<Keyboard>/digit4", "<Keyboard>/digit5", "<Keyboard>/digit6", "<Keyboard>/digit7",
            "<Keyboard>/digit8", "<Keyboard>/digit9"
        };

        static readonly string[] k_FunctionKeys =
        {
            "<Keyboard>/f1",  "<Keyboard>/f2",  "<Keyboard>/f3",  "<Keyboard>/f4",
            "<Keyboard>/f5",  "<Keyboard>/f6",  "<Keyboard>/f7",  "<Keyboard>/f8",
            "<Keyboard>/f9",  "<Keyboard>/f10", "<Keyboard>/f11", "<Keyboard>/f12"
        };

        static readonly string[] k_NavigationKeys =
        {
            "<Keyboard>/upArrow",  "<Keyboard>/downArrow", "<Keyboard>/leftArrow", "<Keyboard>/rightArrow",
            "<Keyboard>/home",     "<Keyboard>/end",       "<Keyboard>/pageUp",    "<Keyboard>/pageDown",
            "<Keyboard>/insert",   "<Keyboard>/delete"
        };

        static readonly string[] k_ModifierKeys =
        {
            "<Keyboard>/leftShift",  "<Keyboard>/rightShift",
            "<Keyboard>/leftCtrl",   "<Keyboard>/rightCtrl",
            "<Keyboard>/leftAlt",    "<Keyboard>/rightAlt",
            "<Keyboard>/leftMeta",   "<Keyboard>/rightMeta"
        };

        static readonly string[] k_CommonKeys =
        {
            "<Keyboard>/space", "<Keyboard>/enter", "<Keyboard>/backspace",
            "<Keyboard>/tab",   "<Keyboard>/escape", "<Keyboard>/capsLock"
        };

        static readonly string[] k_NumpadKeys =
        {
            "<Keyboard>/numpad0",        "<Keyboard>/numpad1",    "<Keyboard>/numpad2",
            "<Keyboard>/numpad3",        "<Keyboard>/numpad4",    "<Keyboard>/numpad5",
            "<Keyboard>/numpad6",        "<Keyboard>/numpad7",    "<Keyboard>/numpad8",
            "<Keyboard>/numpad9",        "<Keyboard>/numpadEnter",
            "<Keyboard>/numpadPlus",     "<Keyboard>/numpadMinus",
            "<Keyboard>/numpadMultiply", "<Keyboard>/numpadDivide",
            "<Keyboard>/numpadPeriod",   "<Keyboard>/numpadEquals"
        };

        [TestCaseSource(nameof(k_LetterKeys))]
        public void GetIcon_ReturnsNonNull_ForLetterKey(string path)
        {
            Assert.IsNotNull(m_iconSet.GetIcon(path));
        }

        [TestCaseSource(nameof(k_DigitKeys))]
        public void GetIcon_ReturnsNonNull_ForDigitKey(string path)
        {
            Assert.IsNotNull(m_iconSet.GetIcon(path));
        }

        [TestCaseSource(nameof(k_FunctionKeys))]
        public void GetIcon_ReturnsNonNull_ForFunctionKey(string path)
        {
            Assert.IsNotNull(m_iconSet.GetIcon(path));
        }

        [TestCaseSource(nameof(k_NavigationKeys))]
        public void GetIcon_ReturnsNonNull_ForNavigationKey(string path)
        {
            Assert.IsNotNull(m_iconSet.GetIcon(path));
        }

        [TestCaseSource(nameof(k_ModifierKeys))]
        public void GetIcon_ReturnsNonNull_ForModifierKey(string path)
        {
            Assert.IsNotNull(m_iconSet.GetIcon(path));
        }

        [TestCaseSource(nameof(k_CommonKeys))]
        public void GetIcon_ReturnsNonNull_ForCommonKey(string path)
        {
            Assert.IsNotNull(m_iconSet.GetIcon(path));
        }

        [TestCaseSource(nameof(k_NumpadKeys))]
        public void GetIcon_ReturnsNonNull_ForNumpadKey(string path)
        {
            Assert.IsNotNull(m_iconSet.GetIcon(path));
        }

        #endregion

        #region Fallback and Overrides

        [Test]
        public void GetIcon_ReturnsNull_ForUnrecognizedPath()
        {
            Assert.IsNull(m_iconSet.GetIcon("<Gamepad>/buttonSouth"));
        }

        [Test]
        public void GetIcon_ReturnsOverrideSprite_WhenOverrideSetContainsPath()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Keyboard>/a";
            InjectAtlas(null);
            InjectOverrides(overrides);

            Assert.AreSame(overrideSprite, m_iconSet.GetIcon("<Keyboard>/a"));
        }

        [Test]
        public void GetIcon_OverrideTakesPrecedence_OverAtlasSprite()
        {
            var overrideSprite = MakeSprite();
            var overrides = ScriptableObject.CreateInstance<TestIconSet>();
            overrides.sprite = overrideSprite;
            overrides.matchPath = "<Keyboard>/a";
            InjectOverrides(overrides);
            // atlas is already loaded in SetUp, so both sources resolve for "<Keyboard>/a"

            Assert.AreSame(overrideSprite, m_iconSet.GetIcon("<Keyboard>/a"));
        }

        // The real Kenney atlas has no suffixed sprites, so this test validates the fallback
        // half of Get(): when "name+suffix" is not found, "name" is tried and succeeds.
        [Test]
        public void GetIcon_FallsBackToBaseName_WhenSuffixedSpriteNotFound()
        {
            InjectSuffix("_dark");

            Assert.IsNotNull(m_iconSet.GetIcon("<Keyboard>/a"));
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
