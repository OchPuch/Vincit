

using StateMachine;

namespace Player.States.DeathState
{
    public class DeathState : PlayerState
    {
        public DeathState(CharacterController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }
    }
}