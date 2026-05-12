using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests.RebindableKeys
{
    public class RebindableActionTests
    {
        GameObject m_gameObject;
        RebindableAction m_rebindableAction;
        GameObject m_managerGameObject;
        RebindableInputManager m_manager;
        InputActionAsset m_asset;
        InputAction m_buttonAction;
        InputAction m_compositeAction;
        AccessibilitySettings m_settings;

        static readonly FieldInfo k_ActionField = typeof(RebindableAction).GetField(
            "m_action",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        static readonly FieldInfo k_ManagerInstance = typeof(RebindableInputManager).GetField(
            "m_instance",
            BindingFlags.NonPublic | BindingFlags.Static
        );
        static readonly FieldInfo k_SettingsInstance = typeof(AccessibilitySettings).GetField(
            "m_instance",
            BindingFlags.NonPublic | BindingFlags.Static
        );

        [SetUp]
        public void SetUp()
        {
            k_ManagerInstance.SetValue(null, null);
            k_SettingsInstance.SetValue(null, null);

            m_settings = ScriptableObject.CreateInstance<AccessibilitySettings>();
            k_SettingsInstance.SetValue(null, m_settings);

            m_asset = CreateTestAsset();
            m_buttonAction = m_asset.FindAction("ButtonAction");
            m_compositeAction = m_asset.FindAction("CompositeAction");

            m_managerGameObject = new GameObject();
            m_manager = m_managerGameObject.AddComponent<RebindableInputManager>();
            m_manager.actionsAsset = m_asset;
            k_ManagerInstance.SetValue(null, m_manager);

            m_gameObject = new GameObject();
            m_gameObject.SetActive(false);
            m_rebindableAction = m_gameObject.AddComponent<RebindableAction>();
            k_ActionField.SetValue(m_rebindableAction, InputActionReference.Create(m_buttonAction));
            m_rebindableAction.bindingId = m_buttonAction.bindings[0].id.ToString();
            m_gameObject.SetActive(true);
            m_rebindableAction.OnEnable();
        }

        [TearDown]
        public void TearDown()
        {
            m_manager.CancelRebind();
            m_rebindableAction.OnDisable();
            DestroyImmediate(m_gameObject);
            DestroyImmediate(m_managerGameObject);
            k_ManagerInstance.SetValue(null, null);
            DestroyImmediate(m_asset);
            k_SettingsInstance.SetValue(null, null);
            DestroyImmediate(m_settings);
        }

        #region Helpers

        InputActionAsset CreateTestAsset()
        {
            var asset = ScriptableObject.CreateInstance<InputActionAsset>();
            var map = asset.AddActionMap("TestMap");

            var button = map.AddAction("ButtonAction", InputActionType.Button);
            button.AddBinding("<Keyboard>/space");

            var composite = map.AddAction("CompositeAction", InputActionType.Value);
            composite
                .AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            map.Enable();
            return asset;
        }

        RebindableAction CreateRebindableActionFor(InputAction action, int bindingIndex)
        {
            var go = new GameObject();
            go.SetActive(false);
            var ra = go.AddComponent<RebindableAction>();
            k_ActionField.SetValue(ra, InputActionReference.Create(action));
            ra.bindingId = action.bindings[bindingIndex].id.ToString();
            go.SetActive(true);
            return ra;
        }

        #endregion

        #region Display Properties

        [Test]
        public void ActionName_ReturnsNonEmptyString()
        {
            Assert.IsFalse(string.IsNullOrEmpty(m_rebindableAction.ActionName));
        }

        [Test]
        public void ActionName_IncludesCompositePart_WhenBindingIsPartOfComposite()
        {
            // bindings[0] is the composite root; bindings[1] is the "Up" part (isPartOfComposite=true)
            var ra = CreateRebindableActionFor(m_compositeAction, 1);

            Assert.That(ra.ActionName, Does.Contain(m_compositeAction.name));
            Assert.That(ra.ActionName, Does.Contain(m_compositeAction.bindings[1].name));

            DestroyImmediate(ra.gameObject);
        }

        [Test]
        public void BindingDisplayString_ReflectsCurrentBindingPath()
        {
            Assert.IsFalse(string.IsNullOrEmpty(m_rebindableAction.BindingDisplayString));
        }

        [Test]
        public void BindingDisplayString_UpdatesAfterRebindCompletes()
        {
            var initial = m_rebindableAction.BindingDisplayString;
            m_buttonAction.ApplyBindingOverride(0, "<Keyboard>/z");

            Assert.AreNotEqual(initial, m_rebindableAction.BindingDisplayString);
        }

        [Test]
        public void BindingIcon_ReturnsNull_WhenNoIconSetPresent()
        {
            m_manager.iconSet = null;

            Assert.IsNull(m_rebindableAction.BindingIcon);
        }

        [Test]
        public void BindingIcon_ReturnsSprite_WhenIconSetIsConfigured()
        {
            var iconSet = ScriptableObject.CreateInstance<TestIconSet>();
            iconSet.sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
            m_manager.iconSet = iconSet;

            Assert.IsNotNull(m_rebindableAction.BindingIcon);

            DestroyImmediate(iconSet);
        }

        #endregion

        #region Rebind Delegation

        [Test]
        public void StartRebind_DelegatesToManager()
        {
            m_rebindableAction.StartRebind();

            Assert.IsFalse(m_buttonAction.actionMap.enabled);

            m_manager.CancelRebind();
        }

        [Test]
        public void ResetBinding_DelegatesToManager()
        {
            m_buttonAction.ApplyBindingOverride(0, "<Keyboard>/z");

            m_rebindableAction.ResetBinding();

            Assert.AreEqual("<Keyboard>/space", m_buttonAction.bindings[0].effectivePath);
        }

        #endregion

        #region Events

        [Test]
        public void OnRebindStarted_Fires_WhenStartRebindCalled()
        {
            bool fired = false;
            m_rebindableAction.onRebindStarted.AddListener(() => fired = true);

            m_rebindableAction.StartRebind();
            m_manager.CancelRebind();

            Assert.IsTrue(fired);
        }

        [Test]
        public void OnRebindCompleted_Fires_WhenRebindFinishes()
        {
            bool fired = false;
            m_rebindableAction.onRebindCompleted.AddListener(() => fired = true);

            // ResetBinding fires OnBindingChanged → HandleBindingChanged → onRebindCompleted
            m_manager.ResetBinding(m_buttonAction, 0);

            Assert.IsTrue(fired);
        }

        [Test]
        public void OnRebindCancelled_Fires_WhenRebindCancelled()
        {
            bool fired = false;
            m_rebindableAction.onRebindCancelled.AddListener(() => fired = true);

            m_manager.onBindingCancelled.Invoke();

            Assert.IsTrue(fired);
        }

        [Test]
        public void OnRebindReset_Fires_WhenResetBindingCalled()
        {
            bool fired = false;
            m_rebindableAction.onRebindReset.AddListener(() => fired = true);

            m_rebindableAction.ResetBinding();

            Assert.IsTrue(fired);
        }

        [Test]
        public void OnDisplayStringChanged_Fires_WithNewBindingString_AfterChange()
        {
            string received = null;
            m_rebindableAction.onDisplayStringChanged.AddListener(s => received = s);

            m_manager.ResetBinding(m_buttonAction, 0);

            Assert.IsNotNull(received);
        }

        [Test]
        public void OnIconChanged_Fires_WithNewSprite_AfterIconChanges()
        {
            var iconSet = ScriptableObject.CreateInstance<TestIconSet>();
            iconSet.sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.zero);
            m_manager.iconSet = iconSet;

            bool fired = false;
            m_rebindableAction.onIconChanged.AddListener(_ => fired = true);

            // m_lastIcon is null after SetUp (iconSet was null during OnEnable);
            // triggering RefreshDisplayString now produces a non-null icon, firing the event
            m_manager.ResetBinding(m_buttonAction, 0);

            Assert.IsTrue(fired);

            DestroyImmediate(iconSet);
        }

        [Test]
        public void OnDisplayStringChanged_Fires_WithDefaultString_AfterResetBinding()
        {
            m_buttonAction.ApplyBindingOverride(0, "<Keyboard>/z");
            string received = null;
            m_rebindableAction.onDisplayStringChanged.AddListener(s => received = s);

            m_rebindableAction.ResetBinding();

            Assert.AreEqual(m_rebindableAction.BindingDisplayString, received);
        }

        #endregion

        private class TestIconSet : InputIconSet
        {
            public Sprite sprite;

            public override Sprite GetIcon(string controlPath) => sprite;
        }
    }
}
