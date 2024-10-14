using FMODUnity;
using UnityEngine;

namespace Player.View.Audio
{
    public class PlayerMovementAudio : MonoBehaviour
    {
        [SerializeField] private StudioEventEmitter jumpEmitter;
        
        public void UpdateFootstepSound()
        {
         
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

        }

        public void PlayDashSound()
        {
           
        }

        public void PlaySlamSound()
        {
            
        }

        public void StartSlideLoop()
        {
  
        }

        public void StopSlideLoop()
        {
 
        }

        public void StopAllSounds()
        {

        }

     
    }
}