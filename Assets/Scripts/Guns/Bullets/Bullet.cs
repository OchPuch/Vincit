using System;
using TimeStop;

namespace Guns.Bullets
{
    public abstract class Bullet : TimeStoppableBehaviour
    {
        public event Action BulletDestroyed;
        
        public virtual void Init()
        {
            
        }

        public void ResetBullet()
        {
            
        }
    }
}