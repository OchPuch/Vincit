using FMODUnity;
using Guns.Types.SpinThrowGun;
using Guns.View;
using UnityEngine;

namespace Guns.Types.Revolver
{
    public class SpinThrowGunAudio : GunAudio, ISpinObserver, IThrowObserver
    {
        [Header("Throw")] 
        [SerializeField] private StudioEventEmitter _lostEmitter;
        [SerializeField] private StudioEventEmitter _catchEmitter;
        [Header("Spin")]
        [SerializeField] private StudioEventEmitter _spinStartEmitter;
        [SerializeField] private StudioEventEmitter _spinEndEmitter;
        
        public void OnSpinStarted()
        {
            if (_spinStartEmitter) _spinStartEmitter.Play();
        }

        public void OnSpinEnded()
        {
            if (_spinEndEmitter) _spinEndEmitter.Play();
        }

        public void OnThrow()
        {
            if (_lostEmitter) _lostEmitter.Play();
        }

        public void OnCatch()
        {
            if (_catchEmitter) _catchEmitter.Play();
        }

        
    }
}