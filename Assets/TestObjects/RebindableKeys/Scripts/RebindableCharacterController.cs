using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    [RequireComponent(typeof(CharacterController))]
    public class RebindableCharacterController : MonoBehaviour
    {
        CharacterController characterController;

        [SerializeField] InputActionReference moveAction;
        [SerializeField] InputActionReference jumpAction;

        [SerializeField] [Range(0f, 20f)] float moveSpeed = 1f;
        [SerializeField] [Range(0f, 1f)] float jumpSpeed = 0.5f;



        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            //movement
            var input = moveAction.action.ReadValue<Vector2>();
            Vector3 movement = new Vector3(input.x, 0f, input.y);

            if(jumpAction.action.IsPressed())
            {
                movement.y = jumpSpeed;
            }
            else
            {
                movement.y -= 9.8f * Time.deltaTime;
            }

            if (movement.magnitude > 0.005f)
            {
                characterController.Move(movement * moveSpeed);
            }
        }
    }
}