using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] private InputActionReference moveAction;
        private Vector2 moveInput;

        private void Awake() 
        {
            moveAction.action.performed += OnMovePerformed;
            moveAction.action.canceled += OnMoveCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext context) 
        {
            moveInput = context.ReadValue<Vector2>();
            Debug.Log("Move Input: " + moveInput);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {

        }

        // Update is called once per frame
        private void FixedUpdate() 
        {

        }

    }
}