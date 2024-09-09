using Entities;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Player.Data;
using UnityEngine;
using Utils;

namespace Player
{
    public class Player : GamePlayBehaviour, IDamageable
    {
        public static Player Instance { get; private set; }
        
        private PlayerData _data;
        private PlayerController _character;
        private ExampleCharacterCamera _characterCamera;
        private PhysicsMover _physicsMover;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        public void Init(PlayerData data, PlayerController character  ,ExampleCharacterCamera characterCamera)
        {
            if (Instance == this) return;
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            _data = data;
            _character = character;
            _characterCamera = characterCamera;
            if (_data.motor.AttachedRigidbody != null)
                _physicsMover = _data.motor.AttachedRigidbody.GetComponent<PhysicsMover>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Instance != this) return;
            Instance = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (_characterCamera.RotateWithPhysicsMover)
            {
                if (_data.motor.AttachedRigidbody && !_physicsMover)
                {
                    _physicsMover = _data.motor.AttachedRigidbody.GetComponent<PhysicsMover>();
                }

                _characterCamera.PlanarDirection =
                    _physicsMover.RotationDeltaFromInterpolation * _characterCamera.PlanarDirection;
                _characterCamera.PlanarDirection = Vector3
                    .ProjectOnPlane(_characterCamera.PlanarDirection, _data.motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            _characterCamera.UpdateWithInput(Time.unscaledDeltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetMouseButtonDown(1))
            {
                _characterCamera.TargetDistance =
                    (_characterCamera.TargetDistance == 0f) ? _characterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerController.PlayerCharacterInputs characterInputs = new PlayerController.PlayerCharacterInputs
            {
                // Build the CharacterInputs struct
                MoveAxisForward = Input.GetAxisRaw(VerticalInput),
                MoveAxisRight = Input.GetAxisRaw(HorizontalInput),
                CameraRotation = _characterCamera.Transform.rotation,
                JumpDown = Input.GetKeyDown(KeyCode.Space),
                CrouchDown = Input.GetKeyDown(KeyCode.CapsLock) || Input.GetKeyDown(KeyCode.LeftControl),
                CrouchUp = Input.GetKeyUp(KeyCode.CapsLock) || Input.GetKeyUp(KeyCode.LeftControl),
                DashDown = Input.GetKeyDown(KeyCode.LeftShift)
            };

            // Apply inputs to character
            _character.SetInputs(ref characterInputs);
        }

        public void RequestPush(Vector3 pushForce, ForceMode pushMode)
        {
            _data.pushForce = pushForce;
            _data.pushMode = pushMode;
            _data.pushRequested = true;
        }

        public void Damage(float damage)
        {
            
        }
    }
    
}