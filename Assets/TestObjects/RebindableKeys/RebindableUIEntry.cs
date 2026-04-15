using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace EasyAccessibility
{
    public class RebindableUIEntry : MonoBehaviour
    {
        public UnityEvent onRebindStarted = new();
        public UnityEvent onRebindFinished = new();

        InputActionReference reference;

        [SerializeField] TMP_Text action;
        [SerializeField] TMP_Text binding;
        [SerializeField] Button rebindButton;
        [SerializeField] Button resetButton;



        
        private void StartRebind()
        {
            ToggleInput(false);

            reference.action.PerformInteractiveRebinding()
                .OnComplete(operation =>
                {
                    ToggleInput(true);
                    SetInformation(reference);
                    onRebindFinished.Invoke();
                    Debug.Log(operation);
                });
        }

        private void ToggleInput(bool enabled)
        {
            if (enabled)
                reference.action.actionMap.Enable();
            else
                reference.action.actionMap.Disable();
        }




        public void Start()
        {
            rebindButton.onClick.AddListener(OnRebindClicked);
            resetButton.onClick.AddListener(OnResetClicked);
        }

        public void SetInformation(InputActionReference inputAction)
        {
            reference = inputAction;

            action.text = reference.action.name;
            binding.text = reference.action.bindings[1].ToDisplayString();
        }

        public void OnRebindClicked()
        {
            StartRebind();
        }

        public void OnResetClicked()
        {
            reference.action.RemoveAllBindingOverrides();
            SetInformation(reference);
        }
    }
}