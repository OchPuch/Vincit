using GlobalManagers;
using UnityEngine;

namespace Utils
{
    public abstract class GamePlayBehaviour : MonoBehaviour
    {
        [SerializeField] private bool pauseable = true;

        protected virtual void Start()
        {
            if (!pauseable) return;
            if (PauseManager.Instance == null)
            {
                Debug.LogError("Pause manager instance is missing");
                return;
            }
            PauseManager.Instance.Paused += OnPause;
            PauseManager.Instance.Resumed += OnResume;
        }

        protected virtual void OnDestroy()
        {
            if (!pauseable) return;
            if (PauseManager.Instance == null) return;
            PauseManager.Instance.Paused -= OnPause;
            PauseManager.Instance.Resumed -= OnResume;
        }

        private void OnPause()
        {
            if (!pauseable) return;
            enabled = false;
            PostPause();
        }


        private void OnResume()
        {
            if (!pauseable) return;
            enabled = true;
            PostResume();
        }
        
        protected virtual void PostPause() {}
        
        protected virtual void PostResume() {}


    }
}