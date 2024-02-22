

using Player.States.DefaultState.Airborne;
using Player.States.DefaultState.Grounded;
using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Special
{
    public class DefaultSlamingState : DefaultState
    {
        private Vector3 _slamStartPos;
        private Vector3 _slamEndPos;
        public DefaultSlamingState(CharacterController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerData.isSlaming = true;
            PlayerData.slamStorage = 0;
            _slamStartPos = PlayerData.motor.Transform.position;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            currentVelocity = PlayerData.gravity.normalized * PlayerData.playerConfig.miscData.slamingSpeed;
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);
            PlayerData.timeSinceLastAbleToJump += deltaTime;

        }

        public override void PostGroundingUpdate(float deltaTime)
        {
            base.PostGroundingUpdate(deltaTime);
            if (PlayerData.motor.GroundingStatus.FoundAnyGround)
            {
                PlayerData.playerMovementAudio.PlaySlamSound();
                StateMachine.SwitchState<DefaultGroundedState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            PlayerData.isSlaming = false;
            
            _slamEndPos = PlayerData.motor.Transform.position;
            PlayerData.slamStorageKeepTimer = 0;
            
            float distance = Vector3.Distance(_slamStartPos, _slamEndPos);
            PlayerData.slamStorage = distance * 2 / PlayerData.playerConfig.miscData.slamFlightBackMaxTime;
            PlayerData.slamStorage = Mathf.Clamp(PlayerData.slamStorage, 0,  PlayerData.playerConfig.miscData.maxSlamStorage);
        }
    }
}