using System;
using TimeStop;
using UnityEngine;

namespace Guns.Bullets
{
    public abstract class Bullet : TimeStoppableBehaviour
    {
        [field: SerializeField] protected BulletConfig Config { get; private set; }
        public event Action BulletDestroyed;
        
        public virtual void Init()
        {
            
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