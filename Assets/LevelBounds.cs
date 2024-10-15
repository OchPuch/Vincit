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
                player.ApplyCheckpoint(saveData);
            }
        }
    }


}
