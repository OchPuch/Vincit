using System;
using FMODUnity;
using UnityEngine;

namespace Player.View.Audio
{
    public class PlayerMovementAudio : MonoBehaviour
    {
        [SerializeField] private StudioEventEmitter jumpEmitter;
        [SerializeField] private StudioEventEmitter wallJumpEmitter;
        [SerializeField] private StudioEventEmitter slideEmitter;
        [SerializeField] private StudioEventEmitter footStepEmitter;


        public void StartFootstepSound()
        {
            footStepEmitter.Play();
        }
        
        public void EndFootStepSound()
        {
            footStepEmitter.Stop();
        }

        private void StartFootstepTimer()
        {
        }

        private void PlayFootstepSound()
        {
        }


        public void PlayJumpSound()
        {
            if (jumpEmitter) jumpEmitter.Play();
        }

        public void PlayLandSound()
        {
        
        }

        public void PlayWallJumpSound(int index)
        {
            if (wallJumpEmitter) wallJumpEmitter.Play();
        }

        public void PlayDashSound()
        {
           
        }

        public void PlaySlamSound()
        {
            
        }

        public void StartSlideLoop()
        {
            if (slideEmitter) slideEmitter.Play();
        }

        public void StopSlideLoop()
        {
            if (slideEmitter) slideEmitter.Stop();
        }

        public void StopAllSounds()
        {

        }


      
    }
}