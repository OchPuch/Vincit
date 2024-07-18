using System;
using GlobalManagers;

namespace TimeStop
{
    [Serializable]
    public class TimeStopAbility 
    {
        public void Activate()
        {
            if (TimeManager.Instance.IsTimeStopped) TimeManager.Instance.TimeContinue();
            else TimeManager.Instance.TimeStop();
        }
    }
}