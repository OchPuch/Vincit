using UnityEngine;

namespace Guns.Bullets.Types
{
    public class WeaklingProjectile : HitscanProjectile
    {
        [SerializeField][Min(1)] private int maxOverloads;
        [SerializeField] private float minBulletSize;
        
        protected override void OnBulletPunchedWithNewBullet(HitscanProjectile projectile)
        {
            base.OnBulletPunchedWithNewBullet(projectile);
            if (projectile.ConsumeData.Overloads < maxOverloads)
            {
                projectile.ConsumeData.Scale -=
                    ConsumeData.Scale * (1 - (float)projectile.ConsumeData.Overloads / maxOverloads);
                if (projectile.ConsumeData.Scale < minBulletSize) projectile.ConsumeData.Scale = minBulletSize;
            }
            else
            {
                projectile.Overload();
            }
        }
    }
    
}