﻿using Player.Data;
using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Grounded
{
    public class DefaultRunState : DefaultGroundedState
    {
        public DefaultRunState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerData.playerMovementAudio.StartFootstepSound();

        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            if (currentVelocity.magnitude < 0.1f)
            {
                StateMachine.SwitchState<DefaultIdleState>();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            PlayerData.playerMovementAudio.EndFootStepSound();
        }
    }
}