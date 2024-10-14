using FMODUnity;
using Guns.Projectiles;
using UnityEngine;

namespace Guns.View
{
    public class GunAudio : GeneralGunView
    {
        [Header("General")] 
        [SerializeField] private StudioEventEmitter _shotEmitter;
        [SerializeField] private StudioEventEmitter _activateEmitter;
        [SerializeField] private StudioEventEmitter _deactivateEmitter;
        [SerializeField] private StudioEventEmitter _equipEmitter;
        [SerializeField] private StudioEventEmitter _reloadEmitter;
        
        public override void OnGunReloaded()
        {
            if (_reloadEmitter) _reloadEmitter.Play();
        }

        public override void OnGunEquip(Transform root)
        {
            if (_equipEmitter) _equipEmitter.Play();
        }

        public override void OnGunDeactivated()
        {
            if (_activateEmitter) _activateEmitter.Stop();
            if (_deactivateEmitter) _deactivateEmitter.Play();
        }

        public override void OnGunActivated()
        {
            if (_activateEmitter) _activateEmitter.Play();
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            if (_shotEmitter) _shotEmitter.Play();
        }
    }
}