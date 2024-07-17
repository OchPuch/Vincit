using System;
using UnityEngine;

namespace GlobalManagers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }
        public event Action TimeStopped;
        public event Action TimeContinued;
        
        public bool IsTimeStopped { get; private set; }
        
        private float _pausedTimeScale = 1.0f;

        public void Init()
        {
            if (Instance == this) return;
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            if (PauseManager.Instance is null) return;
            PauseManager.Instance.Paused += OnPause;
            PauseManager.Instance.Resumed += OnResume;
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            
            Instance = null;
            if (PauseManager.Instance is null) return;
            PauseManager.Instance.Paused -= OnPause;
            PauseManager.Instance.Resumed -= OnResume;
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