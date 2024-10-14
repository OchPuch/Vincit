using Sirenix.OdinInspector;
using UnityEngine;

namespace Saving
{
    public class CheckpointManager : MonoBehaviour
    {
        public static CheckpointManager Instance { get; private set; }
        private SaveData _lastSave;
        
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
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
        
        public void SetCheckpoint(SaveData saveData)
        {
            Database.SaveCheckpoint(saveData);
            _lastSave = saveData;
        }
        
        [Button("LoadCheckpoint")]
        public bool LoadCheckpoint()
        {
            var checkpointData = Database.LoadCheckpoint();
            if (checkpointData != null)
            {
                _lastSave = checkpointData.Value;
                return true;
            }
            else
            {
                Debug.Log("No checkpoint found");
                return false;
            }
        }
        
        [Button("DeleteCheckpoint")]
        public void DeleteCheckpoint()
        {
            Database.DeleteCheckpoint();
            _lastSave = new SaveData();
        }
        
        public SaveData GetLastCheckpoint()
        {
            if (_lastSave.SceneName is null or "")
            {
                LoadCheckpoint();
            }
            return _lastSave;
        }

        public bool TryGetLastCheckPoint(out SaveData saveData)
        {
            if (_lastSave.SceneName is null or "")
            {
                bool loaded = LoadCheckpoint();
                saveData = _lastSave;
                return loaded;
            }
            
            if (_lastSave.SceneName is null or "")
            {
                saveData = _lastSave;
                return false;
            }

            saveData = _lastSave;
            return true;
        }
    }
}