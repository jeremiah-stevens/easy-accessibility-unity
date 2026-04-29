using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace EasyAccessibility
{
    public class RebindableUIEntry : MonoBehaviour
    {
        [SerializeField] private RebindableAction m_rebindableAction;
        [SerializeField] private TMP_Text m_actionName;
        [SerializeField] private TMP_Text m_bindingDisplay;
        [SerializeField] private Button m_rebindButton;
        [SerializeField] private Button m_resetButton;

        private void Start()
        {
            m_actionName.text = m_rebindableAction.ActionName;
            m_bindingDisplay.text = m_rebindableAction.BindingDisplayString;

            m_rebindButton.onClick.AddListener(m_rebindableAction.StartRebind);
            m_resetButton.onClick.AddListener(m_rebindableAction.ResetBinding);

            m_rebindableAction.onDisplayStringChanged.AddListener(OnDisplayStringChanged);
            m_rebindableAction.onRebindStarted.AddListener(OnRebindStarted);
            m_rebindableAction.onRebindCompleted.AddListener(OnRebindFinished);
            m_rebindableAction.onRebindCancelled.AddListener(OnRebindFinished);
        }

        private void OnDestroy()
        {
            m_rebindButton.onClick.RemoveListener(m_rebindableAction.StartRebind);
            m_resetButton.onClick.RemoveListener(m_rebindableAction.ResetBinding);

            m_rebindableAction.onDisplayStringChanged.RemoveListener(OnDisplayStringChanged);
            m_rebindableAction.onRebindStarted.RemoveListener(OnRebindStarted);
            m_rebindableAction.onRebindCompleted.RemoveListener(OnRebindFinished);
            m_rebindableAction.onRebindCancelled.RemoveListener(OnRebindFinished);
        }

        private void OnDisplayStringChanged(string displayString)
        {
            m_bindingDisplay.text = displayString;
        }

        private void OnRebindStarted()
        {
            m_bindingDisplay.text = "Press any key...";
            m_rebindButton.interactable = false;
            m_resetButton.interactable = false;
        }

        private void OnRebindFinished()
        {
            m_rebindButton.interactable = true;
            m_resetButton.interactable = true;
        }
    }
}