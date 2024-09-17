using System;
using General;
using General.GlobalManagers;
using UnityEngine;
using Zenject;

namespace TimeStop
{
    public abstract class TimeStoppableBehaviour : GamePlayBehaviour
    {
        [SerializeField] private bool disableOnTimeStop;
        [Inject] protected ITimeNotifier TimeNotifier;
        
        protected override void Start()
        {
            base.Start();
            TimeNotifier.TimeStopped += OnTimeStop;
            TimeNotifier.TimeContinued += OnTimeContinue;
            if (TimeNotifier.IsTimeStopped) OnTimeStop();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TimeNotifier.TimeStopped -= OnTimeStop;
            TimeNotifier.TimeContinued -= OnTimeContinue;
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