using FMODUnity;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace General.GlobalManagers
{
    public class AudioManager : MonoBehaviour
    {
        //TODO: Move to scriptable object
        [Header("Slowmo")]
        [ParamRef][SerializeField] private string slowMoParameter;
        [SerializeField] private float slowMoDefaultValue;
        [SerializeField] private TweenSettings<float> introSlowmo;
        [SerializeField] private TweenSettings<float> outroSlowmo;
        
        private ITimeNotifier _timeNotifier;
        
        [Inject]
        private void Construct(ITimeNotifier timeNotifier)
        {
            _timeNotifier = timeNotifier;

            _timeNotifier.TimeContinued += OnTimeContinue;
            _timeNotifier.TimeStopped += OnTimeStopped;
        }

        private void Start()
        {
            SetSlowmoParameter(slowMoDefaultValue);
        }

        private void OnDestroy()
        {
            _timeNotifier.TimeStopped -= OnTimeContinue;
            _timeNotifier.TimeStopped -= OnTimeStopped;
        }

        private void OnTimeStopped()
        {
            Tween.Custom(introSlowmo, onValueChange: SetSlowmoParameter);
        }

        private void OnTimeContinue()
        {
            Tween.Custom(outroSlowmo, onValueChange: SetSlowmoParameter);
        }

        private void SetSlowmoParameter(float value)
        {
            RuntimeManager.StudioSystem.setParameterByName(slowMoParameter, value);
        }
    }
}
