using KinematicCharacterController.Core;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace KinematicCharacterController.Examples.Scripts.Editor
{
    public class PauseStateHandler
    {
        private static KinematicCharacterSystem _kinematicCharacterSystem;

        [Inject]
        private static void Construct(KinematicCharacterSystem kinematicCharacterSystem)
        {
            _kinematicCharacterSystem = kinematicCharacterSystem;
        }
    
        [RuntimeInitializeOnLoadMethod()]
        public static void Init()
        {
            EditorApplication.pauseStateChanged += HandlePauseStateChange;
        }
    
        private static void HandlePauseStateChange(PauseState state)
        {
            foreach(KinematicCharacterMotor motor in _kinematicCharacterSystem.CharacterMotors)
            {
                motor.SetPositionAndRotation(motor.Transform.position, motor.Transform.rotation, true);
            }
        }
    }
}
