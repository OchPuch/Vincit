using Player.Data;
using Player.States.DefaultState.Airborne;
using Player.States.DefaultState.Grounded;
using StateMachine;

namespace Player.States.DefaultState.Transitions
{
    public class DefaultEnteringState : DefaultState
    {
        public DefaultEnteringState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData)
            : base(controller, stateMachine, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if (PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                StateMachine.SwitchState<DefaultGroundedState>();
            }
            else
            {
                StateMachine.SwitchState<DefaultAirborneState>();
            }
        }
    }
}