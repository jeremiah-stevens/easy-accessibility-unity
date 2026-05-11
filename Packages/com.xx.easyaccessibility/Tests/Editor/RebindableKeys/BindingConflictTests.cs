using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Object;

namespace EasyAccessibility.Tests
{
    public class BindingConflictTests
    {
        InputActionAsset m_asset;
        InputAction m_action;
        InputAction m_conflictingAction;

        [SetUp]
        public void SetUp()
        {
            m_asset = ScriptableObject.CreateInstance<InputActionAsset>();
            var map = m_asset.AddActionMap("TestMap");

            m_action = map.AddAction("JumpAction", InputActionType.Button);
            m_action.AddBinding("<Keyboard>/space");

            m_conflictingAction = map.AddAction("AttackAction", InputActionType.Button);
            m_conflictingAction.AddBinding("<Keyboard>/space");

            map.Enable();
        }

        [TearDown]
        public void TearDown()
        {
            DestroyImmediate(m_asset);
        }

        #region Helpers

        BindingConflict MakeConflict(InputAction action = null, InputAction conflicting = null) =>
            new BindingConflict
            {
                action = action ?? m_action,
                bindingIndex = 0,
                conflictingAction = conflicting ?? m_conflictingAction,
                conflictingBindingIndex = 0,
                oldPath = "<Keyboard>/j"
            };

        #endregion

        #region Public Properties

        [Test]
        public void PublicProperties_AreSetCorrectly()
        {
            var conflict = new BindingConflict
            {
                action = m_action,
                bindingIndex = 1,
                conflictingAction = m_conflictingAction,
                conflictingBindingIndex = 2,
                oldPath = "<Keyboard>/j"
            };

            Assert.AreSame(m_action, conflict.action);
            Assert.AreEqual(1, conflict.bindingIndex);
            Assert.AreSame(m_conflictingAction, conflict.conflictingAction);
            Assert.AreEqual(2, conflict.conflictingBindingIndex);
            Assert.AreEqual("<Keyboard>/j", conflict.oldPath);
        }

        #endregion

        #region GetConflictDescription

        [Test]
        public void GetConflictDescription_ReturnsNonEmptyString()
        {
            Assert.IsFalse(string.IsNullOrEmpty(MakeConflict().GetConflictDescription()));
        }

        [Test]
        public void GetConflictDescription_ContainsConflictingActionName()
        {
            Assert.That(MakeConflict().GetConflictDescription(), Does.Contain(m_conflictingAction.name));
        }

        [Test]
        public void GetConflictDescription_ContainsNewlyReboundActionName()
        {
            Assert.That(MakeConflict().GetConflictDescription(), Does.Contain(m_action.name));
        }

        [Test]
        public void GetConflictDescription_DoesNotThrow_WhenConflictingActionIsNull()
        {
            var conflict = MakeConflict(conflicting: null);

            Assert.DoesNotThrow(() => conflict.GetConflictDescription());
        }

        #endregion
    }
}
