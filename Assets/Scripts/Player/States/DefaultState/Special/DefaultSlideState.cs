using KinematicCharacterController.Examples;
using Player.Data;
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
        private bool _startVelocitySet;
        private Vector3 _bufferedVelocity;

        public DefaultSlideState(PlayerController controller, IStateSwitcher stateMachine, PlayerData playerData) :
            base(controller, stateMachine, playerData)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerData.isSliding = true;
            PlayerData.motor.SetCapsuleDimensions(0.5f,
                PlayerData.playerConfig.MiscData.CrouchedCapsuleHeight,
                PlayerData.playerConfig.MiscData.CrouchedCapsuleHeight * 0.5f);
            PlayerData.meshRoot.localScale = new Vector3(1f, 0.5f, 1f);
            _stopped = false;
            _startVelocitySet = false;
            _bufferedVelocity = Vector3.zero;

            if (PlayerData.slideSpeedBufferApplyTimer > 0)
            {
                _bufferedVelocity = PlayerData.slideSpeedBuffer;
            }

            PlayerData.playerMovementAudio.StartSlideLoop();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);


            if (PlayerData.motor.GroundingStatus is { FoundAnyGround: true, IsStableOnGround: false })
            {
                PlayerData.bonusOrientationMethod = BonusOrientationMethod.TowardsGroundSlopeAndGravity;
            }
            

            if (PlayerData.motor.GroundingStatus.IsStableOnGround && !_stopped)
            {
                Vector3 currentVelocityOnPlane =
                    Vector3.ProjectOnPlane(currentVelocity, PlayerData.motor.GroundingStatus.GroundNormal);
                Vector3 bufferedVelocityOnPlane =
                    Vector3.ProjectOnPlane(_bufferedVelocity, PlayerData.motor.GroundingStatus.GroundNormal);

                Vector3 slidingSpeedOnPlane = currentVelocityOnPlane.magnitude > bufferedVelocityOnPlane.magnitude
                    ? currentVelocityOnPlane
                    : bufferedVelocityOnPlane;

                if (slidingSpeedOnPlane.magnitude > PlayerData.playerConfig.SlidingData.MaxSlidingSpeed)
                {
                    slidingSpeedOnPlane = slidingSpeedOnPlane.normalized *
                                          PlayerData.playerConfig.SlidingData.MaxSlidingSpeed;
                }

                if (slidingSpeedOnPlane.magnitude >
                    PlayerData.playerConfig.SlidingData.SlidingDirectionByCurrentVelocityThreshold)
                {
                    currentVelocity = slidingSpeedOnPlane;
                    if (currentVelocity.magnitude <= PlayerData.playerConfig.SlidingData.MinSlidingSpeed)
                    {
                        currentVelocity = currentVelocity.normalized *
                                          PlayerData.playerConfig.SlidingData.MinSlidingSpeed;
                    }
                }
                else
                {
                    if (PlayerData.moveInputVector != Vector3.zero && !_startVelocitySet)
                    {
                        Vector3 effectiveGroundNormal = PlayerData.motor.GroundingStatus.GroundNormal;

                        // Reorient velocity on slope
                        currentVelocity =
                            PlayerData.motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                            currentVelocity.magnitude;

                        // Calculate target velocity
                        Vector3 inputRight = Vector3.Cross(PlayerData.moveInputVector, PlayerData.motor.CharacterUp);
                        Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                                  PlayerData.moveInputVector.magnitude;
                        currentVelocity = reorientedInput * PlayerData.playerConfig.SlidingData.MinSlidingSpeed;
                    }
                    else
                    {
                        var lookVector = PlayerData.motor.CharacterForward;
                        currentVelocity = Vector3
                            .ProjectOnPlane(lookVector, PlayerData.motor.GroundingStatus.GroundNormal)
                            .normalized * PlayerData.playerConfig.SlidingData.MinSlidingSpeed;
                        slidingSpeedOnPlane = Vector3.ProjectOnPlane(currentVelocity,
                            PlayerData.motor.GroundingStatus.GroundNormal);
                    }
                }

                if (slidingSpeedOnPlane.magnitude <= PlayerData.playerConfig.SlidingData.SlidingStopThreshold)
                {
                    _stopped = true;
                }

                _startVelocitySet = true;
            }

            if (_stopped)
            {
                Vector3 velocityOnPlane =
                    Vector3.ProjectOnPlane(currentVelocity, PlayerData.motor.GroundingStatus.GroundNormal);
                if (velocityOnPlane.magnitude >= PlayerData.playerConfig.SlidingData.MinSlidingSpeed)
                {
                    _stopped = false;
                }
            }

            //Gravity helps
            if (PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                Vector3 gravityHelp = Vector3.Project(PlayerData.gravity, currentVelocity);
                if (gravityHelp.magnitude > 0.05f && VectorUtils.AreCodirected(currentVelocity, gravityHelp))
                {
                    currentVelocity += gravityHelp * (PlayerData.playerConfig.SlidingData.GravityHelpK * deltaTime);
                    if (currentVelocity.magnitude > PlayerData.playerConfig.SlidingData.MaxSlidingSpeed)
                    {
                        currentVelocity = currentVelocity.normalized *
                                          PlayerData.playerConfig.SlidingData.MaxSlidingSpeed;
                    }

                    if (currentVelocity.magnitude > PlayerData.playerConfig.SlidingData.StableSlidingSpeed)
                    {
                        currentVelocity += -currentVelocity.normalized *
                                           (PlayerData.playerConfig.SlidingData.UnstableDecreaseK * deltaTime);
                    }
                }
            }

            // Acceleration to side
            float currentVelocityMagnitude = currentVelocity.magnitude;
            Vector3 additionalVelocity = PlayerData.motor.CharacterRight *
                                         (PlayerData.playerConfig.SlidingData.SlidingAccelerationByInput *
                                          PlayerData.Inputs.MoveAxisRight);
            Vector3 addForwardPart = Vector3.Project(additionalVelocity, currentVelocity);
            additionalVelocity -= addForwardPart;
            currentVelocity += additionalVelocity * deltaTime;
            currentVelocity = currentVelocity.normalized * currentVelocityMagnitude;


            if (!PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                // Gravity
                currentVelocity += PlayerData.gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (PlayerData.playerConfig.AirMovementData.Drag * deltaTime)));
            }

            if (!Jump(ref currentVelocity, deltaTime))
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
            PlayerData.bonusOrientationMethod = BonusOrientationMethod.TowardsGravity;
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
                PlayerData.motor.SetCapsuleDimensions(0.5f, PlayerData.playerConfig.MiscData.CrouchedCapsuleHeight,
                    PlayerData.playerConfig.MiscData.CrouchedCapsuleHeight * 0.5f);
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