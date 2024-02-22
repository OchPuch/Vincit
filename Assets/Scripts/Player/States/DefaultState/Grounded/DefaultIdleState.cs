using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Grounded
{
    public class DefaultIdleState : DefaultGroundedState
    {
        public DefaultIdleState(CharacterController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }
        

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            if (currentVelocity.magnitude > 0.1f)
            {
                StateMachine.SwitchState<DefaultRunState>();
            }
        }
    }
}