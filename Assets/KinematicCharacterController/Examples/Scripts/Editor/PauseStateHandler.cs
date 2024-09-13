
using UnityEngine;
using UnityEditor;
using KinematicCharacterController;
using KinematicCharacterController.Core;
using Zenject;

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
