using Guns.Data;
using Guns.General;
using UnityEngine;

namespace Guns.View
{
    public class GeneralGunView : GunView
    {
        [Header("Hold view")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject holdViewRoot;
        [Header("PropView")]
        [SerializeField] private GameObject propViewRoot;

        private Gun _gun;
        
        //Bools
        private static readonly int IsSpinning = Animator.StringToHash("IsSpinning");
        private static readonly int IsLost = Animator.StringToHash("Lost");
        //Triggers
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Equip = Animator.StringToHash("Equip");
        
        public override void Init(Gun gun, GunData data)
        {
            _gun = gun;
            
            gun.Shot += OnGunShot;
            gun.Activated += OnGunActivated;
            gun.Deactivated += OnGunDeactivated;
            gun.Equipped += OnGunEquip;

            if (gun is IThrowableGun throwableGun)
            {
                throwableGun.OnLost += OnGunLost;
                throwableGun.OnObtained += OnGunObtained;
            }
            
            if (gun is ISpinnableGun spinnableGun)
            {
                spinnableGun.SpinStarted += OnGunSpinStarted;
                spinnableGun.SpinEnded += OnGunSpinEnded;
            }
            
            
            holdViewRoot.SetActive(false);
            propViewRoot.SetActive(true);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _gun.Shot -= OnGunShot;
            _gun.Activated -= OnGunActivated;
            _gun.Deactivated -= OnGunDeactivated;
            _gun.Equipped -= OnGunEquip;

            if (_gun is IThrowableGun throwableGun)
            {
                throwableGun.OnLost -= OnGunLost;
                throwableGun.OnObtained -= OnGunObtained;
            }
            
            if (_gun is ISpinnableGun spinnableGun)
            {
                spinnableGun.SpinStarted -= OnGunSpinStarted;
                spinnableGun.SpinEnded -= OnGunSpinEnded;
            }
        }

        private void OnGunSpinEnded()
        {
            animator.SetBool(IsSpinning, false);
        }

        private void OnGunSpinStarted()
        {
            animator.SetBool(IsSpinning, true);
        }

        private void OnGunObtained()
        {
            animator.SetBool(IsLost, false);
        }

        private void OnGunLost()
        {
            animator.SetBool(IsLost, true);
        }

        private void OnGunEquip()
        {
            holdViewRoot.SetActive(true);
            propViewRoot.SetActive(false);
            animator.SetTrigger(Equip);
        }

        private void OnGunDeactivated()
        {
            holdViewRoot.SetActive(false);
        }

        private void OnGunActivated()
        {
            holdViewRoot.SetActive(true);
            animator.SetTrigger(Equip);
            if (_gun is ISpinnableGun spinnableGun)
            {
                animator.SetBool(IsSpinning, spinnableGun.IsSpinning);
            }
            if (_gun is IThrowableGun throwableGun)
            {
                animator.SetBool(IsLost, throwableGun.IsLost);
            }
        }

        private void OnGunShot()
        {
            animator.SetTrigger(Shoot);
        }
    }
}