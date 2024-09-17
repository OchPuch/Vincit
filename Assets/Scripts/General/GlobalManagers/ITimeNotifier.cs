using System;

namespace General.GlobalManagers
{
    public interface ITimeNotifier
    {
        public bool IsTimeStopped { get; }
        public event Action TimeStopped;
        public event Action TimeContinued;
    }
}