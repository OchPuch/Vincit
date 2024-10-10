using System;
using General;
using Guns.Data;
using Guns.Projectiles;
using UnityEngine;
using UnityEngine.Video;
using Zenject;

namespace Guns.General
{
    public abstract class Gun : GamePlayBehaviour
    {
        [field: SerializeField] public Projectile Projectile { get; private set; }
        public Player.Player Owner { get; private set; }
        private ProjectileFactory ProjectileFactory { get; set; }
        
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
            ProjectileFactory = diContainer.ResolveId<ProjectileFactory>(Projectile.Config.FactoryId);
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
            if (Data.GunPunchCollider) Data.GunPunchCollider.enabled = false;
            Deactivated?.Invoke();
        }
        
        public virtual void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            if (Data.GunPunchCollider) Data.GunPunchCollider.enabled = true;
            Activated?.Invoke();
        }

        public virtual void Reload()
        {
            Reloaded?.Invoke();
        }
        

        protected void InvokeShot()
        {
            Shot?.Invoke(Projectile.Config);
        }
        
        public void Shoot()
        {
            if (!CanShot()) return;
            OnShot();
            InvokeShot();
            Data.FireTimer = 0;
        }

        protected virtual bool CanShot()
        {
            return !(Data.FireTimer < Data.Config.FireRate);
        }

        protected virtual void OnShot()
        {
            var bullet = ProjectileFactory.CreateProjectile(transform.position, transform.forward);
            bullet.Init(this);
        }

        
    }
}