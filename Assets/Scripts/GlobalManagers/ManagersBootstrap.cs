using UnityEngine;

namespace GlobalManagers
{
    public class ManagersBootstrap : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private GameManager gameManager;
        private void Awake()
        {
            gameManager.Init();
            pauseManager.Init();
            timeManager.Init();
        }
    }
}
