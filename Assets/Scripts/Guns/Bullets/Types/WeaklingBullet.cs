using UnityEngine;

namespace Guns.Bullets.Types
{
    public class WeaklingBullet : HitscanBullet
    {
        [SerializeField][Min(1)] private int maxOverloads;
        [SerializeField] private float minBulletSize;
        
        protected override void OnBulletPunchedWithNewBullet(HitscanBullet bullet)
        {
            base.OnBulletPunchedWithNewBullet(bullet);
            if (bullet.ConsumeData.Overloads < maxOverloads)
            {
                bullet.ConsumeData.Scale -=
                    ConsumeData.Scale * (1 - (float)bullet.ConsumeData.Overloads / maxOverloads);
                if (bullet.ConsumeData.Scale < minBulletSize) bullet.ConsumeData.Scale = minBulletSize;
            }
            else
            {
                bullet.Overload();
            }
        }
    }
    
}