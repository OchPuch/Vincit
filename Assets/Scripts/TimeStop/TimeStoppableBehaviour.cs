using System;
using GlobalManagers;
using UnityEngine;
using Utils;

namespace TimeStop
{
    public abstract class TimeStoppableBehaviour : GamePlayBehaviour
    {
        
        [SerializeField] private bool disableOnTimeStop;

        protected override void Start()
        {
            base.Start();
            if (TimeManager.Instance == null) 
            {
                Debug.LogError("Time Manager is null");
            }
            TimeManager.Instance.TimeStopped += OnTimeStop;
            TimeManager.Instance.TimeContinued += OnTimeContinue;
            if (TimeManager.Instance.IsTimeStopped) OnTimeStop();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TimeManager.Instance.TimeStopped -= OnTimeStop;
            TimeManager.Instance.TimeContinued -= OnTimeContinue;
        }


        private void OnTimeStop()
        {
            if (disableOnTimeStop) enabled = false;
            PostTimeStop();
        }

        private void OnTimeContinue()
        {
            if (disableOnTimeStop) enabled = true;
            PostTimeContinue();
        }
        
        protected virtual void PostTimeStop() { }
        protected virtual void PostTimeContinue() { }
        
        
    }
}