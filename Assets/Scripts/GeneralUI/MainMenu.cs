
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace GeneralUI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button startButton;
        [SerializeField] private Button levelsButton;
        /*
        [Header("Scenes")]
        [SerializeField] private SceneField persistentScene;
        [SerializeField] private SceneField firstLevelScene;
        [Space(10)]
        [SerializeField] private SceneField persistentPoor;
        [SerializeField] private SceneField tutorialScene;

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
            {
                PlayerPrefs.SetString("PlayerName", "Player");
                LoadTutorial();
            }
            else
            {
                startButton.gameObject.SetActive(false);
            }

            continueButton.interactable = CheckpointManager.Instance.TryGetLastCheckPoint(out var _);
        }

        public void StartNewGame()
        {
            MusicManager.Instance.SwitchMusic(musicSwitchData);
            LevelManager.Instance.LoadNewLevel(firstLevelScene);
        }

        
        //TODO: LOAD MUSIC FROM LEVEL DATA
        public void Continue()
        { 
            MusicManager.Instance.SwitchMusic(musicSwitchData);
            SceneManager.LoadScene(persistentScene);
            SceneManager.LoadScene(CheckpointManager.Instance.GetLastCheckpoint().sceneName, LoadSceneMode.Additive);
        }
        
        private void LoadTutorial()
        {
            SceneManager.LoadScene(persistentPoor);
            SceneManager.LoadScene(tutorialScene, LoadSceneMode.Additive);
        }

        public void Exit()
        {
            Application.Quit();
        }
        
        */
    }
}
