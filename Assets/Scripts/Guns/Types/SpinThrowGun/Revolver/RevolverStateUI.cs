using Guns.Data;
using Guns.Projectiles;
using Guns.View;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class RevolverStateUI :  GunStateUI, ISpinObserver, ISpinReportListener
    {
        [SerializeField] private Transform _baraban;
        [SerializeField] private Ease _barabanStopSpinEase;

        private GunConfig _config;
        private SpinReport _lastSpinReport;

        private Quaternion _barabanOffsetRotation;
        private Quaternion _barabanStartRotation;
        private Tween _spinStopTween;
        
        private Quaternion TargetRotation => _barabanStartRotation * _barabanOffsetRotation;

        
        [Inject]
        private void Construct(GunConfig config)
        {
            _config = config;
        }

        protected override void Start()
        {
            base.Start();
            _barabanStartRotation = _baraban.rotation;
        }
        
        private void Update()
        {
            if (_lastSpinReport.IsSpinning) 
                _baraban.Rotate(0,0, _lastSpinReport.SpinSpeed * Time.deltaTime);
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            _spinStopTween.Complete();
            RotateToTarget();
        }
        
        private void RotateToTarget()
        {
            _spinStopTween.Stop();
            _spinStopTween = Tween.LocalRotation(_baraban, TargetRotation, _config.SpinStopTime, _barabanStopSpinEase);
        }
        
        public void OnSpinStarted()
        {
            _spinStopTween.Stop();
            
        }

        public void OnSpinEnded()
        {
            _spinStopTween.Stop();
            RotateToTarget();
        }

        public void UpdateSpinState(SpinReport spinReport)
        {
            _lastSpinReport = spinReport;
        }
    }
}