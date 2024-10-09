using FMODUnity;
using Guns.Projectiles;
using UnityEngine;

namespace Guns.View
{
    public class GunAudio : GeneralGunView
    {
        [Header("General")] [SerializeField] private StudioEventEmitter shotEmitter;
        [SerializeField] private StudioEventEmitter activateEmitter;
        [SerializeField] private StudioEventEmitter deactivateEmitter;
        [SerializeField] private StudioEventEmitter equipEmitter;
       
        public override void OnGunEquip(Transform root)
        {
            if (equipEmitter) equipEmitter.Play();
        }

        public override void OnGunDeactivated()
        {
            if (activateEmitter) activateEmitter.Stop();
            if (deactivateEmitter) deactivateEmitter.Play();
        }

        public override void OnGunActivated()
        {
            if (activateEmitter) activateEmitter.Play();
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            if (shotEmitter) shotEmitter.Play();
        }
    }
}