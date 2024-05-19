using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager playerManager;
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAMount;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection = Vector3.zero;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if(playerManager.IsOwner)
            {
                playerManager.characterNetworkManager.verticalMovement.Value = verticalMovement;
                playerManager.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                playerManager.characterNetworkManager.moveAmount.Value = moveAMount;
            }
            else
            {
                moveAMount = playerManager.characterNetworkManager.moveAmount.Value;
                verticalMovement = playerManager.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = playerManager.characterNetworkManager.horizontalMovement.Value;

                playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAMount);
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            // Aerial movement
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManger.instance.verticalInput;
            horizontalMovement = PlayerInputManger.instance.horizontalInput;
            moveAMount = PlayerInputManger.instance.moveAmount;

            // CLAMP MOVEMENTS
        }

        private void HandleGroundedMovement()
        {
            GetMovementValues();

            Vector3 cameraForward = PlayerCamera.instance.transform.forward;
            Vector3 cameraRight = PlayerCamera.instance.transform.right;

            // Ensure the y component is 0 to prevent unwanted vertical movement
            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate move direction based on camera's forward and right directions
            moveDirection = cameraForward * verticalMovement + cameraRight * horizontalMovement;
            moveDirection.Normalize();

            if (PlayerInputManger.instance.moveAmount > 0.5f)
            {
                // MOVE AT A RUNNING SPEED
                playerManager.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else
            {
                // MOVE AT A WALKING SPEED
                playerManager.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if(targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }
}
