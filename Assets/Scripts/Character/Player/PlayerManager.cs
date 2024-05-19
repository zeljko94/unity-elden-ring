using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
                return;

            playerLocomotionManager.HandleAllMovement();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if(IsOwner)
            {
                PlayerCamera.instance.playerManager = this;
                PlayerInputManger.instance.playerManager = this;
            }
            transform.position = new Vector3(10, -2, 2.5f);
        }

        protected override void LateUpdate()
        {
            if(!IsOwner) 
                return;
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }
    }
}
