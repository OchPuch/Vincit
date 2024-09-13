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

        private TimeController _timeManager;
        private ITimeNotifier _timeNotifier;

        [Inject]
        private void Construct(TimeController timeController, ITimeNotifier timeNotifier)
        {
            _timeManager = timeController;
            _timeNotifier = timeNotifier;
        }
        
        public void SwitchActive()
        {
            if (_timeNotifier.IsTimeStopped)
            {
                _timeManager.RequestFullTimeContinue();
            }
            else
            {
                _timeManager.RequestFullTimeStop();
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