using System;
using General.GlobalManagers;
using Saving;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player.Player player))
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
        }
    }


}
