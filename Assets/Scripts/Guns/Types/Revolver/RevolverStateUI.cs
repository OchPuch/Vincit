using Guns.View;
using PrimeTween;
using UnityEngine;

namespace Guns.Types.Revolver
{
    public class RevolverStateUI : GunStateUI
    {
        [SerializeField] private Transform baraban;
        [SerializeField] private Ease barabanStopSpinEase;

        private Revolver _revolver;
        
        private Quaternion _barabanStartRotation;
        private Tween _spinStopTween;

        protected override void Start()
        {
            base.Start();
            _barabanStartRotation = baraban.rotation;
            if (Gun is Revolver revolver)
            {
                _revolver = revolver;
            }
            else
            {
                Debug.LogError("Gun is not revolver");
            }
        }

        private void Update()
        {
            if (!_revolver.IsSpinning) return;
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