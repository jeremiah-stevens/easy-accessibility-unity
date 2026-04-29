using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    public class RebindableUI : MonoBehaviour
    {
        private InputAction m_menuAction;

        private void Start()
        {
            m_menuAction = InputSystem.actions.FindAction("Menu");
            m_menuAction.performed += OnMenuToggled;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            m_menuAction.performed -= OnMenuToggled;
        }

        private void OnMenuToggled(InputAction.CallbackContext ctx)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
}