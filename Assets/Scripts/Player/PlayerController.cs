using GlobalManagers;
using KinematicCharacterController;
using Player.AdditionalPhysics;
using Player.Data;
using Player.States;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, ICharacterController
    {
        
        public struct PlayerCharacterInputs
        {
            public float MoveAxisForward;
            public float MoveAxisRight;
            public Quaternion CameraRotation;
            public bool JumpDown;
            public bool CrouchDown;
            public bool CrouchUp;
            public bool DashDown;
        }

        [Header("Additional Physics")] 
        [SerializeField] private float timeStopGravityScale = 0.5f;
        [SerializeField] private WallDetector wallDetector;
        public PlayerData PlayerData => _playerData;
        private PlayerData _playerData;
        
        private PlayerStateMachine _stateMachine;
        public global::Player.StateMachine.StateMachine StateMachine => _stateMachine;

        public void Init(PlayerData data)
        {
            _playerData = data;
            _playerData.motor.CharacterController = this;
            _stateMachine = new PlayerStateMachine(this, _playerData);
            wallDetector.Init(_playerData, _stateMachine);
            TimeManager.Instance.TimeStopped += OnTimeStopped;
            TimeManager.Instance.TimeContinued += OnTimeContinued;
        }
        private void OnTimeContinued()
        {
            _playerData.gravity /= timeStopGravityScale;
        }

        private void OnTimeStopped()
        {
            _playerData.gravity *= timeStopGravityScale;
        }

        public void SetGravity(Vector3 newGravity)
        {
            if (TimeManager.Instance.IsTimeStopped) 
                newGravity *= timeStopGravityScale;
            
            _playerData.gravity = newGravity;
        }

        #region StateMachineCalls
        
        private void LateUpdate()
        {
            _stateMachine.Update();
        }

        public void SetInputs(ref PlayerCharacterInputs newInputs)
        {
            _stateMachine.SetInputs(ref newInputs);
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            _stateMachine.UpdateRotation(ref currentRotation, deltaTime);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            _stateMachine.UpdateVelocity(ref currentVelocity, deltaTime);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            _stateMachine.BeforeCharacterUpdate(deltaTime);
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            _stateMachine.PostGroundingUpdate(deltaTime);
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            _stateMachine.AfterCharacterUpdate(deltaTime);
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return _stateMachine.IsColliderValidForCollisions(coll);
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            _stateMachine.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            _stateMachine.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            _stateMachine.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition,
                atCharacterRotation, ref hitStabilityReport);
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            _stateMachine.OnDiscreteCollisionDetected(hitCollider);
        }
        
        #endregion

        
    }
}