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
        
        public StableMovement StableMovementData => stableMovementData;
        public AirMovement AirMovementData => airMovementData;
        public Jumping JumpingData => jumpingData;
        public Sliding SlidingData => slidingData;
        public Slaming SlamingData => slamingData;
        public Misc MiscData => miscData;
        
        [Serializable]
        public class StableMovement
        {
            [field: SerializeField] public float MaxStableMoveSpeed { get; private set; } = 10f;
            [field: SerializeField] public float StableMovementSharpness { get; private set; } = 15f;
            [field: SerializeField] public float OrientationSharpness { get; private set; } = 10f;
            [field: SerializeField] public OrientationMethod OrientationMethod { get; private set; } = OrientationMethod.TowardsCamera;
        }
        
        [Serializable]
        public class AirMovement
        {
            [field: SerializeField] public float MaxAirMoveSpeed { get; private set; } = 15f;
            [field: SerializeField] public float AirAccelerationSpeed { get; private set; } = 15f;
            [field: SerializeField] public float Drag { get; private set; } = 0.1f;
        }
        [Serializable]
        public class Jumping
        {
            [field: SerializeField] public bool AllowJumpingWhenSliding { get; private set; } = false;
            [field: SerializeField] public float JumpUpSpeed { get; private set; } = 10f;
            [field: SerializeField] public float JumpScalableForwardSpeed { get; private set; } = 10f;
            [field: SerializeField] public float JumpPreGroundingGraceTime { get; private set; } = 0f;
            [field: SerializeField] public float JumpPostGroundingGraceTime { get; private set; } = 0f;
            [field: SerializeField] public int WallJumps { get; private set; } = 3;
            [field: SerializeField] [MinMax(0,1)] public float WallJumpControlPercent { get; private set; } = 1f;
        }

        [Serializable]
        public class Sliding
        {
            [field: SerializeField] public float SpeedBufferApplyTime { get; private set; } = 0.1f;
            [field: SerializeField] public float MaxSlidingSpeed { get; private set; } = 100f;
            [field: SerializeField] public float StableSlidingSpeed { get; private set; } = 35f;
            [field: SerializeField] public float MinSlidingSpeed { get; private set; } = 30f;
            [field: SerializeField] public float SlidingAccelerationByInput { get; private set; } = 30f;
            [field: SerializeField] public float SlidingDirectionByCurrentVelocityThreshold { get; private set; } = 0.1f;
            [field: SerializeField] public float SlidingStopThreshold { get; private set; } = 0.1f;
            [field: SerializeField] public float GravityHelpK { get; private set; } = 5f;
            [field: SerializeField] public float UnstableDecreaseK { get; private set; } = 2f;
            
        }

        [Serializable]
        public class Slaming
        {
            [field: SerializeField] public float SlamingSpeed { get; private set; } = 50;
            [field: SerializeField] public float SlamStorageKeepTime { get; private set; } = 1f;
            [field: SerializeField] public float MaxSlamStorage { get; private set; } = 30;
            [field: SerializeField] public float SlamFlightBackMaxTime { get; private set; } = 2f;
            [field: SerializeField] public float SlamDamageMultiplier { get; private set; } = 10f;
            [field: SerializeField] public float CancelInAirGraceTime { get; private set; } = 50;

            [field: SerializeField] [Tooltip("How big slam storage should be to crush object after jumping with it.")] public float MinCrushPercentage { get; private set; } = 0.5f;
        }
        

        [Serializable]
        public class Misc
        {
            [field: SerializeField] public BonusOrientationMethod BonusOrientationMethod { get; private set; } = BonusOrientationMethod.None;
            [field: SerializeField] public float BonusOrientationSharpness { get; private set; } = 10f;
            [field: SerializeField] public float CrouchedCapsuleHeight { get; private set; } = 1f;
            [field: SerializeField] public float DashSpeed { get; private set; }
            [field: SerializeField] public float DashDuration { get; private set; }
            [field: SerializeField] public float DashMaxEnergy { get; private set; } = 100f;
            [field: SerializeField] public float DashCost { get; private set; } = 33f;
            [field: SerializeField] public float DashRechargeRate { get; private set; } = 10f;
            [field: SerializeField] public float DashDirectionByInputThreshold { get; private set; } = 0.1f;

        }
    }
}