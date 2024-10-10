using General;
using Guns.Projectiles;
using UnityEngine;

namespace Guns.View
{
    public abstract class GeneralGunView : GamePlayBehaviour
    {
        
        
        public abstract void OnGunEquip(Transform root);
        public abstract void OnGunDeactivated();
        public abstract void OnGunActivated();
        public abstract void OnGunShot(ProjectileConfig projectileConfig);
        
    }
}