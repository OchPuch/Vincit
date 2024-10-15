using General.GlobalManagers;
using Saving;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Utils;

namespace GeneralUI
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Scenes")] [SerializeField] private SceneField mainMenu;
        [Header("UI Objects")] [SerializeField]
        private Button _checkpointButton;
        [SerializeField] private GameObject pauseMenu;
        [Header("Sound Effects")] 
        [SerializeField] private Slider sfx;
        [SerializeField] private Slider music;
        [SerializeField] private Slider master;
        
        private AudioManager _audioManager;
        private PauseManager _pauseManager;
        private Player.Player _player;
        
        [Inject]
        private void Construct(AudioManager audioManager, PauseManager pauseManager, Player.Player player)
        {
            _pauseManager = pauseManager;
            _audioManager = audioManager;
            _player = player;
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
            _checkpointButton.interactable = CheckpointManager.Instance.TryGetLastCheckPoint(out var saveData);
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
            pauseMenu.SetActive(false);
            LevelManager.Instance.LoadNewLevel(mainMenu);
            UnPause();
        }

        public void Checkpoint()
        {
            if (CheckpointManager.Instance.TryGetLastCheckPoint(out var data))
            {
                _player.ApplyCheckpoint(data);
            }

            UnPause();
        }

        public void Restart()
        {
            if (CheckpointManager.Instance.TryGetLastCheckPoint(out var saveData))
            {
                var sceneName = saveData.SceneName;
                CheckpointManager.Instance.DeleteCheckpoint();
                LevelManager.Instance.LoadNewLevel(sceneName);
            }
            else
            {
                LevelManager.Instance.LoadNewLevel(LevelManager.Instance.LastLoadedLevel);
            }

            UnPause();
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