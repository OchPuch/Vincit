using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace General.GlobalManagers
{
    public class LevelManager : MonoBehaviour
    {        
        void Start()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}
