using General.GlobalManagers;
using KinematicCharacterController.Core;
using UnityEngine;
using Zenject;

namespace General
{
    public class ManagersInstaller : MonoInstaller
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TimeController timeController;
        [SerializeField] private KinematicCharacterSystem kcc;
        [SerializeField] private AudioManager audioManager;
        
        private PauseManager _pauseManager;
        private TimeManager _timeManager;
        public override void InstallBindings()
        {
            _pauseManager = new PauseManager();
            _timeManager = new TimeManager(timeController, _pauseManager);
            Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PauseManager>().FromInstance(_pauseManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TimeManager>().FromInstance(_timeManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TimeController>().FromInstance(timeController).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<KinematicCharacterSystem>().FromInstance(kcc).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AudioManager>().FromInstance(audioManager).AsSingle().NonLazy();
        }
        
    }
}
