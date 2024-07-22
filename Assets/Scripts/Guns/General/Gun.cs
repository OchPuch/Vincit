using System;
using Guns.Bullets;
using Guns.Data;
using UnityEngine;
using Utils;

namespace Guns.General
{
    public abstract class Gun : GamePlayBehaviour
    {
        public Player.Player Owner { get; private set; }
        protected GunData Data;
        public BulletFactory BulletFactory { get; private set; }
        public event Action Shot;
        public event Action Equipped;
        public event Action Activated;
        public event Action Deactivated;
        
        public virtual void Init(GunData data)
        {
            Data = data;
            BulletFactory = new BulletFactory(Data.Config.Projectile, this);
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
            var bullet = BulletFactory.CreateBullet();
            bullet.Init(this);
        }
    }
}