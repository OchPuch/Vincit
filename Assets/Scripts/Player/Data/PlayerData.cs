using System;
using System.Collections.Generic;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Player.Configs;
using Player.View.Audio;
using RayFire;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Data
{
    [Serializable]
    public class PlayerData
    {
        [Header("Settings")] public PlayerConfig playerConfig;
        public Vector3 gravity = new Vector3(0, -30f, 0);
        [Header("Components")] [Space(5)] public KinematicCharacterMotor motor;
        public PlayerMovementAudio playerMovementAudio;
        [Space(2)] public RayfireGun slamGun;
        public RayfireGun dashGun;
        public Transform dashCrushPoint;
        [Header("Transforms")] [Space(5)] public Transform meshRoot;
        public Transform cameraFollowPoint;
        [Space(10)] public LayerMask levelLayerMask;
        public List<Collider> IgnoredColliders = new List<Collider>();
        public BonusOrientationMethod bonusOrientationMethod = BonusOrientationMethod.TowardsGravity;

        #region input

        public PlayerController.PlayerCharacterInputs Inputs;
        public Vector3 moveInputVector;
        public Vector3 lookInputVector;

        #endregion

        #region jumping

        public bool jumpRequested = false;
        public bool jumpConsumed = false;
        public bool jumpedThisFrame = false;
        public float timeSinceJumpRequested = Mathf.Infinity;
        public float timeSinceLastAbleToJump = 0f;

        #endregion

        #region wallJumping

        public Collider lastWallJumpCollider = null;
        public bool isNearWall = false;
        public Vector3 wallNormal = Vector3.zero;
        public int wallJumpCount = 0;

        #endregion

        #region sliding

        [FormerlySerializedAs("bufferApplyTimer")] public float slideSpeedBufferApplyTimer;
        public Vector3 slideSpeedBuffer;
        public bool shouldBeSliding = false;
        public bool isSliding = false;

        #endregion

        #region dash

        public bool dashRequested = false;
        public bool isDashing = false;
        public float currentDashEnergy = 0f;

        #endregion

        #region slam

        public bool isSlaming = false;
        public float slamStorage = 0f;
        public float slamStorageKeepTimer = 0f;

        #endregion
        
        public Vector3 internalVelocityAdd = Vector3.zero;

        public Collider[] probedColliders = new Collider[8];
        public RaycastHit[] probedHits = new RaycastHit[8];

        public Vector3 lastInnerNormal = Vector3.zero;
        public Vector3 lastOuterNormal = Vector3.zero;

        public bool pushRequested;
        public ForceMode pushMode;
        public Vector3 pushForce;
    }
}