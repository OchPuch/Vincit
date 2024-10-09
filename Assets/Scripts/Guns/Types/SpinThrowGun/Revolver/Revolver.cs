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
        [SerializeField] private List<ProjectileConfig> availableProjectiles;
        private Dictionary<ProjectileConfig ,ProjectileFactory> _availableFactories = new();

        public override void BindGun(DiContainer container)
        {
            container.Bind<Revolver>().FromInstance(this);
        }

        [Inject]
        private void Construct(DiContainer container)
        {
            foreach (var projectileConfig in availableProjectiles)
            {
                _availableFactories.Add(projectileConfig, container.ResolveId<ProjectileFactory>(projectileConfig.FactoryId));
            }
        }

        public override void Shoot()
        {
            if (IsLost) return;
            if (Data.fireTimer < Data.Config.FireRate) return;

            try
            {
                var capsuleHolder = Data.CapsuleHolders.First(x => x.IsLoaded);
                var bullet = capsuleHolder.Shoot(transform.position, transform.forward);
                bullet.Init(this);
                InvokeShot();
                Data.fireTimer = 0;
                
            }
            catch (Exception e)
            {
                Reload();
            }
        }
    }
}