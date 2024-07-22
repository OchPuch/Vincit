using Guns.General;
using KinematicCharacterController.Examples;
using Player.Data;
using UnityEngine;

namespace Player
{
    public class PlayerBootstrap : MonoBehaviour
    {
        public PlayerData playerData = new PlayerData();
        public PlayerController character;
        public ExampleCharacterCamera characterCamera;
        public Player player;
        public GunController gunController;
        
        private void Start()
        {
            character.Init(playerData);
            player.Init(playerData, character, characterCamera);
            
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            characterCamera.SetFollowTransform(playerData.cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            characterCamera.IgnoredColliders.Clear();
            characterCamera.IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());

            gunController.Init(player);
        }
    } 
}
