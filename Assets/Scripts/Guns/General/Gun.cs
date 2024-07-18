using System;
using Guns.Bullets;
using Guns.Data;
using UnityEngine;
using Utils;

namespace Guns.General
{
    public abstract class Gun : GamePlayBehaviour
    {
        protected GunData Data;
        protected BulletFactory BulletFactory;
        public event Action Shot;
        public event Action Equipped;
        public event Action Activated;
        public event Action Deactivated;
        
        public void Init(GunData data)
        {
            Data = data;
            BulletFactory = new BulletFactory(Data.Config.Projectile, this);
        }
        
        private void Update()
        {
            Data.fireTimer += Time.deltaTime;
        }

        public void HandleInput(GunInput input)
        {
            if (input.ShootRequest && Data.fireTimer > Data.Config.FireRate)
            {
                Shoot();
                Shot?.Invoke();
                Data.fireTimer = 0;
            }
        }

        public void Equip()
        {
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

        protected virtual void Shoot()
        {
            var bullet = BulletFactory.CreateBullet(transform.position);
            bullet.Init();
        }
    }
}