

using Player.Data;
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
        public DefaultSlamingState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
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
            currentVelocity = PlayerData.gravity.normalized * PlayerData.playerConfig.SlamingData.SlamingSpeed;
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);
            PlayerData.timeSinceLastAbleToJump += deltaTime;
            if (PlayerData.motor.Velocity.magnitude < PlayerData.playerConfig.SlamingData.SlamingSpeed)
            {
                StateMachine.SwitchState<DefaultGroundedState>();
            }
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

        public override void ProcessPushRequest(PushRequest pushRequest,ref Vector3 currentVelocity, float deltaTime)
        {
            if (!pushRequest.forceUngroundOnPush) return;
            if (pushRequest.pushMode == ForceMode.Force) return;
            currentVelocity = Vector3.zero;
            base.ProcessPushRequests(ref currentVelocity, deltaTime);
            StateMachine.SwitchState<DefaultAirborneState>();
        }

        public override void Exit()
        {
            base.Exit();
            PlayerData.isSlaming = false;
            
            _slamEndPos = PlayerData.motor.Transform.position;
            PlayerData.slamStorageKeepTimer = 0;
            
            float distance = Vector3.Distance(_slamStartPos, _slamEndPos);
            PlayerData.slamStorage = distance * 2 / PlayerData.playerConfig.SlamingData.SlamFlightBackMaxTime;
            PlayerData.slamStorage = Mathf.Clamp(PlayerData.slamStorage, 0,  PlayerData.playerConfig.SlamingData.MaxSlamStorage);
        }
    }
}