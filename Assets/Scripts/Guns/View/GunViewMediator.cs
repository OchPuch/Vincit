using Guns.General;
using Guns.Projectiles;
using UnityEngine;
using Zenject;

namespace Guns.View
{
    public abstract class GunViewMediator : MonoBehaviour
    {
        private Gun _gun;
        
        [Inject]
        private void Construct(Gun gun)
        {
            _gun = gun;
            gun.Shot += OnGunShot;
            gun.Activated += OnGunActivated;
            gun.Deactivated += OnGunDeactivated;
            gun.Equipped += OnGunEquip;
        }

        private void OnDestroy()
        {
           _gun.Shot -= OnGunShot;
           _gun.Activated -= OnGunActivated;
           _gun.Deactivated -= OnGunDeactivated;
           _gun.Equipped -= OnGunEquip;
        }

        protected abstract void OnGunEquip(Player.Player obj);
        
        protected abstract void OnGunShot(ProjectileConfig obj);
        
        protected virtual void OnGunDeactivated()
        {
            enabled = false;
        }

        protected virtual void OnGunActivated()
        {
            enabled = true;
        }
    }
}