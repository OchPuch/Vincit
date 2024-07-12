using KinematicCharacterController;
using Player.Data;
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

        public DefaultDashState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData) :
            base(controller, stateMachine, playerData)
        {
            PlayerData.currentDashEnergy = PlayerData.playerConfig.MiscData.DashMaxEnergy;
        }
        public override void SetInputs(ref PlayerController.PlayerCharacterInputs newInputs)
        {
            base.SetInputs(ref newInputs);
            if (PlayerData.Inputs.CrouchDown)
            {
                if (PlayerData.motor.GroundingStatus.IsStableOnGround)
                {
                    StateMachine.SwitchState<DefaultSlideState>();
                }
            }
        }


        public override void Enter()
        {
            base.Enter();
            _dashTime = 0;
            PlayerData.slamStorage = 0;
            PlayerData.currentDashEnergy -= PlayerData.playerConfig.MiscData.DashCost;

            //if no move input, dash forward
            if (PlayerData.moveInputVector.magnitude < PlayerData.playerConfig.MiscData.DashDirectionByInputThreshold)
            {
                _dashDirection = PlayerData.motor.CharacterForward;
            }
            else
            {
                _dashDirection = PlayerData.moveInputVector.normalized;
            }

            PlayerData.playerMovementAudio.PlayDashSound();
            
            PlayerData.dashCrushPoint.position = PlayerData.dashGun.transform.position + _dashDirection.normalized;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            _dashTime += deltaTime;
            PlayerData.dashGun.Shoot();
            if (_dashTime > PlayerData.playerConfig.MiscData.DashDuration)
            {
                SetSpeedSlideBuffer();
                currentVelocity = _dashDirection * PlayerData.playerConfig.AirMovementData.MaxAirMoveSpeed;
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

            currentVelocity = _dashDirection * PlayerData.playerConfig.MiscData.DashSpeed;
        }
        
    }
}