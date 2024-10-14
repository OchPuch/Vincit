using Guns.General;
using KinematicCharacterController.Examples;
using Player.Data;
using Player.View;
using TimeStop;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerBootstrap : MonoInstaller
    {
        public PlayerData playerData = new();
        public PlayerController character;
        public ExampleCharacterCamera characterCamera;
        public Player player;
        public TimeStopAbility TimeStopAbility;

        
        public override void InstallBindings()
        { 
            player.Init(playerData, character, characterCamera);

            Container.Bind<TimeStopAbility>().FromInstance(TimeStopAbility);
            Container.Bind<PlayerData>().FromInstance(playerData);
            Container.Bind<Player>().FromInstance(player);
            Container.Bind<PlayerController>().FromInstance(character);
            
            
            // Tell camera to follow transform
            characterCamera.SetFollowTransform(playerData.cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            characterCamera.IgnoredColliders.Clear();
            characterCamera.IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());


        }
    } 
}
