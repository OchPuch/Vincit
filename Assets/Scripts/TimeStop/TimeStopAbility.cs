using System;
using GlobalManagers;
using UnityEngine;

namespace TimeStop
{
    [Serializable]
    public class TimeStopAbility 
    {
        [SerializeField] private float abilityDuration;
        private float _abilityTimer;
        
        
        public void SwitchActive()
        {
            if (TimeManager.Instance.IsTimeStopped)
            {
                TimeManager.Instance.TimeContinue();
            }
            else
            {
                TimeManager.Instance.TimeStop();
            }
        }

        public void Update()
        {
            if (TimeManager.Instance.IsTimeStopped)
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