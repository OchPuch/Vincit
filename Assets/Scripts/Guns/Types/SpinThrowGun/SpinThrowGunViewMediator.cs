﻿using Guns.Projectiles;
using Guns.Types.Revolver;
using Guns.View;
using UnityEngine;
using Zenject;

namespace Guns.Types.SpinThrowGun
{
    public class SpinThrowGunViewMediator : GunViewMediator
    {
        [SerializeField] private SpinThrowAnimationView _spinThrowAnimationView;
        [SerializeField] private SpinThrowGunAudio _spinThrowGunAudio;

        private SpinThrowGun _spinThrowGun;
        
        [Inject]
        private void Construct(SpinThrowGun spinThrow)
        {
            _spinThrowGun = spinThrow;
            _spinThrowGun.SpinStarted += OnSpinStarted;
            _spinThrowGun.SpinEnded += OnSpinEnded;
            _spinThrowGun.OnLost += OnLost;
            _spinThrowGun.OnObtained += OnObtained;
        }

        private void OnDestroy()
        {
            _spinThrowGun.SpinStarted -= OnSpinStarted;
            _spinThrowGun.SpinEnded -= OnSpinEnded;
            _spinThrowGun.OnLost -= OnLost;
            _spinThrowGun.OnObtained -= OnObtained;
        }

        protected virtual void OnObtained()
        {
            _spinThrowAnimationView.OnCatch();
            _spinThrowGunAudio.OnCatch();
        }

        protected virtual void OnLost()
        {
            _spinThrowAnimationView.OnThrow();
            _spinThrowGunAudio.OnThrow();
        }

        protected virtual void OnSpinEnded()
        {
            _spinThrowAnimationView.OnSpinEnded();
            _spinThrowGunAudio.OnSpinEnded();
        }

        protected virtual void OnSpinStarted()
        {
            _spinThrowAnimationView.OnSpinStarted();
            _spinThrowGunAudio.OnSpinStarted();
        }

        protected override void OnGunEquip(Player.Player obj)
        {
            _spinThrowGunAudio.OnGunEquip(obj.transform);
            _spinThrowAnimationView.OnGunEquip(obj.transform);
        }

        protected override void OnGunDeactivated()
        {
            _spinThrowAnimationView.OnGunDeactivated();
            _spinThrowGunAudio.OnGunDeactivated();
        }

        protected override void OnGunActivated()
        {
            _spinThrowAnimationView.OnGunActivated();
            _spinThrowGunAudio.OnGunActivated();
            
            _spinThrowAnimationView.UpdateSpinState(new SpinReport()
            {
                IsSpinning = _spinThrowGun.IsSpinning,
                SpinSpeed = _spinThrowGun.Data.currentSpinSpeed
            });
        }

        protected override void OnGunShot(ProjectileConfig obj)
        {
            _spinThrowAnimationView.OnGunDeactivated();
            _spinThrowGunAudio.OnGunDeactivated();
        }
    }
}