using System;
using GlobalManagers;
using UnityEngine;
using Zenject;

namespace TimeStop
{
    [Serializable]
    public class TimeStopAbility 
    {
        [SerializeField] private float abilityDuration;
        private float _abilityTimer;

        private TimeController _timeController;
        private ITimeNotifier _timeNotifier;

        public void Init(TimeController timeController, ITimeNotifier timeNotifier)
        {
            _timeController = timeController;
            _timeNotifier = timeNotifier;
        }
        
        public void SwitchActive()
        {
            if (_timeNotifier.IsTimeStopped)
            {
                _timeController.RequestFullTimeContinue();
            }
            else
            {
                _timeController.RequestFullTimeStop();
            }
        }

        public void Update()
        {
            if (_timeNotifier.IsTimeStopped)
            {
                _abilityTimer += Time.unscaledDeltaTime;
                if (_abilityTimer > abilityDuration)
                {
                    _abilityTimer = 0f;
                    SwitchActive();
                }
            }
            else if (_abilityTimer > 0)
            {
                _abilityTimer -= Time.unscaledDeltaTime;
                if (_abilityTimer < 0) _abilityTimer = 0;
            }
        }
    }
}