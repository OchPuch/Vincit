﻿using UnityEngine;
using Zenject;

namespace General.GlobalManagers
{
    public class TimeController : MonoBehaviour
    {
        private TimeManager _timeManager;

        [Inject]
        private void Construct(TimeManager timeManager)
        {
            _timeManager = timeManager;
        }

        public void RequestTimeFreezeEffect(float duration, float timeScale = 0f)
        {
            _timeManager.FreezeTimeEffectStart(duration, timeScale);
        }

        public void RequestTimeUnfreezeEffect()
        {
            _timeManager.StopFreezeTimeEffect();
        }

        public void RequestFullTimeStop()
        {
            _timeManager.TimeStop();
        }

        public void RequestFullTimeContinue()
        {
            _timeManager.TimeContinue();
        }
    }
    
}