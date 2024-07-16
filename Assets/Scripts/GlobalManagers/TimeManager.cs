using UnityEngine;

namespace GlobalManagers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }
        
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
            
            PauseManager.Instance.Paused += OnPause;
            PauseManager.Instance.Resumed += OnResume;
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            
            Instance = null;
            PauseManager.Instance.Paused -= OnPause;
            PauseManager.Instance.Resumed -= OnResume;
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