using System;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace General.GlobalManagers
{
    public class LevelManager : MonoBehaviour
    {        
        public static LevelManager Instance { get; private set; }
        [SerializeField] private SceneField persistentGameplayScene;
        [SerializeField] private SceneField startScene;
        public int Reloads { get; private set; }
        public string LastLoadedLevel { get; private set; }
        public event Action LoadedNewLevel;
        public event Action LoadedCheckpoint;
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadNewLevel(startScene);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
        
        public void LoadNewLevel(string level)
        {
            CheckpointManager.Instance.DeleteCheckpoint();
            Reloads = 0;
            SceneManager.LoadScene(persistentGameplayScene);
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
            LastLoadedLevel = level;
            LoadedNewLevel?.Invoke();
        }
        
        public void LoadCheckpoint(SaveData saveData)
        { 
            Reloads += 1;
            SceneManager.LoadScene(persistentGameplayScene);
            SceneManager.LoadScene(saveData.SceneName, LoadSceneMode.Additive);
            LastLoadedLevel = saveData.SceneName;
            LoadedCheckpoint?.Invoke();
        }

        public void LoadScenes(SceneField[] scenesToLoad) {
            foreach (var scene in scenesToLoad) {
                if (!SceneUtils.IsSceneLoaded(scene)) {
                    SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                    LastLoadedLevel = scene;
                }
            }
        }

        public void UnloadScenes(SceneField[] scenesToUnload) {
            foreach (var t in scenesToUnload) {
                if (SceneUtils.IsSceneLoaded(t)) {
                    SceneManager.UnloadSceneAsync(t);
                }
            }
        }
        
        public void UnloadScenes(Scene[] scenesToUnload) {
            foreach (var t in scenesToUnload) {
                if (t.isLoaded) {
                    SceneManager.UnloadSceneAsync(t);
                }
            }
        }
        
        
    }
}
