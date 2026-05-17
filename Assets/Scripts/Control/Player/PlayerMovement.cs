using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerMovement : MonoBehaviour, PlayerController.IOverWorldActions
    {
        
        private PlayerController playerInput;
        private Rigidbody characterRigidbody;

        private Vector2 moveInput;
        private Vector3 characterMove;
        //Checks
        [SerializeField] private LayerMask groundMask;
        private bool isGrounded = false;
        private float groundCheckDistance = 0.7f;


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
            
            if (isGrounded && context.started)
            {
                characterRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            }
            Debug.Log("Jump Input: " + context.started);
        }


        private void FixedUpdate() 
        {
            OnMove();
            OnCheckGround();
            
        }

        private void OnMove() //Primary Normal Movement Logic like walking/running
        {
            characterMove = new Vector3(moveInput.x, 0, moveInput.y);
            characterRigidbody.MovePosition(characterRigidbody.position + characterMove * 5 * Time.fixedDeltaTime);
        }

        private void OnCheckGround()
        {
            isGrounded = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit, groundCheckDistance, groundMask);
            Debug.Log("Is Grounded: " + isGrounded);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, 0.5f);
        }

    }
}