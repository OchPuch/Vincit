using Player.States.DefaultState.Airborne;
using Player.States.DefaultState.Grounded;
using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Special
{
    public class DefaultDashState : DefaultState
    {
        private float _dashTime;
        private Vector3 _dashDirection;

        public DefaultDashState(CharacterController controller, IStateSwitcher stateMachine, PlayerData playerData) :
            base(controller, stateMachine, playerData)
        {
            PlayerData.currentDashEnergy = PlayerData.playerConfig.miscData.dashMaxEnergy;
        }

        public override void Enter()
        {
            base.Enter();
            _dashTime = 0;
            PlayerData.slamStorage = 0;
            PlayerData.currentDashEnergy -= PlayerData.playerConfig.miscData.dashCost;

            //if no move input, dash forward
            if (PlayerData.moveInputVector.magnitude < PlayerData.playerConfig.miscData.dashDirectionByInputThreshold)
            {
                _dashDirection = PlayerData.motor.CharacterForward;
            }
            else
            {
                _dashDirection = PlayerData.moveInputVector.normalized;
            }

            PlayerData.playerMovementAudio.PlayDashSound();
            
            PlayerData.dashCrushPoint.position = PlayerData.meshRoot.position + Vector3.up + _dashDirection.normalized;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            _dashTime += deltaTime;
            PlayerData.dashGun.Shoot();
            if (_dashTime > PlayerData.playerConfig.miscData.dashDuration)
            {
                currentVelocity = _dashDirection * PlayerData.playerConfig.airMovementData.maxAirMoveSpeed;
                if (PlayerData.motor.GroundingStatus.IsStableOnGround)
                {
                    StateMachine.SwitchState<DefaultGroundedState>();
                }
                else
                {
                    StateMachine.SwitchState<DefaultAirborneState>();
                }

                return;
            }

            currentVelocity = _dashDirection * PlayerData.playerConfig.miscData.dashSpeed;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}