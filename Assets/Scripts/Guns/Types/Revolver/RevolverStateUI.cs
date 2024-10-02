using Guns.View;
using PrimeTween;
using UnityEngine;

namespace Guns.Types.Revolver
{
    public class RevolverStateUI : GunStateUI
    {
        [SerializeField] private Transform baraban;
        [SerializeField] private Ease barabanStopSpinEase;

        private Quaternion _barabanStartRotation;
        private Tween _spinStopTween;

        protected override void Start()
        {
            base.Start();
            _barabanStartRotation = baraban.rotation;
        }

        protected override void SpinnableUpdate()
        {
            if (!SpinnableGun.IsSpinning) return;
            baraban.Rotate(0,0, Data.currentSpinSpeed * Time.deltaTime);
        }

        protected override void OnGunSpinStarted()
        {
            _spinStopTween.Stop();
        }

        protected override void OnGunSpinEnded()
        {
            _spinStopTween = Tween.LocalRotation(baraban, _barabanStartRotation, Data.Config.SpinStopTime, barabanStopSpinEase);
        }
    }
}