using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneUtils
    {
        public static bool IsSceneLoaded(string sceneName)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name != sceneName) continue;
                return true;
            }
            return false;
        }
        
        public static bool IsSceneLoaded(SceneField scene)
        {
            return IsSceneLoaded(scene.SceneName);
        }
        
        public static bool IsSceneLoaded(Scene scene)
        {
            return IsSceneLoaded(scene.name);
        }
        
        
        
        
    }
}