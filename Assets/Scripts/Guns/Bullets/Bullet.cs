using System;
using Guns.General;
using TimeStop;
using UnityEngine;

namespace Guns.Bullets
{
    public class Bullet : TimeStoppableBehaviour
    {
        [field: SerializeField] public BulletConfig Config { get; private set; }
        public event Action BulletDestroyed;
        protected Gun Origin;
        
        public virtual void Init(Gun origin)
        {
            Origin = origin;
        }

        public virtual void ResetBullet()
        {
            
        }

        public virtual void DestroyBullet()
        {
            if (!gameObject.activeSelf) return;
            BulletDestroyed?.Invoke();
        }
    }
}