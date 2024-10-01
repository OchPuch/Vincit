using General.GlobalManagers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GeneralUI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [Header("Sound Effects")]
        [SerializeField] private Slider sfx;
        [SerializeField] private Slider music;
        [SerializeField] private Slider master;

        private AudioManager _audioManager;
        private PauseManager _pauseManager;

        [Inject]
        private void Construct(AudioManager audioManager, PauseManager pauseManager)
        {
            _pauseManager = pauseManager;
            _audioManager = audioManager;
        }

        private void Start()
        {
            sfx.value = _audioManager.GetSoundEffectsVolume01();
            music.value = _audioManager.GetMusicVolume01();
            master.value = _audioManager.GetMasterVolume01();
            
            sfx.onValueChanged.AddListener(UpdateSoundEffectsVolume);
            music.onValueChanged.AddListener(UpdateMusicVolume);
            master.onValueChanged.AddListener(UpdateMasterVolume);
            
            UnPause();
        }
        
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (_pauseManager.IsPaused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            _pauseManager.Pause();
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
        }

        public void UnPause()
        {
            _pauseManager.Resume();
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
        }
    
        public void LoadMainMenu()
        {
            
        }

        public void Checkpoint()
        {
            
        }

        public void Restart()
        {
            
        }

        private void UpdateSoundEffectsVolume(float value)
        {
            _audioManager.UpdateSfxVolume(value);
        }

        private void UpdateMusicVolume(float value)
        {
            _audioManager.UpdateMusicVolume(value);
        }

        private void UpdateMasterVolume(float value)
        {
            _audioManager.UpdateMasterVolume(value);
        }


    }
}
