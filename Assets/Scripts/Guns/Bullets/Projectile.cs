using System;
using Guns.General;
using TimeStop;
using UnityEngine;

namespace Guns.Bullets
{
    public class Projectile : TimeStoppableBehaviour
    {
        [field: SerializeField] public ProjectileConfig Config { get; private set; }
        public event Action BulletDestroyed;
        protected Gun Origin;
        
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