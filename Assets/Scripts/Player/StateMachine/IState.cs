namespace Player.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();

        void Update();

    }
}