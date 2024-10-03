using FMODUnity;
using Guns.Data;
using Guns.General;
using Guns.Projectiles;
using TimeStop;
using UnityEngine;
using Zenject;

namespace Guns.View
{
    public class GunAudio : TimeStoppableBehaviour
    {
        [Header("General")] 
        [SerializeField] private StudioEventEmitter shotEmitter;
        [SerializeField] private StudioEventEmitter activateEmitter;
        [SerializeField] private StudioEventEmitter deactivateEmitter;
        [SerializeField] private StudioEventEmitter equipEmitter;
        [Header("Throw")] 
        [SerializeField] private StudioEventEmitter lostEmitter;
        [SerializeField] private StudioEventEmitter catchEmitter;
        [Header("Spin")] 
        [SerializeField] private StudioEventEmitter spinStartEmitter;
        [SerializeField] private StudioEventEmitter spinEndEmitter;

        private Gun _gun;

        [Inject]
        private void Construct(Gun gun, GunData data)
        {
            _gun = gun;
            gun.Shot += OnGunShot;
            gun.Activated += OnGunActivated;
            gun.Deactivated += OnGunDeactivated;
            gun.Equipped += OnGunEquip;

            if (gun is IThrowableGun throwableGun)
            {
                throwableGun.OnLost += OnGunLost;
                throwableGun.OnObtained += OnGunObtained;
            }

            if (gun is ISpinnableGun spinnableGun)
            {
                spinnableGun.SpinStarted += OnGunSpinStarted;
                spinnableGun.SpinEnded += OnGunSpinEnded;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _gun.Shot -= OnGunShot;
            _gun.Activated -= OnGunActivated;
            _gun.Deactivated -= OnGunDeactivated;
            _gun.Equipped -= OnGunEquip;

            if (_gun is IThrowableGun throwableGun)
            {
                throwableGun.OnLost -= OnGunLost;
                throwableGun.OnObtained -= OnGunObtained;
            }

            if (_gun is ISpinnableGun spinnableGun)
            {
                spinnableGun.SpinStarted -= OnGunSpinStarted;
                spinnableGun.SpinEnded -= OnGunSpinEnded;
            }
        }

        private void OnGunSpinEnded()
        {
            if (spinEndEmitter) spinEndEmitter.Play();
        }

        private void OnGunSpinStarted()
        {
            if (spinStartEmitter) spinStartEmitter.Play();
        }

        private void OnGunObtained()
        {
            if (catchEmitter) catchEmitter.Play();
        }

        private void OnGunLost()
        {
            if (lostEmitter) lostEmitter.Play();
        }

        private void OnGunEquip(Player.Player player)
        {
            if (equipEmitter) equipEmitter.Play();
        }

        private void OnGunDeactivated()
        {
            if (activateEmitter) activateEmitter.Stop();
            if (deactivateEmitter) deactivateEmitter.Play();
        }

        private void OnGunActivated()
        {
            if (activateEmitter) activateEmitter.Play();
        }

        private void OnGunShot(ProjectileConfig projectileConfig)
        {
            if (shotEmitter) shotEmitter.Play();
        }
        
    }
}