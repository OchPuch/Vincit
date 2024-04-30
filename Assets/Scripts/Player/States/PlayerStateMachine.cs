using System.Collections.Generic;
using KinematicCharacterController;
using Player.Data;
using Player.StateMachine;
using Player.States.DefaultState.Airborne;
using Player.States.DefaultState.Grounded;
using Player.States.DefaultState.Special;
using Player.States.DefaultState.Transitions;
using StateMachine;
using UnityEngine;

namespace Player.States
{
    public class PlayerStateMachine : global::Player.StateMachine.StateMachine, IStateSwitcher
    {
        private PlayerState PlayerState => (PlayerState)currentState;

        public PlayerStateMachine(PlayerController playerController, PlayerData playerData)
        {
            states = new List<IState>(new List<PlayerState>()
            {
                //Transition
                new DefaultEnteringState(playerController, this, playerData),
                //Airborne
                new DefaultFallingState(playerController, this, playerData),
                new DefaultFlyingState(playerController, this, playerData), 
                //Grounded                
                new DefaultRunState(playerController, this, playerData),
                new DefaultIdleState(playerController, this, playerData),
                //Special
                new DefaultSlideState(playerController, this, playerData),
                new DefaultSlamingState(playerController, this, playerData),
                new DefaultDashState(playerController, this, playerData),
                //Death
                new DeathState.DeathState(playerController, this, playerData),
            });

            currentState = states[0];
            currentState.Enter();
        }


        public void SwitchState<TState>() where TState : IState
        {
            if (currentState is TState) return;
            PlayerState newState = (PlayerState)states.Find(state => state is TState);
            currentState.Exit();
            OnStateExited(currentState);
            currentState = newState;
            currentState.Enter();
            OnStateEntered(currentState);
        }

        public void Update()
        {
            PlayerState.Update();
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            PlayerState.UpdateRotation(ref currentRotation, deltaTime);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            PlayerState.UpdateVelocity(ref currentVelocity, deltaTime);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            PlayerState.BeforeCharacterUpdate(deltaTime);
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            PlayerState.PostGroundingUpdate(deltaTime);
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            PlayerState.AfterCharacterUpdate(deltaTime);
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return PlayerState.IsColliderValidForCollisions(coll);
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            PlayerState.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            PlayerState.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            PlayerState.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition,
                atCharacterRotation, ref hitStabilityReport);
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            PlayerState.OnDiscreteCollisionDetected(hitCollider);
        }

        public void SetInputs(ref PlayerController.PlayerCharacterInputs newInputs)
        {
            PlayerState.SetInputs(ref newInputs);
        }
    }
}