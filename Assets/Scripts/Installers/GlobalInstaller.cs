using UnityEngine;
using Zenject;
using Application = UnityEngine.Device.Application;

namespace Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Debug.Log("GlobalInstaller Installed");
            Application.targetFrameRate = 240;
        }
    }
}