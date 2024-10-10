using System;
using System.Collections.Generic;
using System.Linq;
using Guns.Projectiles;
using UnityEngine;
using Zenject;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class Revolver : SpinThrowGun
    {
        [SerializeField] private List<ProjectileConfig> _availableProjectiles;
        private Dictionary<ProjectileConfig ,ProjectileFactory> _availableFactories = new();

        [Inject]
        private void Construct(DiContainer container)
        {
            foreach (var projectileConfig in _availableProjectiles)
            {
                _availableFactories.Add(projectileConfig, container.ResolveId<ProjectileFactory>(projectileConfig.FactoryId));
            }
        }

        public override void Shoot()
        {
            if (IsLost) return;
            if (Data.FireTimer < Data.Config.FireRate) return;
            try
            {
                var capsuleHolder = Data.CapsuleHolders.First(x => x.IsLoaded);
                var bullet = capsuleHolder.Shoot(transform.position, transform.forward);
                bullet.Init(this);
                InvokeShot();
                Data.FireTimer = 0;
                
            }
            catch (Exception)
            {
                Reload();
            }
        }
    }
}