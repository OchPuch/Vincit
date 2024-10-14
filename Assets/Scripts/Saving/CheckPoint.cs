using System;
using Environment;
using UnityEngine;

namespace Saving
{
    public class CheckPoint : PlayerTrigger
    {
        [SerializeField] private bool clearCheckpointsInstead;
        private string _sceneName;
        private bool _set;
        public event Action CheckPointSet;
        
        protected override void Start()
        {
            base.Start();
            _sceneName = gameObject.scene.name;
        }

        protected override void OnPlayerEnter(Player.Player player1)
        {
            if (_set) return;
            if (clearCheckpointsInstead)
            {
                CheckpointManager.Instance.DeleteCheckpoint();
                return;
            }
            
            var saveData = player1.GetCharacterSaveData();
            CheckpointManager.Instance.SetCheckpoint(new SaveData
            {
                SceneName = _sceneName,
                PlayerSaveData = saveData,
            }); 
            _set = true;
            CheckPointSet?.Invoke();
        }
    }
}

