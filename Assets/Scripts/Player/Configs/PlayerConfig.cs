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
        public StableMovement stableMovementData;
        public AirMovement airMovementData;
        public Jumping jumpingData;
        public Sliding slidingData;
        public Slaming slamingData;
        public Misc miscData;
        
        [Serializable]
        public class StableMovement
        {
            public float maxStableMoveSpeed = 10f;
            public float stableMovementSharpness = 15f;
            public float orientationSharpness = 10f;
            public OrientationMethod orientationMethod = OrientationMethod.TowardsCamera;
        }
        
        [Serializable]
        public class AirMovement
        {
            public float maxAirMoveSpeed = 15f;
            public float airAccelerationSpeed = 15f;
            public float drag = 0.1f;
        }
        [Serializable]
        public class Jumping
        {
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
            public float minSlidingSpeed = 30f;
            public float slidingAccelerationByInput = 30f;
            public float slidingDirectionByCurrentVelocityThreshold = 0.1f;
            public float slidingStopThreshold = 0.1f;
            public float gravityHelpK = 5f;
        }

        [Serializable]
        public class Slaming
        {
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