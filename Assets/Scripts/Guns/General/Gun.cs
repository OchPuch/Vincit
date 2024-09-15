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
        public BulletFactory BulletFactory { get; private set; }
        
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

        public virtual void HandleInput(GunInput input)
        {
            if (input.ShootRequest && Data.fireTimer > Data.Config.FireRate)
            {
                Shoot();
                InvokeShot();
                Data.fireTimer = 0;
            }
        }

        public void Equip(Player.Player owner)
        {
            Owner = owner;
            Equipped?.Invoke();
        }

        public void Deactivate()
        {
            Deactivated?.Invoke();
        }
        
        public void Activate()
        {
            Activated?.Invoke();
        }

        protected void InvokeShot()
        {
            Shot?.Invoke();
        }
        
        protected virtual void Shoot()
        {
            var bullet = BulletFactory.CreateBullet(transform.position, transform.forward);
            bullet.Init(this);
        }
    }
}