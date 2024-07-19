using UnityEngine;

namespace GlobalManagers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [field: SerializeField] public GameSettings GameSettings { get; private set; }
        public void Init()
        {
            if (Instance == this) return;
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}