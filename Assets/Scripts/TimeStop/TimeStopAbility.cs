using General;
using General.GlobalManagers;
using UnityEngine;
using Zenject;

namespace TimeStop
{
    public class TimeStopAbility : GamePlayBehaviour
    {
        [field: SerializeField] public float AbilityDuration { get; private set; }
        private float _abilityTimer;

        private TimeController _timeController;
        private ITimeNotifier _timeNotifier;

        [Inject]
        private void Construct(TimeController timeController, ITimeNotifier timeNotifier)
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
                _abilityTimer -= Time.unscaledDeltaTime;
                if (_abilityTimer <= 0)
                {
                    _abilityTimer = 0f;
                    SwitchActive();
                }
            }
            else
            {
                _abilityTimer += Time.unscaledDeltaTime;
                if (_abilityTimer > AbilityDuration) _abilityTimer = AbilityDuration;
            }
        }

        public float GetCharge01()
        {
            return _abilityTimer / AbilityDuration;
        }
    }
}