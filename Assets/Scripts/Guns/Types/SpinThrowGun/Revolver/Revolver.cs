using System;
using System.Collections.Generic;
using System.Linq;
using Guns.General;
using Guns.Projectiles;
using Unity.VisualScripting;
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

        private void Awake()
        {
            for (int i = 0; i < Data.Config.MagSize; i++)
            {
                var capsuleHolder = new CapsuleHolder();
                capsuleHolder.Reload(_availableFactories[_availableProjectiles[0]]);
                Data.CapsuleHolders.Add(capsuleHolder);
            }
        }
        

        protected override ProjectileConfig OnShot()
        {
            try
            {
                var capsuleHolder = Data.CapsuleHolders.First(x => x.IsLoaded);
                var bullet = capsuleHolder.Shoot(transform.position, transform.forward);
                if (bullet is null) return null;
                bullet.Init(this);
                return bullet.Config;

            }
            catch (Exception)
            {
                Reload();
                return null;
            }
        }
    }
}