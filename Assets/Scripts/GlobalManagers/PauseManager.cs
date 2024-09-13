using System;

namespace GlobalManagers
{
    public class PauseManager : IPauseNotifier
    {
        public bool IsPaused { get; private set; }
        public event Action Paused;
        public event Action Resumed;
        
        public void Pause()
        {
            IsPaused = true;
            Paused?.Invoke();
        }

        public void Resume()
        {
            IsPaused = false;
            Resumed?.Invoke();
        }
    }
}