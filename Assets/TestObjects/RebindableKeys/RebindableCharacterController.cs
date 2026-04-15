using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    [RequireComponent(typeof(CharacterController))]
    public class RebindableCharacterController : MonoBehaviour
    {
        CharacterController characterController;

        InputAction moveAction;
        InputAction jumpAction;

        [SerializeField] [Range(0f, 20f)] float moveSpeed = 1f;
        [SerializeField] [Range(0f, 1f)] float jumpSpeed = 0.5f;



        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        void Update()
        {
            //movement
            var input = moveAction.ReadValue<Vector2>();
            Vector3 movement = new Vector3(input.x, 0f, input.y);

            if(jumpAction.IsPressed())
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