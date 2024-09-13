namespace KinematicCharacterController.Core
{
    public interface IKccMotorRegister
    {
        public void RegisterCharacterMotor(KinematicCharacterMotor motor);
        
        public void UnregisterCharacterMotor(KinematicCharacterMotor motor);
    }
}