﻿using General.GlobalManagers;
using UnityEngine;
using Zenject;

namespace General
{
    public abstract class GamePlayBehaviour : MonoBehaviour
    {
        [SerializeField] private bool pauseable = true;
        [Inject] private IPauseNotifier _pauseNotifier;
        
        protected virtual void Start()
        {
            if (!pauseable) return;
            _pauseNotifier.Paused += OnPause;
            _pauseNotifier.Resumed += OnResume;
        }

        protected virtual void OnDestroy()
        {
            if (!pauseable) return;
            _pauseNotifier.Paused -= OnPause;
            _pauseNotifier.Resumed -= OnResume;
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