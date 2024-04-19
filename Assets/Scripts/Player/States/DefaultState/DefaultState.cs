using System;
using KinematicCharacterController.Examples;
using Player.States.DefaultState.Airborne;
using Player.States.DefaultState.Special;
using StateMachine;
using UnityEngine;
using Utils;


namespace Player.States.DefaultState
{
    [Serializable]
    public class DefaultState : PlayerState
    {
        public DefaultState(CharacterController controller, IStateSwitcher stateMachine, PlayerData playerData) : base(
            controller, stateMachine, playerData)
        {
            
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            base.UpdateRotation(ref currentRotation, deltaTime);
            if (PlayerData.lookInputVector.sqrMagnitude > 0f &&
                PlayerData.playerConfig.stableMovementData.orientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(PlayerData.motor.CharacterForward,
                        PlayerData.lookInputVector,
                        1 - Mathf.Exp(-PlayerData.playerConfig.stableMovementData.orientationSharpness * deltaTime))
                    .normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, PlayerData.motor.CharacterUp);
            }

            Vector3 currentUp = (currentRotation * Vector3.up);
            switch (PlayerData.playerConfig.miscData.bonusOrientationMethod)
            {
                case BonusOrientationMethod.TowardsGravity:
                {
                    // Rotate from current up to invert gravity
                    Vector3 smoothedGravityDir = Vector3.Slerp(currentUp,
                        -PlayerData.gravity.normalized,
                        1 - Mathf.Exp(-PlayerData.playerConfig.miscData.bonusOrientationSharpness * deltaTime));
                    currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                    break;
                }
                case BonusOrientationMethod.TowardsGroundSlopeAndGravity
                    when PlayerData.motor.GroundingStatus.IsStableOnGround:
                {
                    Vector3 initialCharacterBottomHemiCenter =
                        PlayerData.motor.TransientPosition + (currentUp * PlayerData.motor.Capsule.radius);

                    Vector3 smoothedGroundNormal = Vector3.Slerp(PlayerData.motor.CharacterUp,
                        PlayerData.motor.GroundingStatus.GroundNormal,
                        1 - Mathf.Exp(-PlayerData.playerConfig.miscData.bonusOrientationSharpness * deltaTime));
                    currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                    // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                    PlayerData.motor.SetTransientPosition(initialCharacterBottomHemiCenter +
                                                          (currentRotation * Vector3.down *
                                                           PlayerData.motor.Capsule.radius));
                    break;
                }
                case BonusOrientationMethod.TowardsGroundSlopeAndGravity:
                {
                    Vector3 smoothedGravityDir = Vector3.Slerp(currentUp,
                        -PlayerData.gravity.normalized,
                        1 - Mathf.Exp(-PlayerData.playerConfig.miscData.bonusOrientationSharpness * deltaTime));
                    currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                    break;
                }
                default:
                {
                    Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up,
                        1 - Mathf.Exp(-PlayerData.playerConfig.miscData.bonusOrientationSharpness * deltaTime));
                    currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                    break;
                }
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);

            PlayerData.jumpedThisFrame = false;
            PlayerData.timeSinceJumpRequested += deltaTime;
            
            PlayerData.slamStorageKeepTimer += deltaTime;
            PlayerData.currentDashEnergy += deltaTime * PlayerData.playerConfig.miscData.dashRechargeRate;
            PlayerData.currentDashEnergy = Mathf.Clamp(PlayerData.currentDashEnergy, 0, PlayerData.playerConfig.miscData.dashMaxEnergy);
            
            // Take into account additive velocity
            if (PlayerData.internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += PlayerData.internalVelocityAdd;
                PlayerData.internalVelocityAdd = Vector3.zero;
            }
            

        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            base.AfterCharacterUpdate(deltaTime);

            // Handle jumping pre-ground grace period
            if (PlayerData.jumpRequested && PlayerData.timeSinceJumpRequested >
                PlayerData.playerConfig.jumpingData.jumpPreGroundingGraceTime)
            {
                PlayerData.jumpRequested = false;
            }
            
            if (PlayerData.slamStorageKeepTimer > PlayerData.playerConfig.slamingData.slamStorageKeepTime)
            {
                PlayerData.slamStorage = 0;
            }

            if (PlayerData.dashRequested)
            {
                if (PlayerData.currentDashEnergy >= PlayerData.playerConfig.miscData.dashCost)
                {
                    StateMachine.SwitchState<DefaultDashState>();
                }
                PlayerData.dashRequested = false;
            }
        }

        public override void SetInputs(ref CharacterController.PlayerCharacterInputs newInputs)
        {
            base.SetInputs(ref newInputs);
            PlayerData.Inputs = newInputs;
            // Clamp input
            Vector3 moveInputVector =
                Vector3.ClampMagnitude(
                    new Vector3(PlayerData.Inputs.MoveAxisRight, 0f, PlayerData.Inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection =
                Vector3.ProjectOnPlane(PlayerData.Inputs.CameraRotation * Vector3.forward, PlayerData.motor.CharacterUp)
                    .normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3
                    .ProjectOnPlane(PlayerData.Inputs.CameraRotation * Vector3.up, PlayerData.motor.CharacterUp)
                    .normalized;
            }

            Quaternion cameraPlanarRotation =
                Quaternion.LookRotation(cameraPlanarDirection, PlayerData.motor.CharacterUp);


            // Move and look inputs
            PlayerData.moveInputVector = cameraPlanarRotation * moveInputVector;

            switch (PlayerData.playerConfig.stableMovementData.orientationMethod)
            {
                case OrientationMethod.TowardsCamera:
                    PlayerData.lookInputVector = cameraPlanarDirection;
                    break;
                case OrientationMethod.TowardsMovement:
                    PlayerData.lookInputVector = PlayerData.moveInputVector.normalized;
                    break;
            }

            // Jumping input
            if (PlayerData.Inputs.JumpDown)
            {
                PlayerData.timeSinceJumpRequested = 0f;
                PlayerData.jumpRequested = true;
            }

            // Crouching input
            if (PlayerData.Inputs.CrouchDown)
            {
                PlayerData.shouldBeSliding = true;
            }
            else if (PlayerData.Inputs.CrouchUp)
            {
                PlayerData.shouldBeSliding = false;
            }

            if (PlayerData.Inputs.DashDown)
            {
                PlayerData.dashRequested = true;
            }
        }

        protected void Jump(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!PlayerData.jumpRequested) return;
            // See if we actually are allowed to jump
            if (!PlayerData.jumpConsumed &&
                 ((PlayerData.playerConfig.jumpingData.allowJumpingWhenSliding
                      ? PlayerData.motor.GroundingStatus.FoundAnyGround
                      : PlayerData.motor.GroundingStatus.IsStableOnGround) ||
                  PlayerData.timeSinceLastAbleToJump <=
                  PlayerData.playerConfig.jumpingData.jumpPostGroundingGraceTime))
            {
                // Calculate jump direction before ungrounding
                Vector3 jumpDirection = PlayerData.motor.CharacterUp;
                if (PlayerData.motor.GroundingStatus is { FoundAnyGround: true, IsStableOnGround: false })
                {
                    jumpDirection = PlayerData.motor.GroundingStatus.GroundNormal;
                }
                
                PlayerData.motor.ForceUnground();

                // Add to the return velocity and reset jump state
                currentVelocity += (jumpDirection * PlayerData.playerConfig.jumpingData.jumpUpSpeed) -
                                   Vector3.Project(currentVelocity, PlayerData.motor.CharacterUp);
                currentVelocity += (PlayerData.moveInputVector *
                                    PlayerData.playerConfig.jumpingData.jumpScalableForwardSpeed);
                currentVelocity += (jumpDirection * PlayerData.slamStorage);
                
                //Crush ground if slam storage is greater than 0
                if (PlayerData.slamStorage > 0)
                {
                    float slamRadius = PlayerData.slamGun.radius;
                    float slamStoragePercent = PlayerData.slamStorage / PlayerData.playerConfig.slamingData.maxSlamStorage;
                    if (slamStoragePercent >= PlayerData.playerConfig.slamingData.minCrushPercentage)
                    {
                        PlayerData.slamGun.radius = slamStoragePercent * slamRadius;
                        PlayerData.slamGun.damage = slamStoragePercent * PlayerData.playerConfig.slamingData.slamDamageMultiplier;
                        PlayerData.slamGun.Shoot();
                        PlayerData.slamGun.radius = slamRadius;
                    }
                }
                
                
                PlayerData.jumpRequested = false;
                PlayerData.jumpConsumed = true;
                PlayerData.jumpedThisFrame = true;
                StateMachine.SwitchState<DefaultFlyingState>();
                PlayerData.playerMovementAudio.PlayJumpSound();
            }
        }

        protected void WallJump(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!PlayerData.jumpRequested) return;
            if (PlayerData.wallJumpCount >= PlayerData.playerConfig.jumpingData.wallJumps) return;
            if (PlayerData.isNearWall && !PlayerData.motor.GroundingStatus.IsStableOnGround)
            {
                //hit the wall with raycast to get the normal
                RaycastHit hit;
                var position = PlayerData.meshRoot.transform.position;
                Vector3 closestPoint = PlayerData.lastWallJumpCollider.ClosestPointOnBounds(position);
                Vector3 direction = closestPoint - position;
                if (Physics.Raycast(position, direction, out hit, 1f + direction.magnitude, PlayerData.levelLayerMask))
                {
                    PlayerData.wallNormal = hit.normal;
                }
                
                
                Vector3 jumpDirection;
                if (VectorUtils.IsLookingAtThePlane(PlayerData.motor.CharacterForward, PlayerData.wallNormal))
                {
                    jumpDirection = -PlayerData.motor.CharacterForward;
                }
                else
                {
                    jumpDirection = PlayerData.motor.CharacterForward;
                }
                
                float jumpControlPercent = PlayerData.wallNormal == Vector3.zero ? 1 : PlayerData.playerConfig.jumpingData.wallJumpControlPercent;
                
                PlayerData.motor.ForceUnground();
                
                currentVelocity += (PlayerData.motor.CharacterUp * PlayerData.playerConfig.jumpingData.jumpUpSpeed) -
                                   Vector3.Project(currentVelocity, PlayerData.motor.CharacterUp);
                currentVelocity += (jumpDirection * 
                                    (PlayerData.playerConfig.jumpingData.jumpUpSpeed * 
                                     jumpControlPercent));
                currentVelocity += (PlayerData.wallNormal *
                                    (PlayerData.playerConfig.jumpingData.jumpUpSpeed *
                                     (1 - jumpControlPercent)));
                PlayerData.jumpRequested = false;
                PlayerData.jumpConsumed = true;
                PlayerData.jumpedThisFrame = true;
                PlayerData.wallJumpCount++;
                
                StateMachine.SwitchState<DefaultFlyingState>();
                PlayerData.playerMovementAudio.PlayWallJumpSound(PlayerData.wallJumpCount - 1);
            }
        }
    }
}