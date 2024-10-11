using System;
using System.Collections.Generic;
using System.Linq;
using General;
using Guns.Data;
using Guns.Projectiles;
using UnityEngine;
using Zenject;

namespace Guns.General
{
    public abstract class Gun : GamePlayBehaviour
    {
        public Player.Player Owner { get; private set; }
        
        private Dictionary<ProjectileConfig ,ProjectileFactory> _availableFactories = new();
        
        //TODO: Replace with reactive bools
        public bool IsActive { get; private set; }
        public GunData Data { get; private set; }
        public event Action<ProjectileConfig> Shot;
        public event Action Reloaded;
        public event Action<Player.Player> Equipped;
        public event Action Activated;
        public event Action Deactivated;
        
        [Inject]
        private void Construct(DiContainer diContainer, GunData data)
        {
            Data = data;
            
            foreach (var projectileConfig in Data.AvailableProjectiles)                                                                
            {                                                                                                                      
                _availableFactories.Add(projectileConfig, diContainer.ResolveId<ProjectileFactory>(projectileConfig.FactoryId));     
            }                                                                                                                      

            for (int i = 0; i < Data.Config.MagSize; i++)
            {
                var capsuleHolder = new CapsuleHolder();
                capsuleHolder.Reload(_availableFactories[Data.AvailableProjectiles[0]]);
                Data.CapsuleHolders.Add(capsuleHolder);
            }
            
        }
        
        protected virtual void Update()
        {
            Data.FireTimer += Time.deltaTime;
        }
        
        public void Equip(Player.Player owner)
        {
            Owner = owner;
            Equipped?.Invoke(owner);
        }

        public virtual void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            enabled = false;
            if (Data.GunPunchCollider) Data.GunPunchCollider.enabled = false;
            Deactivated?.Invoke();
        }
        
        public virtual void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            enabled = true;
            if (Data.GunPunchCollider) Data.GunPunchCollider.enabled = true;
            Activated?.Invoke();
        }

        public void Reload()
        {
            OnReload();
            Reloaded?.Invoke();
        }

        private void OnReload()
        {
            foreach (var capsuleHolder in Data.CapsuleHolders)
            {
                capsuleHolder.ReloadSame();
            }

            Data.FireTimer = -Data.Config.ReloadTIme;
        }

        private void InvokeShot(ProjectileConfig config)
        {
            Shot?.Invoke(config);
        }
        
        public void Shoot()
        {
            if (!CanShot()) return;
            var bulletConfig = OnShot();
            if (bulletConfig is null) return;
            InvokeShot(bulletConfig);
            Data.FireTimer = 0;
        }

        protected virtual bool CanShot()
        {
            return !(Data.FireTimer < Data.Config.FireRate);
        }

        protected virtual ProjectileConfig OnShot()
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