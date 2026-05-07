using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    public class RebindableUI : MonoBehaviour
    {
        [SerializeField] GameObject firstObject;
        private InputAction m_menuAction;
        private InputActionMap m_playerMap;

        private void Start()
        {
            var actions = InputSystem.actions;
            m_playerMap = actions.FindActionMap("Player");

            m_menuAction = actions.FindAction("Menu");
            m_menuAction.performed += OnMenuToggled;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            m_menuAction.performed -= OnMenuToggled;
        }

        private void OnMenuToggled(InputAction.CallbackContext ctx)
        {
            var open = !gameObject.activeInHierarchy;
            gameObject.SetActive(open);

            if (open)
            {
                EventSystem.current.SetSelectedGameObject(firstObject);
                m_playerMap?.Disable();
            }
            else
            {
                m_playerMap?.Enable();
            }
        }
    }
}