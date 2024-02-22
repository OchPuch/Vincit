using System;
using System.Collections.Generic;
using KinematicCharacterController;
using Player.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [Serializable]
    public class PlayerData
    {
        [Header("Settings")]
        public PlayerConfig playerConfig;
        public Vector3 gravity = new Vector3(0, -30f, 0);
        [Header("Components")]
        public KinematicCharacterMotor motor;
        public PlayerView playerView;
        public PlayerMovementAudio playerMovementAudio;
        [Header("Transforms")]
        public Transform meshRoot;
        public Transform cameraFollowPoint;
        [Space(10)]
        public List<Collider> IgnoredColliders = new List<Collider>();

        #region input
        [HideInInspector] public CharacterController.PlayerCharacterInputs Inputs;
        [HideInInspector] public Vector3 moveInputVector;
        [HideInInspector] public Vector3 lookInputVector;
        #endregion
        
        #region jumping
        [HideInInspector] public bool jumpRequested = false;
        [HideInInspector] public bool jumpConsumed = false;
        [HideInInspector] public bool jumpedThisFrame = false;
        [HideInInspector] public float timeSinceJumpRequested = Mathf.Infinity;
        [HideInInspector] public float timeSinceLastAbleToJump = 0f;
        #endregion

        #region wallJumping
        [HideInInspector] public bool isNearWall = false;
        [HideInInspector] public Vector3 wallNormal = Vector3.zero;
        [HideInInspector] public int wallJumpCount = 0;
        #endregion
        
        #region sliding
        [HideInInspector] public bool shouldBeSliding = false;
        [HideInInspector] public bool isSliding = false;
        #endregion

        #region dash
        [HideInInspector] public bool dashRequested = false;
        [HideInInspector] public bool isDashing = false;
        [HideInInspector] public float currentDashEnergy = 0f;
        #endregion

        #region slam
        [HideInInspector] public bool isSlaming = false;
        [HideInInspector] public float slamStorage = 0f;
        [HideInInspector] public float slamStorageKeepTimer = 0f;
        #endregion
        
        [HideInInspector] public Vector3 internalVelocityAdd = Vector3.zero;
        
        [HideInInspector] public Collider[] probedColliders = new Collider[8];
        [HideInInspector] public RaycastHit[] probedHits = new RaycastHit[8];
        
        [HideInInspector] public Vector3 lastInnerNormal = Vector3.zero;
        [HideInInspector] public Vector3 lastOuterNormal = Vector3.zero;
        
    }
}