using Player.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovementAudio : MonoBehaviour
    {
        [Header("Config")] [SerializeField] private PlayerAudioConfig audioConfig;
        [Space(10)] [SerializeField] private float footstepDelay = 0.5f;
        [SerializeField] private float walkPitchRandomRange = 0.2f;
        [SerializeField] private float wallJumpPitchAddition = 0.2f;

        [Header("Components")] [SerializeField]
        private AudioSource audioSource;

        private int _footstepIndex = 0;
        private float _footstepTimer = 0f;

        public void UpdateFootstepSound()
        {
            if (_footstepTimer > 0)
            {
                _footstepTimer -= Time.deltaTime;
            }

            if (_footstepTimer <= 0)
            {
                PlayFootstepSound();
                StartFootstepTimer();
            }
        }

        private void StartFootstepTimer()
        {
            _footstepTimer = footstepDelay;
        }

        private void PlayFootstepSound()
        {
            //randomize pitch
            audioSource.pitch = Random.Range(1f - walkPitchRandomRange, 1f + walkPitchRandomRange);
            audioSource.PlayOneShot(audioConfig.footstepSounds[_footstepIndex]);
            _footstepIndex = (_footstepIndex + 1) % audioConfig.footstepSounds.Length;
        }


        public void PlayJumpSound()
        {
            NormalizePitch();
            audioSource.PlayOneShot(audioConfig.jumpSound);
        }

        public void PlayLandSound()
        {
            NormalizePitch();
            audioSource.PlayOneShot(audioConfig.landSound);
        }

        public void PlayWallJumpSound(int index)
        {
            NormalizePitch();
            if (index < audioConfig.wallJumpSound.Length)
                audioSource.PlayOneShot(audioConfig.wallJumpSound[index]);
            else if (audioConfig.wallJumpSound.Length > 0)
            {
                var pitch = 1 + ((audioConfig.wallJumpSound.Length - index) + 1) * wallJumpPitchAddition;
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioConfig.wallJumpSound[^1]);
            }
        }

        public void PlayDashSound()
        {
            NormalizePitch();
            audioSource.PlayOneShot(audioConfig.dashSound);
        }

        public void PlaySlamSound()
        {
            NormalizePitch();
            audioSource.PlayOneShot(audioConfig.slamSound);
        }

        public void StartSlideLoop()
        {
            NormalizePitch();
            audioSource.PlayOneShot(audioConfig.startSlideSound);
            audioSource.clip = audioConfig.slideSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void StopSlideLoop()
        {
            NormalizePitch();
            audioSource.loop = false;
            audioSource.Stop();
        }

        public void StopAllSounds()
        {
            NormalizePitch();
            audioSource.Stop();
        }

        private void NormalizePitch()
        {
            audioSource.pitch = 1f;
        }
    }
}