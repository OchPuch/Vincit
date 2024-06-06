using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Player.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public PlayerData playerData = new PlayerData();
        [FormerlySerializedAs("character")] public PlayerController player;
        public ExampleCharacterCamera characterCamera;

        private PhysicsMover _physicsMover;
        
        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        

        private void Start()
        {
            player.Init(playerData);
            
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            characterCamera.SetFollowTransform(playerData.cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            characterCamera.IgnoredColliders.Clear();
            characterCamera.IgnoredColliders.AddRange(player.GetComponentsInChildren<Collider>());

            if (playerData.motor.AttachedRigidbody != null) 
                _physicsMover = playerData.motor.AttachedRigidbody.GetComponent<PhysicsMover>();
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
            if (characterCamera.RotateWithPhysicsMover)
            {
                if (playerData.motor.AttachedRigidbody && !_physicsMover)
                {
                    _physicsMover = playerData.motor.AttachedRigidbody.GetComponent<PhysicsMover>();
                }
                characterCamera.PlanarDirection = _physicsMover.RotationDeltaFromInterpolation * characterCamera.PlanarDirection;
                characterCamera.PlanarDirection = Vector3.ProjectOnPlane(characterCamera.PlanarDirection, playerData.motor.CharacterUp).normalized;
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
            characterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetMouseButtonDown(1))
            {
                characterCamera.TargetDistance = (characterCamera.TargetDistance == 0f) ? characterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerController.PlayerCharacterInputs characterInputs = new PlayerController.PlayerCharacterInputs
            {
                // Build the CharacterInputs struct
                MoveAxisForward = Input.GetAxisRaw(VerticalInput),
                MoveAxisRight = Input.GetAxisRaw(HorizontalInput),
                CameraRotation = characterCamera.Transform.rotation,
                JumpDown = Input.GetKeyDown(KeyCode.Space),
                CrouchDown = Input.GetKeyDown(KeyCode.CapsLock),
                CrouchUp = Input.GetKeyUp(KeyCode.CapsLock),
                DashDown = Input.GetKeyDown(KeyCode.LeftShift)
            };

            // Apply inputs to character
            player.SetInputs(ref characterInputs);
        }
    } 
}
