using System.Collections.Generic;
using Guns.General;
using Guns.Interfaces.Spin;
using Guns.Projectiles;
using Guns.View;
using PrimeTween;
using UnityEngine;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class RevolverStateUI :  GunStateUI, ISpinObserver, ISpinReportListener
    {
        [SerializeField] private List<CapsuleHolderView> _capsuleHolderViews;
        
        [SerializeField] private RectTransform _baraban;
        [SerializeField] private Ease _barabanStopSpinEase;

        private SpinReport _lastSpinReport;
        private Tween _spinStopTween;
        
        public void InitCapsuleHolderViews(List<CapsuleHolder> capsuleHolders)
        {
            for (int i = 0; i < capsuleHolders.Count; i++)
            {
                _capsuleHolderViews[i].Init(capsuleHolders[i]);
            }
        }
        
        protected override void Start()
        {
            base.Start();
            _lastSpinReport = new SpinReport()
            {
                IsSpinning = false,
                SpinSpeed =  0
            };
        }
        
        private void Update()
        {
            _baraban.Rotate(0,0, _lastSpinReport.SpinSpeed * Time.deltaTime);
        }

        public override void OnGunReloaded()
        {
            
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