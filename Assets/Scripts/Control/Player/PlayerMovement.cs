using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerMovement : MonoBehaviour, PlayerController.IOverWorldActions
    {
        private PlayerController _input;

        private Rigidbody rb;
        private Vector2 moveInput;
        private Vector3 characterMove;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            _input = new PlayerController();
            _input.OverWorld.SetCallbacks(this);
        }

        private void OnDestroy()
        {
            _input.OverWorld.RemoveCallbacks(this);
            _input.Dispose();
        }
    
        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();

        // Update is called once per frame

        void PlayerController.IOverWorldActions.OnMovement(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            Debug.Log("Move Input: " + moveInput);
        }
        private void FixedUpdate() 
        {
            OnMove();
            rb.MovePosition(rb.position + characterMove * 5 * Time.fixedDeltaTime);
        }

        private void OnMove()
        {
            characterMove = new Vector3(moveInput.x, 0, moveInput.y);
        }
    }
}