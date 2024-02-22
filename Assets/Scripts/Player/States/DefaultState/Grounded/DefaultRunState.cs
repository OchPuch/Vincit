using Player.States.DefaultState.Special;
using Player.States.DefaultState.Transitions;
using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Grounded
{
    public class DefaultRunState : DefaultGroundedState
    {
        public DefaultRunState(CharacterController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            if (currentVelocity.magnitude < 0.1f)
            {
                StateMachine.SwitchState<DefaultIdleState>();
            }
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);
            PlayerData.playerMovementAudio.UpdateFootstepSound();
        }
    }
}