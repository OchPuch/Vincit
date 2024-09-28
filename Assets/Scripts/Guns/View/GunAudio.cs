using FMODUnity;
using General;
using Guns.Data;
using Guns.General;
using UnityEngine;

namespace Guns.View
{
    public class GunAudio : GamePlayBehaviour
    {
        [SerializeField] private StudioEventEmitter studioEventEmitter;

        private Gun _gun;
        
        public void Init(Gun gun, GunData data)
        {
            _gun = gun;

            gun.Shot += OnGunShot;
        }

        private void OnGunShot()
        {
            studioEventEmitter.Play();
        }
    }
}