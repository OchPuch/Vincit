using System;
using Guns.General;
using TimeStop;
using UnityEngine;

namespace Guns.Bullets
{
    public abstract class Bullet : TimeStoppableBehaviour
    {
        [field: SerializeField] protected BulletConfig Config { get; private set; }
        public event Action BulletDestroyed;
        protected Gun Origin;
        
        public virtual void Init(Gun origin)
        {
            Origin = origin;
        }

        public virtual void ResetBullet()
        {
            
        }

        protected virtual void DestroyBullet()
        {
            BulletDestroyed?.Invoke();
        }
    }
}