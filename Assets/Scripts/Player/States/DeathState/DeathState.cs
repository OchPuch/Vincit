

using Player.Data;
using StateMachine;

namespace Player.States.DeathState
{
    public class DeathState : PlayerState
    {
        public DeathState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }
    }
}