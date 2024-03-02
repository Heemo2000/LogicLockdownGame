using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Core;

namespace Game.PlayerManagement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {        
        
        [SerializeField]private Transform followCamera;
        [Header("Movement Settings: ")]
        [Min(0.0f)]
        [SerializeField]private float moveSpeed = 20.0f;
        

        [Range(0.1f,1.0f)]
        [SerializeField]private float turnSmoothTime = 0.1f;

        [Header("Gravity Settings: ")]

        [Min(0f)]
        [SerializeField]private float jumpTime = 0.5f;
    
        [SerializeField]private Transform groundCheck;
        
        [Range(0.05f,0.5f)]
        [SerializeField]private float groundCheckDist = 0.1f;

        [Range(0.1f,1.0f)]
        [SerializeField]private float maxGroundCheckDist = 0.5f;
        [Min(0f)]
        [SerializeField]private float jumpHeight = 10f;

        [Min(0.0f)]
        [SerializeField]private float fallMultiplier = 1.0f;
        [SerializeField]private LayerMask groundMask;
        [Header("Animation Settings: ")]
        [SerializeField]private Animator playerAnimator;

        

        private CharacterController _controller;

        private Vector2 _moveInput = Vector2.zero;
        
        private float _turnSmoothVelocity = 0.0f;
        private float _gravity;
        private float _initialJumpVelocity;
        private float _velocityY;

        public Transform FollowCamera { get => followCamera; set => followCamera = value; }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if(context.canceled)
            {
                _moveInput.x = 0.0f;
                _moveInput.y = 0.0f;
                return;
            }


            _moveInput = context.ReadValue<Vector2>();

        }

        public void OnJumpPressed(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Jump();
            }
        }

        

        private void CalculateParameters()
        {
            float halfJumpTime = jumpTime/2f;
            _gravity = (-2f * jumpHeight)/(halfJumpTime * halfJumpTime);
            _initialJumpVelocity = 2f * jumpHeight/halfJumpTime;
        }

        private void Jump()
        {
            if(!IsGrounded())
            {
                return;
            }

            
            Debug.Log("Jumping");
            playerAnimator.SetTrigger("jump_trigger");
            _velocityY += _initialJumpVelocity;
        }
        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, groundCheckDist, groundMask.value);
        }

        private bool AlmostOnGround()
        {
            return Physics.CheckSphere(groundCheck.position, maxGroundCheckDist, groundMask.value);     
        }
        
        private void HandleGravity()
        {
            bool isFalling = _velocityY < 0.0f;
    
            if(!IsGrounded())
            {
                float currentVelocityY = _velocityY;
                float currentGravity = _gravity;
                
                if(isFalling)
                {
                    playerAnimator.SetBool("is_falling",true);
                    currentGravity *= fallMultiplier;
                }
                
                float newVelocityY = currentVelocityY + (currentGravity * Time.fixedDeltaTime);
                float nextVelocityY = (currentVelocityY + newVelocityY) * 0.5f;
                _velocityY = nextVelocityY;
    
            }
            else
            {
                _velocityY = 0f;
            }
    
            
            if(AlmostOnGround() && isFalling)
            {    
                playerAnimator.SetBool("is_falling",false);
            }
            
        }

        private void HandleMovement()
        {
            _controller.Move(Vector3.up * _velocityY * Time.fixedDeltaTime);

            Vector3 direction = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;
            if(direction.sqrMagnitude < 0.01f)
            {
                playerAnimator.SetFloat("move_input", 0.0f, 0.1f, Time.fixedDeltaTime);
                return;
            }   


            playerAnimator.SetFloat("move_input", 1.0f, 0.1f, Time.fixedDeltaTime);
            //Debug.Log("Follow camera angle: " + followCamera.transform.eulerAngles.y);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + followCamera.eulerAngles.y;
            
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, smoothedAngle, 0.0f);
            

            Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            moveDir.Normalize();
            _controller.Move(moveDir * moveSpeed * Time.fixedDeltaTime);
        }

        
        

        private void Awake() {
            _controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate() {
            if(GameManager.Instance.GamePauseStatus != GamePauseStatus.Paused)
            {
                CalculateParameters();
            
                HandleMovement();
                HandleGravity();
            }
        }

        private void OnDrawGizmos() 
        {
            if(groundCheck != null)
            {
                
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckDist);
                
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundCheck.position, maxGroundCheckDist);
                
            }
        }
    }
}
