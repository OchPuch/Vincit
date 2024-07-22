
using Player.Data;
using Player.States.DefaultState.Airborne;
using Player.States.DefaultState.Special;
using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Grounded
{
    public class DefaultGroundedState : DefaultState
    {
        public DefaultGroundedState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(controller, stateMachine, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerData.motor.SetCapsuleDimensions(0.5f, 2f, 1f);
            if (PlayerData.motor.CharacterOverlap(
                    PlayerData.motor.TransientPosition,
                    PlayerData.motor.TransientRotation,
                    PlayerData.probedColliders,
                    PlayerData.motor.CollidableLayers,
                    QueryTriggerInteraction.Ignore) > 0)
            {
                // If obstructions, just stick to crouching dimensions
                PlayerData.motor.SetCapsuleDimensions(0.5f, PlayerData.playerConfig.MiscData.CrouchedCapsuleHeight, PlayerData.playerConfig.MiscData.CrouchedCapsuleHeight * 0.5f);
            }
            else
            {
                // If no obstructions, uncrouch
                PlayerData.meshRoot.localScale = new Vector3(1f, 1f, 1f);
                PlayerData.isSliding = false;
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);

            if (PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = PlayerData.motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = PlayerData.motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(PlayerData.moveInputVector, PlayerData.motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * PlayerData.moveInputVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * PlayerData.playerConfig.StableMovementData.MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-PlayerData.playerConfig.StableMovementData.StableMovementSharpness * deltaTime));
            }
            
            Jump(ref currentVelocity, deltaTime);
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);
            // If we're on a ground surface, reset jumping values
            if (!PlayerData.jumpedThisFrame)
            {
                PlayerData.jumpConsumed = false;
            }
            
            PlayerData.timeSinceLastAbleToJump = 0f;
            PlayerData.wallJumpCount = 0;
        }

        public override void PostGroundingUpdate(float deltaTime)
        {
            base.PostGroundingUpdate(deltaTime);
            if (!PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                StateMachine.SwitchState<DefaultAirborneState>();
            }
            else if (PlayerData.shouldBeSliding)
            {
                StateMachine.SwitchState<DefaultSlideState>();
            }
        }
    }
}