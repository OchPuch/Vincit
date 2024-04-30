using Player.Data;
using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Airborne
{
    public class DefaultFallingState : DefaultAirborneState
    {
        public DefaultFallingState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }
        
        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);
            var projectOnGravity = Vector3.Project(PlayerData.motor.Velocity, -PlayerData.gravity);
            if (projectOnGravity.normalized + PlayerData.gravity.normalized == Vector3.zero)
            {
                StateMachine.SwitchState<DefaultFlyingState>();
            }
            
        }
    }
}