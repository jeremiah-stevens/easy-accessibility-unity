using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    /*TODO:
     * - Enable/disable UI on Menu key pressed
     * - Dynamically populate actions (InputActionReference and binding)
     * 
     */
    public class RebindableUI : MonoBehaviour
    {
        InputAction menuAction;

        [Header("Rebindings")]
        [SerializeField] RectTransform entryParent;
        [SerializeField] RebindableUIEntry prefab;
        [SerializeField] List<InputActionReference> actions;




        private void InitializeEntries()
        {
            foreach(var curr in actions)
            {
                if (curr == null) continue;

                var instance = GameObject.Instantiate(prefab, entryParent);
                var entry = instance.GetComponent<RebindableUIEntry>();
                entry.SetInformation(curr);
            }
        }




        private void Start()
        {
            menuAction = InputSystem.actions.FindAction("Menu");
            menuAction.performed += OnMenuToggled;
            this.gameObject.SetActive(false);

            InitializeEntries();
        }

        private void OnDestroy()
        {
            menuAction.performed -= OnMenuToggled;
        }

        private void OnMenuToggled(InputAction.CallbackContext ctx)
        {
            this.gameObject.SetActive(!this.gameObject.activeInHierarchy);
        }
    }
}