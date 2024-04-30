using System;
using KinematicCharacterController.Examples;
using Unity.Entities.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Player/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private StableMovement stableMovementData;
        [SerializeField] private AirMovement airMovementData;
        [SerializeField] private Jumping jumpingData;
        [SerializeField] private Sliding slidingData;
        [SerializeField] private Slaming slamingData;
        [SerializeField] private Misc miscData;
        
        public StableMovement StableMovementData => stableMovementData.GetReadonly();
        public AirMovement AirMovementData => airMovementData.GetReadonly();
        public Jumping JumpingData => jumpingData.GetReadonly();
        public Sliding SlidingData => slidingData.GetReadonly();
        public Slaming SlamingData => slamingData.GetReadonly();
        public Misc MiscData => miscData.GetReadonly();
        
        [Serializable]
        public class StableMovement
        {
            public StableMovement GetReadonly()
            {
                return new StableMovement
                {
                    maxStableMoveSpeed = maxStableMoveSpeed,
                    stableMovementSharpness = stableMovementSharpness,
                    orientationSharpness = orientationSharpness,
                    orientationMethod = orientationMethod
                };
            }
            
            public float maxStableMoveSpeed = 10f;
            public float stableMovementSharpness = 15f;
            public float orientationSharpness = 10f;
            public OrientationMethod orientationMethod = OrientationMethod.TowardsCamera;
        }
        
        [Serializable]
        public class AirMovement
        {
            public AirMovement GetReadonly()
            {
                return new AirMovement
                {
                    maxAirMoveSpeed = maxAirMoveSpeed,
                    airAccelerationSpeed = airAccelerationSpeed,
                    drag = drag
                };
            }
            
            public float maxAirMoveSpeed = 15f;
            public float airAccelerationSpeed = 15f;
            public float drag = 0.1f;
        }
        [Serializable]
        public class Jumping
        {
            public Jumping GetReadonly()
            {
                return new Jumping
                {
                    jumpPreGroundingGraceTime = jumpPreGroundingGraceTime,
                    allowJumpingWhenSliding = allowJumpingWhenSliding,
                    jumpUpSpeed = jumpUpSpeed,
                    jumpScalableForwardSpeed = jumpScalableForwardSpeed,
                    jumpPostGroundingGraceTime = jumpPostGroundingGraceTime,
                    wallJumps = wallJumps,
                    wallJumpControlPercent = wallJumpControlPercent
                };
            }
            
            public bool allowJumpingWhenSliding = false;
            public float jumpUpSpeed = 10f;
            public float jumpScalableForwardSpeed = 10f;
            public float jumpPreGroundingGraceTime = 0f;
            public float jumpPostGroundingGraceTime = 0f;
            public int wallJumps = 3;
            [MinMax(0,1)] public float wallJumpControlPercent = 1f;
        }

        [Serializable]
        public class Sliding
        {
            public Sliding GetReadonly()
            {
                return new Sliding
                {
                    minSlidingSpeed = minSlidingSpeed,
                    slidingAccelerationByInput = slidingAccelerationByInput,
                    slidingDirectionByCurrentVelocityThreshold = slidingDirectionByCurrentVelocityThreshold,
                    slidingStopThreshold = slidingStopThreshold,
                    gravityHelpK = gravityHelpK
                };
            }
            
            public float minSlidingSpeed = 30f;
            public float slidingAccelerationByInput = 30f;
            public float slidingDirectionByCurrentVelocityThreshold = 0.1f;
            public float slidingStopThreshold = 0.1f;
            public float gravityHelpK = 5f;
        }

        [Serializable]
        public class Slaming
        {
            public Slaming GetReadonly()
            {
                return new Slaming
                {
                    slamingSpeed = slamingSpeed,
                    slamStorageKeepTime = slamStorageKeepTime,
                    maxSlamStorage = maxSlamStorage,
                    slamFlightBackMaxTime = slamFlightBackMaxTime,
                    slamDamageMultiplier = slamDamageMultiplier,
                    minCrushPercentage = minCrushPercentage
                };
            }
            
            public float slamingSpeed = 50;
            public float slamStorageKeepTime = 1f;
            public float maxSlamStorage = 30;
            public float slamFlightBackMaxTime = 2f;
            public float slamDamageMultiplier = 10f;
            [Tooltip("How big slam storage should be to crush object after jumping with it.")] public float minCrushPercentage = 0.5f;
        }
        

        [Serializable]
        public class Misc
        {
            public Misc GetReadonly()
            {
                return new Misc
                {
                    bonusOrientationMethod = bonusOrientationMethod,
                    bonusOrientationSharpness = bonusOrientationSharpness,
                    crouchedCapsuleHeight = crouchedCapsuleHeight,
                    dashSpeed = dashSpeed,
                    dashDuration = dashDuration,
                    dashMaxEnergy = dashMaxEnergy,
                    dashCost = dashCost,
                    dashRechargeRate = dashRechargeRate,
                    dashDirectionByInputThreshold = dashDirectionByInputThreshold
                };
            }
            
            
            public BonusOrientationMethod bonusOrientationMethod = BonusOrientationMethod.None;
            public float bonusOrientationSharpness = 10f;
            public float crouchedCapsuleHeight = 1f;
            public float dashSpeed;
            public float dashDuration;
            public float dashMaxEnergy = 100f;
            public float dashCost = 33f;
            public float dashRechargeRate = 10f;
            public float dashDirectionByInputThreshold = 0.1f;

        }
    }
}