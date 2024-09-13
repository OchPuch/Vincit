using System;

namespace GlobalManagers
{
    public interface IPauseNotifier
    {
        public bool IsPaused { get; }
        public event Action Paused;
        public event Action Resumed;
    }
}