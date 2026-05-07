using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerMovement : MonoBehaviour, PlayerController.IOverWorldActions
    {
        private PlayerController playerInput;

        private Rigidbody characterRigidbody;
        private Vector2 moveInput;
        
        private bool jumpInput = false;
        private Vector3 characterMove;

        private void Awake() 
        {
            characterRigidbody = this.GetComponent<Rigidbody>();
            characterRigidbody.freezeRotation = true;

            playerInput = new PlayerController();
            playerInput.OverWorld.SetCallbacks(this);
        }

        private void OnDestroy()
        {
            playerInput.OverWorld.RemoveCallbacks(this);
            playerInput.Dispose();
        }
    
        private void OnEnable() => playerInput.Enable();
        private void OnDisable() => playerInput.Disable();

        void PlayerController.IOverWorldActions.OnMovement(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            Debug.Log("Move Input: " + moveInput);
        }

        void PlayerController.IOverWorldActions.OnJump(InputAction.CallbackContext context)
        {
            jumpInput = context.ReadValue<float>() > 0; //Idk if the float is one that needs to be check or might be sonething else.
            Debug.Log("Jump Input: " + jumpInput);
        }

        private void FixedUpdate() 
        {
            OnMove();
            OnJump();
            
        }

        private void OnMove() //Primary Normal Movement Logic like walking/running
        {
            characterMove = new Vector3(moveInput.x, 0, moveInput.y);
            characterRigidbody.MovePosition(characterRigidbody.position + characterMove * 5 * Time.fixedDeltaTime);
        }

        private void OnJump()
        {
            //TODO: Implement jump logic, including checking if the character is grounded and applying a vertical force to the Rigidbody when jumpInput is true.
        }

        
    }
}