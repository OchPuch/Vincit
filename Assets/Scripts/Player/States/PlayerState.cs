using System;
using KinematicCharacterController;
using Player.Data;
using Player.StateMachine;
using StateMachine;
using UnityEngine;


namespace Player.States
{
    [Serializable]
    public abstract class PlayerState : IState
    {
        protected readonly PlayerController PlayerController;
        protected readonly IStateSwitcher StateMachine;
        protected readonly PlayerData PlayerData;

        public PlayerState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData)
        {
            PlayerController = controller;
            this.StateMachine = stateMachine;
            this.PlayerData = playerData;
        }
        
        public virtual void Enter()
        {
            //Debug.Log($"Entered {GetType().Name}");
        }
        
        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
            
        }
        
        public virtual void SetInputs(ref PlayerController.PlayerCharacterInputs newInputs)
        {
            
        }

        public virtual void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
        }

        public virtual void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
        }

        public virtual void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public virtual void PostGroundingUpdate(float deltaTime)
        {
        }

        public virtual void AfterCharacterUpdate(float deltaTime)
        {
        }

        public virtual bool IsColliderValidForCollisions(Collider coll)
        {
            if (PlayerData.IgnoredColliders.Count == 0)
            {
                return true;
            }

            return !PlayerData.IgnoredColliders.Contains(coll);
        }

        public virtual void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public virtual void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public virtual void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        
    }
}