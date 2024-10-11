using System;
using System.Linq;
using Guns.General;
using Guns.Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class Revolver : SpinThrowGun
    {
        [SerializeField] private float _spinReloadDebuff;
        private float _spinDeaccelerationDebuffByBullets;
        
        protected override ProjectileConfig OnShot()
        {
            try
            {
                var loadedHolders = Data.CapsuleHolders.Where(x => x.IsLoaded).ToList();
                _spinDeaccelerationDebuffByBullets = Mathf.Clamp01((float) loadedHolders.Count / Data.Config.MagSize);
                if (loadedHolders.Count <= 0)
                {
                    Reload();
                    return null;   
                }
                CapsuleHolder capsuleHolder = IsSpinning ? loadedHolders[Random.Range(0, loadedHolders.Count)] : loadedHolders[0];
                var bullet = capsuleHolder.Shoot(transform.position, transform.forward);
                if (bullet is null)
                {
                    Reload();
                    return null;
                }
                bullet.Init(this);
                return bullet.Config;

            }
            catch (Exception)
            {
                Reload();
                return null;
            }
        }

        protected override void ProcessSpinRequest(bool spinRequest)
        {
            if (spinRequest)
            {
                Data.CurrentSpinSpeed += Data.Config.SpinAcceleration * Time.deltaTime;
                if (Data.CurrentSpinSpeed >= Data.Config.SpinMaxSpeed) Data.CurrentSpinSpeed = Data.Config.SpinMaxSpeed;
            }
            else
            {
                Data.CurrentSpinSpeed -= Data.Config.SpinDeacceleration * (_spinDeaccelerationDebuffByBullets)
                                         * Time.deltaTime;
                if (Data.CurrentSpinSpeed <= 0) Data.CurrentSpinSpeed = 0;
            }
        }

        protected override void OnReload()
        {
            _spinDeaccelerationDebuffByBullets = 1f;
            if (IsSpinning) Data.FireTimer -= _spinReloadDebuff;
            base.OnReload();
        }
    }
    
}