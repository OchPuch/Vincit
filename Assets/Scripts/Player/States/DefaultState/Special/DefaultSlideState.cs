using Player.States.DefaultState.Airborne;
using Player.States.DefaultState.Grounded;
using StateMachine;
using UnityEngine;
using Utils;

namespace Player.States.DefaultState.Special
{
    public class DefaultSlideState : DefaultState
    {
        
        private bool _stopped;

        public DefaultSlideState(CharacterController controller, IStateSwitcher stateMachine, PlayerData playerData) :
            base(controller, stateMachine, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerData.isSliding = true;
            PlayerData.motor.SetCapsuleDimensions(0.5f,
                PlayerData.playerConfig.miscData.crouchedCapsuleHeight,
                PlayerData.playerConfig.miscData.crouchedCapsuleHeight * 0.5f);
            PlayerData.meshRoot.localScale = new Vector3(1f, 0.5f, 1f);
            _stopped = false;
            
            PlayerData.playerMovementAudio.StartSlideLoop();
            
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);
            
            if (PlayerData.motor.GroundingStatus.IsStableOnGround && !_stopped)
            {
                Vector3 velocityOnPlane = Vector3.ProjectOnPlane(currentVelocity, PlayerData.motor.GroundingStatus.GroundNormal);
                
                if (velocityOnPlane.magnitude > PlayerData.playerConfig.slidingData.slidingDirectionByCurrentVelocityThreshold)
                {
                    currentVelocity = velocityOnPlane;
                    if (currentVelocity.magnitude <= PlayerData.playerConfig.slidingData.minSlidingSpeed)
                    {
                        currentVelocity = currentVelocity.normalized *
                                          PlayerData.playerConfig.slidingData.minSlidingSpeed;
                    }
                }
                else 
                {
                    var lookVector = PlayerData.motor.CharacterForward;
                    currentVelocity = Vector3.ProjectOnPlane(lookVector, PlayerData.motor.GroundingStatus.GroundNormal)
                            .normalized * PlayerData.playerConfig.slidingData.minSlidingSpeed;
                    velocityOnPlane = Vector3.ProjectOnPlane(currentVelocity, PlayerData.motor.GroundingStatus.GroundNormal);
                }
                
                //Gravity helps
                Vector3 gravityHelp = Vector3.Project(PlayerData.gravity, currentVelocity);
                if (VectorUtils.AreCodirected(currentVelocity, gravityHelp))
                {
                    currentVelocity += gravityHelp * (PlayerData.playerConfig.slidingData.gravityHelpK * deltaTime);
                }
                
                if (velocityOnPlane.magnitude <= PlayerData.playerConfig.slidingData.slidingStopThreshold)
                {
                    _stopped = true;
                }
            }

            // Acceleration to side
            float currentVelocityMagnitude = currentVelocity.magnitude;
            if (!_stopped)
            {
                Vector3 additionalVelocity = PlayerData.motor.CharacterRight * (PlayerData.playerConfig.slidingData.slidingAccelerationByInput * PlayerData.Inputs.MoveAxisRight);
                Vector3 addForwardPart = Vector3.Project(additionalVelocity, currentVelocity);
                additionalVelocity -= addForwardPart;
                currentVelocity += additionalVelocity * deltaTime;
                currentVelocity = currentVelocity.normalized * currentVelocityMagnitude;
                
            }

            if (!PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                // Gravity
                currentVelocity += PlayerData.gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (PlayerData.playerConfig.airMovementData.drag * deltaTime)));
            }
            else
            {
                Jump(ref currentVelocity, deltaTime);
            }
            
            WallJump(ref currentVelocity, deltaTime);
            
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);
            if (!PlayerData.shouldBeSliding)
            {
                if (PlayerData.motor.GroundingStatus.IsStableOnGround)
                {
                    StateMachine.SwitchState<DefaultGroundedState>();
                }
                else
                {
                    StateMachine.SwitchState<DefaultAirborneState>();
                }
            }

            if (PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                if (!PlayerData.jumpedThisFrame)
                {
                    PlayerData.jumpConsumed = false;
                }

                PlayerData.timeSinceLastAbleToJump = 0f;
                PlayerData.wallJumpCount = 0;
            }
        }

        public override void Exit()
        {
            base.Exit();
            PlayerData.isSliding = false;
            PlayerData.motor.SetCapsuleDimensions(0.5f, 2f, 1f);
            if (PlayerData.motor.CharacterOverlap(
                    PlayerData.motor.TransientPosition,
                    PlayerData.motor.TransientRotation,
                    PlayerData.probedColliders,
                    PlayerData.motor.CollidableLayers,
                    QueryTriggerInteraction.Ignore) > 0)
            {
                // If obstructions, just stick to crouching dimensions
                PlayerData.motor.SetCapsuleDimensions(0.5f, PlayerData.playerConfig.miscData.crouchedCapsuleHeight,
                    PlayerData.playerConfig.miscData.crouchedCapsuleHeight * 0.5f);
            }
            else
            {
                // If no obstructions, uncrouch
                PlayerData.meshRoot.localScale = new Vector3(1f, 1f, 1f);
                PlayerData.isSliding = false;
            }
            
            PlayerData.playerMovementAudio.StopSlideLoop();
        }
    }
}