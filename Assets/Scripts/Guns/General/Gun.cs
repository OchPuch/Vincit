using System;
using General;
using Guns.Bullets;
using Guns.Data;
using UnityEngine;
using Utils;
using Zenject;

namespace Guns.General
{
    public abstract class Gun : GamePlayBehaviour
    {
        [field: SerializeField] public Bullet Projectile { get; private set; }
        public Player.Player Owner { get; private set; }
        private BulletFactory BulletFactory { get; set; }
        
        public bool IsActive { get; private set; }
        
        protected GunData Data;
        public event Action Shot;
        public event Action Equipped;
        public event Action Activated;
        public event Action Deactivated;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            BulletFactory = diContainer.ResolveId<BulletFactory>(Projectile.Config.FactoryId);
        }
        
        public virtual void Init(GunData data)
        {
            Data = data;
        }
        
        protected virtual void Update()
        {
            Data.fireTimer += Time.deltaTime;
        }
        
        public void Equip(Player.Player owner)
        {
            Owner = owner;
            Equipped?.Invoke();
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            Deactivated?.Invoke();
        }
        
        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            Activated?.Invoke();
        }

        protected void InvokeShot()
        {
            Shot?.Invoke();
        }
        
        public virtual void Shoot()
        {
            if (Data.fireTimer < Data.Config.FireRate) return;
            var bullet = BulletFactory.CreateBullet(transform.position, transform.forward);
            bullet.Init(this);
            InvokeShot();
            Data.fireTimer = 0;

        }
    }
}