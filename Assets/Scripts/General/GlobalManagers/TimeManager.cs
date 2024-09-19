using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace General.GlobalManagers
{
    public class TimeManager : ITimeNotifier
    { 
        public bool IsTimeStopped { get; private set; }
        public event Action TimeStopped;
        public event Action TimeContinued;

        private float _pausedTimeScale = 1.0f;
        private Coroutine _freezingEffect;
        
        private readonly IPauseNotifier _pauseNotifier;
        private readonly MonoBehaviour _context;
        
        public TimeManager(MonoBehaviour context, IPauseNotifier pauseManager)
        {
            _context = context;
            _pauseNotifier = pauseManager;
            pauseManager.Paused += OnPause;
            pauseManager.Resumed += OnResume;
        }
        

        public void FreezeTimeEffectStart(float effectTime)
        {
            if (_freezingEffect is not null) _context.StopCoroutine(_freezingEffect);
            if (_context is null)
            {
                Debug.LogError("Context is null");
                return;
            }
            _freezingEffect = _context.StartCoroutine(FreezeTimeForSeconds(effectTime));
        }

        public void StopFreezeTimeEffect()
        {
            if (_freezingEffect is null) return;
            if (_pauseNotifier.IsPaused) _pausedTimeScale = 1.0f;
            else Time.timeScale = 1.0f;
        }

        private IEnumerator FreezeTimeForSeconds(float time)
        {
            float elapsedTime = 0;
            Time.timeScale = 0;
            while (elapsedTime < time)
            {
                if (_pauseNotifier.IsPaused)
                {
                    yield return null;
                    continue;
                }
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = 1.0f;
            _freezingEffect = null;
        }

        private void OnDestroy()
        {
            _pauseNotifier.Paused -= OnPause;
            _pauseNotifier.Resumed -= OnResume;
        }

        public void TimeStop()
        {
            IsTimeStopped = true;
            TimeStopped?.Invoke();
        }

        public void TimeContinue()
        {
            IsTimeStopped = false;
            TimeContinued?.Invoke();
        }

        private void OnResume()
        {
            Time.timeScale = _pausedTimeScale;
        }

        private void OnPause()
        {
            _pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
    }
}