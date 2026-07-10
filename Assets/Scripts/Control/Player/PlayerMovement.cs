using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;

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
    

        private PlayerController _playerInput;
        private Rigidbody _characterRigidbody;
        private Vector2 _moveInput;
        //private Vector2 lookInput;
        private Vector3 _characterMove;
        
        [Header("Movement")]

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float rotationSpeed = 180f; // degrees per second
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float deceleration = 10f;

        [Header("Checks")]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float groundCheckDistance = 1f;
        
        [Header("Look")]
        [SerializeField] private float lookSensitivity = 1f;
        //private CinemachineOrbitalFollow cameraFollow;

        [Header("Debug")]
        private bool _isGrounded;
        [SerializeField] private bool checkGrounded = true;
        [SerializeField] private bool checkJumping = true;
        [SerializeField] private bool checkWall = true;



        private void Awake() 
        {
            _characterRigidbody = this.GetComponentInChildren<Rigidbody>();
            _characterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            //cameraFollow = FindAnyObjectByType<CinemachineOrbitalFollow>();

            _playerInput = new PlayerController();
            _playerInput.OverWorld.SetCallbacks(this);
        }

        private void OnDestroy()
        {
            _playerInput.OverWorld.RemoveCallbacks(this);
            _playerInput.Dispose();
        }
    
        private void OnEnable() => _playerInput.Enable();
        private void OnDisable() => _playerInput.Disable();

        void PlayerController.IOverWorldActions.OnMovement(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            Debug.Log("Move Input: " + _moveInput);
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
            //lookInput = context.ReadValue<Vector2>();
            //Debug.Log("Look Input: " + lookInput);
        }
        

        private void FixedUpdate() 
        {
            OnCheckGround();
            OnMove();
        }


        private void OnMove() 
        {
            
            Vector3 inputDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
    
            float targetSpeed = inputDirection != Vector3.zero ? maxSpeed : 0f;
            float rate = inputDirection != Vector3.zero ? acceleration : deceleration;

            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, rate * Time.fixedDeltaTime);

            _characterRigidbody.MovePosition(_characterRigidbody.position + inputDirection * (moveSpeed * Time.fixedDeltaTime));

            if (inputDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputDirection,  Vector3.up);
                Quaternion newRotation = Quaternion.RotateTowards(_characterRigidbody.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                _characterRigidbody.MoveRotation(newRotation);
            }
            
         
        }

        private void OnCheckGround()
        {
            _isGrounded = Physics.SphereCast(_characterRigidbody.position, 0.5f, Vector3.down, out RaycastHit hit, groundCheckDistance, groundMask);
            //Debug.Log("Is Grounded: " + isGrounded);

        }

        private void OnDrawGizmos()
        {
            if (_characterRigidbody == null) return;
            
            if (checkGrounded)
            {
                Gizmos.color = _isGrounded ? Color.green : Color.red;    
                Gizmos.DrawWireSphere(_characterRigidbody.position + Vector3.down * groundCheckDistance, 0.5f);   
            }
            
            if (checkWall)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_characterRigidbody.position + Vector3.forward * 1f, 1f);
            }
            
        }
    

    }
}