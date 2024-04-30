using Player.Data;
using Player.States.DefaultState.Grounded;
using Player.States.DefaultState.Special;
using Player.States.DefaultState.Transitions;
using StateMachine;
using UnityEngine;

namespace Player.States.DefaultState.Airborne
{
    public class DefaultAirborneState : DefaultState
    {
        public DefaultAirborneState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData)
            : base(controller, stateMachine, playerData)
        {
        }
        
        public override void SetInputs(ref PlayerController.PlayerCharacterInputs newInputs)
        {
            base.SetInputs(ref newInputs);
            if (PlayerData.Inputs.CrouchDown)
            {
                StateMachine.SwitchState<DefaultSlamingState>();
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            if (PlayerData.moveInputVector.sqrMagnitude > 0f)
            {
                Vector3 addedVelocity = PlayerData.moveInputVector * (PlayerData.playerConfig.AirMovementData.airAccelerationSpeed * deltaTime);

                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, PlayerData.motor.CharacterUp);

                // Limit air velocity from inputs
                if (currentVelocityOnInputsPlane.magnitude < PlayerData.playerConfig.AirMovementData.maxAirMoveSpeed)
                {
                    // clamp addedVel to make total vel not exceed max vel on inputs plane
                    Vector3 newTotal =
                        Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, PlayerData.playerConfig.AirMovementData.maxAirMoveSpeed);
                    addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    
                }
                else
                {
                    // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                    if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                    {
                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                    }
                }

                // Prevent air-climbing sloped walls
                if (PlayerData.motor.GroundingStatus.FoundAnyGround)
                {
                    if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3
                            .Cross(Vector3.Cross(PlayerData.motor.CharacterUp, PlayerData.motor.GroundingStatus.GroundNormal),
                                PlayerData.motor.CharacterUp).normalized;
                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                    }
                }

                // Apply added velocity
                currentVelocity += addedVelocity;
            }
            
            // Gravity
            currentVelocity += PlayerData.gravity * deltaTime;

            // Drag
            currentVelocity *= (1f / (1f + (PlayerData.playerConfig.AirMovementData.drag * deltaTime)));
            
            
            WallJump(ref currentVelocity, deltaTime);
            
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);
            // Keep track of time since we were last able to jump (for grace period)
            PlayerData.timeSinceLastAbleToJump += deltaTime;
        }

        public override void PostGroundingUpdate(float deltaTime)
        {
            base.PostGroundingUpdate(deltaTime);
            if (PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                StateMachine.SwitchState<DefaultGroundedState>();
                PlayerData.playerMovementAudio.PlayLandSound();
            }
        }
    }
}