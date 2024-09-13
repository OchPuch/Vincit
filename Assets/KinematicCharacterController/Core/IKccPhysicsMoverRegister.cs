namespace KinematicCharacterController.Core
{
    public interface IKccPhysicsMoverRegister
    {
        public void RegisterPhysicsMover(PhysicsMover mover);
        
        public void UnregisterPhysicsMover(PhysicsMover mover);
    }
}