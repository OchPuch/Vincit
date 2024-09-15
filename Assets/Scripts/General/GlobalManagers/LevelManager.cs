using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalManagers
{
    public class LevelManager : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}
