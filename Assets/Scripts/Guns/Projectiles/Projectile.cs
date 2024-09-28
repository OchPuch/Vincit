using System;
using Guns.General;
using Guns.Projectiles.Types;
using TimeStop;
using UnityEngine;

namespace Guns.Projectiles
{
    public class Projectile : TimeStoppableBehaviour
    {
        [field: SerializeField] public ProjectileConfig Config { get; private set; }
        [field: SerializeField] protected Collider BulletCollider { get; private set; }
        public event Action BulletDestroyed;
        protected Gun Origin;
        public ConsumeData ConsumeData { get; set; }
        
        public virtual void Init(Gun origin)
        {
            Origin = origin;
        }

        public virtual void ResetBullet()
        {
            
        }

        public virtual void DestroyProjectile()
        {
            if (!gameObject.activeSelf) return;
            BulletDestroyed?.Invoke();
        }

       
    }
}