using Guns.General;
using KinematicCharacterController.Examples;
using Player.Data;
using Player.View;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerBootstrap : MonoInstaller
    {
        public PlayerData playerData = new PlayerData();
        public PlayerController character;
        public ExampleCharacterCamera characterCamera;
        public Player player;

        
        public override void InstallBindings()
        { 
            player.Init(playerData, character, characterCamera);
            
            Container.Bind<PlayerData>().FromInstance(playerData);
            Container.Bind<Player>().FromInstance(player);
            
            
            // Tell camera to follow transform
            characterCamera.SetFollowTransform(playerData.cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            characterCamera.IgnoredColliders.Clear();
            characterCamera.IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());


        }
    } 
}
