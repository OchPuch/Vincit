using System;
using System.Collections.Generic;
using System.Linq;
using Guns.Data;
using Guns.General;
using Guns.Interfaces.Spin;
using Guns.Projectiles;
using Guns.View;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class RevolverStateUI :  GunStateUI, ISpinObserver, ISpinReportListener
    {
        [SerializeField] private List<CapsuleHolderView> _capsuleHolderViews;
        
        [SerializeField] private float _barabanRotateDuration = 0.5f;
        [SerializeField] private RectTransform _baraban;
        [SerializeField] private Ease _barabanStopSpinEase;

        private SpinReport _lastSpinReport;
        private Quaternion _targetRotation;
        private Tween _spinStopTween;

        [Inject]
        private void Construct(GunConfig config)
        {
            _barabanRotateDuration = config.FireRate;
        }
        
        
        public void InitCapsuleHolderViews(IEnumerable<CapsuleHolder> capsuleHolders)
        {
            var enumerable = capsuleHolders.ToList();
            for (int i = 0; i < enumerable.Count; i++)
            {
                _capsuleHolderViews[i].Init(enumerable[i]);
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
            // if (_lastSpinReport.IsSpinning) return;
            // if (_spinStopTween.isAlive) _spinStopTween.Complete();
            // RotateToBase();
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            // if (_lastSpinReport.IsSpinning) return;
            // if (_spinStopTween.isAlive) _spinStopTween.Complete();
            // RotateToTarget();
        }
        
        private void RotateToTarget()
        { 
            // if (_spinStopTween.isAlive) _spinStopTween.Stop();
            // var capsuleHolder = _capsuleHolderViews.FirstOrDefault(x => x.IsLoaded);
            // RotateToCapsuleHolder(capsuleHolder);
        }

        private void RotateToBase()
        {
            // if (_spinStopTween.isAlive) _spinStopTween.Stop();
            // var capsuleHolder = _capsuleHolderViews[0];
            // RotateToCapsuleHolder(capsuleHolder);
        }

        private void RotateToCapsuleHolder(CapsuleHolderView capsuleHolderView)
        {
            // if (capsuleHolderView is not null)
            // {
            //     float angle = -Vector3.SignedAngle(capsuleHolderView.transform.up, Vector3.up,
            //         _baraban.transform.forward);
            //     Debug.Log(angle);
            //     Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            //     _spinStopTween = Tween.LocalRotation(_baraban, targetRotation, _barabanRotateDuration , _barabanStopSpinEase);
            // }
            // else
            // {
            //     if (_capsuleHolderViews.Count > 0) 
            //         RotateToBase();
            // }
        }
        
       
        
        public void OnSpinStarted()
        {
            // if (_spinStopTween.isAlive) _spinStopTween.Stop();
        }

        public void OnSpinEnded()
        {
            // if (_spinStopTween.isAlive) _spinStopTween.Stop();
            // RotateToTarget();
        }
        public void UpdateSpinState(SpinReport spinReport)
        {
            _lastSpinReport = spinReport;
        }
    }
}