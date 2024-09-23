using General.GlobalManagers;
using UnityEngine;
using Zenject;

namespace General
{
    public abstract class GamePlayBehaviour : MonoBehaviour
    {
        [SerializeField] private bool pauseable = true;
        [Inject] protected IPauseNotifier PauseNotifier;
        
        protected virtual void Start()
        {
            if (!pauseable) return;
            PauseNotifier.Paused += OnPause;
            PauseNotifier.Resumed += OnResume;
        }

        protected virtual void OnDestroy()
        {
            if (!pauseable) return;
            if (PauseNotifier is null) return;
            PauseNotifier.Paused -= OnPause;
            PauseNotifier.Resumed -= OnResume;
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