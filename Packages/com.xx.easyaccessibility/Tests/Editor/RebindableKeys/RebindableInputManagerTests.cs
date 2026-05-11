using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace EasyAccessibility.Tests
{
    public class RebindableInputManagerTests
    {
        GameObject m_gameObject;
        RebindableInputManager m_manager;
        InputActionAsset m_asset;
        InputAction m_actionA;
        InputAction m_actionB;
        AccessibilitySettings m_settings;
        string m_settingsSavePath;
        string m_settingsBackup;

        static readonly FieldInfo k_ManagerInstance =
            typeof(RebindableInputManager).GetField("m_instance", BindingFlags.NonPublic | BindingFlags.Static);

        static readonly FieldInfo k_SettingsInstance =
            typeof(AccessibilitySettings).GetField("m_instance", BindingFlags.NonPublic | BindingFlags.Static);

        [SetUp]
        public void SetUp()
        {
            k_ManagerInstance.SetValue(null, null);
            k_SettingsInstance.SetValue(null, null);

            m_settings = ScriptableObject.CreateInstance<AccessibilitySettings>();
            k_SettingsInstance.SetValue(null, m_settings);

            m_settingsSavePath = Path.Combine(Application.persistentDataPath, "accessibility_settings.json");
            m_settingsBackup = File.Exists(m_settingsSavePath) ? File.ReadAllText(m_settingsSavePath) : null;

            m_asset = CreateTestAsset();
            m_actionA = m_asset.FindAction("ActionA");
            m_actionB = m_asset.FindAction("ActionB");

            m_gameObject = new GameObject();
            m_manager = m_gameObject.AddComponent<RebindableInputManager>();
            m_manager.actionsAsset = m_asset;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_gameObject);
            k_ManagerInstance.SetValue(null, null);

            Object.DestroyImmediate(m_asset);

            k_SettingsInstance.SetValue(null, null);
            Object.DestroyImmediate(m_settings);

            if (m_settingsBackup != null)
                File.WriteAllText(m_settingsSavePath, m_settingsBackup);
            else if (File.Exists(m_settingsSavePath))
                File.Delete(m_settingsSavePath);
        }

        #region Helpers

        InputActionAsset CreateTestAsset()
        {
            var asset = ScriptableObject.CreateInstance<InputActionAsset>();
            var map = asset.AddActionMap("TestMap");
            var actionA = map.AddAction("ActionA", InputActionType.Button);
            actionA.AddBinding("<Keyboard>/a");
            var actionB = map.AddAction("ActionB", InputActionType.Button);
            actionB.AddBinding("<Keyboard>/b");
            map.Enable();
            return asset;
        }

        BindingConflict CreateConflict(
            InputAction action, int bindingIndex,
            InputAction conflictingAction, int conflictingBindingIndex,
            string oldPath
        )
        {
            return new BindingConflict
            {
                action = action,
                bindingIndex = bindingIndex,
                conflictingAction = conflictingAction,
                conflictingBindingIndex = conflictingBindingIndex,
                oldPath = oldPath
            };
        }

        BindingConflict ArrangeConflict()
        {
            m_actionA.ApplyBindingOverride(0, "<Keyboard>/b");
            return CreateConflict(m_actionA, 0, m_actionB, 0, "<Keyboard>/a");
        }

        #endregion

        #region Initialization

        [Test]
        public void Instance_CreatesGameObject_WhenNoneExists()
        {
            k_ManagerInstance.SetValue(null, null);

            var instance = RebindableInputManager.Instance;

            Assert.IsNotNull(instance);
            Assert.IsNotNull(GameObject.Find("RebindableInputManager"));

            Object.DestroyImmediate(instance.gameObject);
            k_ManagerInstance.SetValue(null, null);
        }

        [Test]
        public void Instance_ReturnsSameReference_OnRepeatedCalls()
        {
            k_ManagerInstance.SetValue(null, m_manager);

            Assert.AreSame(RebindableInputManager.Instance, RebindableInputManager.Instance);
        }

        [Test]
        public void IsInitialized_ReturnsFalse_BeforeFirstAccess()
        {
            k_ManagerInstance.SetValue(null, null);

            Assert.IsFalse(RebindableInputManager.IsInitialized);
        }

        [Test]
        public void IsInitialized_ReturnsTrue_AfterInstanceAccessed()
        {
            k_ManagerInstance.SetValue(null, m_manager);

            Assert.IsTrue(RebindableInputManager.IsInitialized);
        }

        #endregion

        #region CancelRebind

        [Test]
        public void CancelRebind_DoesNotThrow_WhenNoRebindInProgress()
        {
            Assert.DoesNotThrow(() => m_manager.CancelRebind());
        }

        #endregion

        #region ResetBinding

        [Test]
        public void ResetBinding_RemovesOverride_WhenOverrideIsPresent()
        {
            m_actionA.ApplyBindingOverride(0, "<Keyboard>/z");

            m_manager.ResetBinding(m_actionA, 0);

            Assert.AreEqual("<Keyboard>/a", m_actionA.bindings[0].effectivePath);
        }

        [Test]
        public void ResetBinding_Fires_OnBindingChanged_WithCorrectActionAndIndex()
        {
            int callCount = 0;
            InputAction receivedAction = null;
            int receivedIndex = int.MinValue;
            m_manager.OnBindingChanged += (a, i) => { callCount++; receivedAction = a; receivedIndex = i; };
            m_actionA.ApplyBindingOverride(0, "<Keyboard>/z");

            m_manager.ResetBinding(m_actionA, 0);

            Assert.AreEqual(1, callCount);
            Assert.AreSame(m_actionA, receivedAction);
            Assert.AreEqual(0, receivedIndex);
        }

        [Test]
        public void ResetBinding_DoesNotThrow_WhenActionIsNull()
        {
            Assert.DoesNotThrow(() => m_manager.ResetBinding(null, 0));
        }

        [Test]
        public void ResetBinding_DoesNotThrow_WhenBindingIndexIsOutOfRange()
        {
            Assert.DoesNotThrow(() => m_manager.ResetBinding(m_actionA, 999));
        }

        #endregion

        #region ResetAllBindings

        [Test]
        public void ResetAllBindings_RemovesAllOverrides()
        {
            m_actionA.ApplyBindingOverride(0, "<Keyboard>/z");
            m_actionB.ApplyBindingOverride(0, "<Keyboard>/z");

            m_manager.ResetAllBindings();

            Assert.AreEqual("<Keyboard>/a", m_actionA.bindings[0].effectivePath);
            Assert.AreEqual("<Keyboard>/b", m_actionB.bindings[0].effectivePath);
        }

        [Test]
        public void ResetAllBindings_Fires_OnBindingChanged_WithNullAction_AndMinusOneIndex()
        {
            int callCount = 0;
            InputAction receivedAction = new InputAction();
            int receivedIndex = int.MinValue;
            m_manager.OnBindingChanged += (a, i) => { callCount++; receivedAction = a; receivedIndex = i; };

            m_manager.ResetAllBindings();

            Assert.AreEqual(1, callCount);
            Assert.IsNull(receivedAction);
            Assert.AreEqual(-1, receivedIndex);
        }

        #endregion

        #region Save/Load

        [Test]
        public void SaveBindings_WritesJson_ToAccessibilitySettings()
        {
            m_actionA.ApplyBindingOverride(0, "<Keyboard>/z");

            m_manager.SaveBindings();

            Assert.IsFalse(string.IsNullOrEmpty(m_settings.rebindableKeys));
            Assert.DoesNotThrow(() => m_asset.LoadBindingOverridesFromJson(m_settings.rebindableKeys));
        }

        [Test]
        public void LoadBindings_RestoresOverride_FromAccessibilitySettings()
        {
            m_actionA.ApplyBindingOverride(0, "<Keyboard>/z");
            m_manager.SaveBindings();
            m_manager.ResetAllBindings();

            m_manager.LoadBindings();

            Assert.AreEqual("<Keyboard>/z", m_actionA.bindings[0].effectivePath);
        }

        [Test]
        public void LoadBindings_DoesNotThrow_WhenRebindableKeysIsEmpty()
        {
            m_settings.rebindableKeys = string.Empty;

            Assert.DoesNotThrow(() => m_manager.LoadBindings());
        }

        [Test]
        public void SaveLoad_RoundTrips_BindingOverride()
        {
            m_actionA.ApplyBindingOverride(0, "<Keyboard>/z");

            m_manager.SaveBindings();
            m_manager.ResetAllBindings();
            m_manager.LoadBindings();

            Assert.AreEqual("<Keyboard>/z", m_actionA.bindings[0].effectivePath);
            Assert.AreEqual("<Keyboard>/b", m_actionB.bindings[0].effectivePath);
        }

        #endregion

        #region Conflict Resolution

        [Test]
        public void ResolveConflictBlock_RestoresActionA_ToDefaultPath()
        {
            var conflict = ArrangeConflict();

            m_manager.ResolveConflictBlock(conflict);

            Assert.AreEqual("<Keyboard>/a", m_actionA.bindings[0].effectivePath);
            Assert.AreEqual("<Keyboard>/b", m_actionB.bindings[0].effectivePath);
        }

        [Test]
        public void ResolveConflictClear_RemovesConflictingBinding_AndAssignsPathToAction()
        {
            var conflict = ArrangeConflict();

            m_manager.ResolveConflictClear(conflict);

            Assert.AreEqual("<Keyboard>/b", m_actionA.bindings[0].effectivePath);
            Assert.AreEqual("", m_actionB.bindings[0].effectivePath);
        }

        [Test]
        public void ResolveConflictClear_Fires_OnBindingChanged_ForBothActions()
        {
            var calls = new List<InputAction>();
            m_manager.OnBindingChanged += (a, i) => calls.Add(a);
            var conflict = ArrangeConflict();

            m_manager.ResolveConflictClear(conflict);

            Assert.AreEqual(2, calls.Count);
            Assert.Contains(m_actionB, calls);
            Assert.Contains(m_actionA, calls);
        }

        [Test]
        public void ResolveConflictSwap_ExchangesBindings()
        {
            var conflict = ArrangeConflict();

            m_manager.ResolveConflictSwap(conflict);

            Assert.AreEqual("<Keyboard>/b", m_actionA.bindings[0].effectivePath);
            Assert.AreEqual("<Keyboard>/a", m_actionB.bindings[0].effectivePath);
        }

        [Test]
        public void ResolveConflictSwap_Fires_OnBindingChanged_ForBothActions()
        {
            var calls = new List<InputAction>();
            m_manager.OnBindingChanged += (a, i) => calls.Add(a);
            var conflict = ArrangeConflict();

            m_manager.ResolveConflictSwap(conflict);

            Assert.AreEqual(2, calls.Count);
            Assert.Contains(m_actionB, calls);
            Assert.Contains(m_actionA, calls);
        }

        #endregion
    }
}
