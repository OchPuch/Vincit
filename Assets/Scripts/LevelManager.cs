using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

   
}
