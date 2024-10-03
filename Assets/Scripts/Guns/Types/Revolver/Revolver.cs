using System.Collections.Generic;
using Guns.General;
using Guns.Projectiles;
using UnityEngine;
using Zenject;

namespace Guns.Types.Revolver
{
    public class Revolver : SpinThrowGun
    {
        [SerializeField] private List<ProjectileConfig> availableProjectiles;
        private Dictionary<ProjectileConfig ,ProjectileFactory> _availableFactories = new();
        
        [Inject]
        private void Construct(DiContainer container)
        {
            foreach (var projectileConfig in availableProjectiles)
            {
                _availableFactories.Add(projectileConfig, container.ResolveId<ProjectileFactory>(projectileConfig.FactoryId));
            }
        }
        
        
    }
}