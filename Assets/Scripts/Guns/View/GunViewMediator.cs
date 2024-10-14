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
            gun.Reloaded += OnReload;
            gun.StopReload += OnStopReload;
        }
        

        private void OnDestroy()
        {
           _gun.Shot -= OnGunShot;
           _gun.Activated -= OnGunActivated;
           _gun.Deactivated -= OnGunDeactivated;
           _gun.Equipped -= OnGunEquip;
           _gun.Reloaded -= OnReload;
           _gun.StopReload -= OnStopReload;
           foreach (var capsuleHolder in _gun.Data.CapsuleHolders)
           {
               capsuleHolder.Reloaded -= OnCapsuleReload;
           }
        }

        protected abstract void OnCapsuleReload(ProjectileConfig obj);

        protected virtual void OnGunEquip(Player.Player obj)
        {
            foreach (var capsuleHolder in _gun.Data.CapsuleHolders)
            {
                capsuleHolder.Reloaded += OnCapsuleReload;
            }
        }
        
        protected abstract void OnGunShot(ProjectileConfig obj);
        
        protected virtual void OnGunDeactivated()
        {
            enabled = false;
        }

        protected virtual void OnGunActivated()
        {
            enabled = true;
        }

        protected abstract void OnStopReload();

        protected abstract void OnReload();

    }
}