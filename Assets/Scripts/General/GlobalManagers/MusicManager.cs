using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace General.GlobalManagers
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private StudioEventEmitter musicEmitter;

        private void Start()
        {
            InitMusic();
        }

        private void InitMusic()
        {
            musicEmitter.Play();
        }
        
      
        
    }
}