using Environment;
using General.GlobalManagers;
using UnityEngine;
using Utils;

namespace Levels
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelDoor : PlayerTrigger
    {
        [SerializeField] private SceneField mainMenu;
        [SerializeField] private SceneField nextScene;
        private bool _entered;

        protected override void OnPlayerEnter(Player.Player player)
        {
            if (_entered) return;
            if (string.IsNullOrEmpty(nextScene.SceneName))
            {
                LevelManager.Instance.LoadNewLevel(mainMenu);
            }
            else
            {
                LevelManager.Instance.LoadNewLevel(nextScene);
            }
            _entered = true;
        }
    }
}