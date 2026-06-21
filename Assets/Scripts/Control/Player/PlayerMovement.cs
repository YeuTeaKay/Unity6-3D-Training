using System;
using System.Collections;
using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace PlayerControl
{
    public class PlayerMovement : MonoBehaviour, PlayerController.IOverWorldActions
    {
        /// <summary>
        /// Things that need to be done in this script:
        /// 1. Acceleration and deceleration to the movement [Done]
        /// 2. Short jump and long jump based on how long the jump button is held [Ongoing]
        /// 3. Wall jump and wall slide mechanics []
        /// 4. Camera movement and rotation based on mouse input []
        /// </summary>
    

        private PlayerController playerInput;
        private Rigidbody characterRigidbody;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private Vector3 characterMove;
        
        [Header("Movement")]

        [SerializeField] private float moveSpeed = 0f;
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float deceleration = 10f;

        [Header("Checks")]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float groundCheckDistance = 1f;
        
        [Header("Look")]
        [SerializeField] private float lookSensitivity = 1f;
        private CinemachineOrbitalFollow cameraFollow;

        [Header("Debug")]
        private bool isGrounded = false;
        private bool isJumping = false;
        [SerializeField] private bool CheckGrounded = true;
        [SerializeField] private bool CheckJumping = true;
        [SerializeField] private bool CheckWall = true;



        private void Awake() 
        {
            characterRigidbody = this.GetComponentInChildren<Rigidbody>();
            cameraFollow = FindAnyObjectByType<CinemachineOrbitalFollow>();

            playerInput = new PlayerController();
            playerInput.OverWorld.SetCallbacks(this);
        }

        private void Start()
        {
            
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
            //Debug.Log("Move Input: " + moveInput);
        }


        void PlayerController.IOverWorldActions.OnJump(InputAction.CallbackContext context)
        {
            /*
            if (isGrounded && context.started)
            {
                characterRigidbody.AddForce(Vector3.up * 3, ForceMode.Impulse);
                //Debug.Log("Short Jump");
            }
            */
            
        }

        void PlayerController.IOverWorldActions.OnCamera(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
            //Debug.Log("Look Input: " + lookInput);
        }

        private void LateUpdate() 
        {
            PlayerLook();
        }

        private void FixedUpdate() 
        {
            OnCheckGround();
            OnMove();
        }


        private void OnMove() 
        {
            
            Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);

            float targetSpeed = inputDirection != Vector3.zero ? maxSpeed : 0f;
            float rate = inputDirection != Vector3.zero ? acceleration : deceleration;

            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, rate * Time.fixedDeltaTime);

            characterRigidbody.MovePosition(characterRigidbody.position + inputDirection * (moveSpeed * Time.fixedDeltaTime));
         
        }

        private void PlayerLook()
        {
        }

        private void ClampAngle(float angle, float min, float max)
        {
            
        }
        

        private IEnumerator JumpCooldown()
        {
            yield return new WaitForSeconds(0.3f);

        }

        private void OnCheckGround()
        {
            isGrounded = Physics.SphereCast(characterRigidbody.position, 0.5f, Vector3.down, out RaycastHit hit, groundCheckDistance, groundMask);
            //Debug.Log("Is Grounded: " + isGrounded);

        }

        private void OnDrawGizmos()
        {
            if (CheckGrounded)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;    
                Gizmos.DrawWireSphere(characterRigidbody.position + Vector3.down * groundCheckDistance, 0.5f);   
            }
            
            if (CheckWall)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(characterRigidbody.position + Vector3.forward * 1f, 1f);
            }
            
        }
    

    }
}