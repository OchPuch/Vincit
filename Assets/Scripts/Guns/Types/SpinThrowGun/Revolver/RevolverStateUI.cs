using Guns.Interfaces.Spin;
using Guns.Projectiles;
using Guns.View;
using PrimeTween;
using UnityEngine;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class RevolverStateUI :  GunStateUI, ISpinObserver, ISpinReportListener
    {
        [SerializeField] private RectTransform _baraban;
        [SerializeField] private Ease _barabanStopSpinEase;

        private SpinReport _lastSpinReport;

        private Quaternion _barabanOffsetRotation;
        private Quaternion _barabanStartRotation;
        private Tween _spinStopTween;
        
        private Quaternion TargetRotation => _barabanStartRotation * _barabanOffsetRotation;

        protected override void Start()
        {
            base.Start();
            _lastSpinReport = new SpinReport()
            {
                IsSpinning = false,
                SpinSpeed =  0
            };
            _barabanStartRotation = _baraban.rotation;
        }
        
        private void Update()
        {
            _baraban.Rotate(0,0, _lastSpinReport.SpinSpeed * Time.deltaTime);
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            _spinStopTween.Complete();
           // RotateToTarget();
        }
        
        private void RotateToTarget()
        {
            //_spinStopTween.Stop();
            //spinStopTween = Tween.RotationAtSpeed(_baraban, TargetRotation, _lastSpinReport.SpinSpeed, _barabanStopSpinEase);
        }
        
        public void OnSpinStarted()
        {
            //_spinStopTween.Stop();
            
        }

        public void OnSpinEnded()
        {
            //_spinStopTween.Stop();
            //RotateToTarget();
        }

        public void UpdateSpinState(SpinReport spinReport)
        {
            _lastSpinReport = spinReport;
        }
    }
}