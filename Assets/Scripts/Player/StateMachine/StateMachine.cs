using System;
using System.Collections.Generic;

namespace Player.StateMachine
{
    public abstract class StateMachine
    {
        protected List<IState> states;
        protected IState currentState;
        public event Action<IState> StateEntered;
        public event Action<IState> StateExited;
        
        protected void OnStateEntered(IState state)
        {
            StateEntered?.Invoke(state);
        }
        
        protected void OnStateExited(IState state)
        {
            StateExited?.Invoke(state);
        }
        
        public bool IsInState<TState>() where TState : IState
        {
            return currentState is TState;
        }
        
        
    }
}