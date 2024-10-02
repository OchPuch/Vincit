using System;
using General;
using Guns.Data;
using Guns.General;
using UnityEngine;

namespace Guns.View
{
    public class GunStateUI : GamePlayBehaviour
    {
        [SerializeField] private RectTransform rootPanel;
        

        protected GunData Data;
        protected Gun Gun;
        protected IThrowableGun ThrowableGun; //Will be null if gun does not inherit IThrowableGun
        protected ISpinnableGun SpinnableGun; //Will be null if gun does not inherit ISpinnableGun

        private Action _updated;


        public void Init(Gun gun, GunData data)
        {
            Gun = gun;
            Data = data;

            rootPanel.gameObject.SetActive(false);

            gun.Shot += OnGunShot;
            gun.Activated += OnGunActivated;
            gun.Deactivated += OnGunDeactivated;
            gun.Equipped += OnGunEquip;

            if (gun is IThrowableGun throwableGun)
            {
                ThrowableGun = throwableGun;
                throwableGun.OnLost += OnGunLost;
                throwableGun.OnObtained += OnGunObtained;
                _updated += ThrowableUpdate;
            }

            if (gun is ISpinnableGun spinnableGun)
            {
                SpinnableGun = spinnableGun;
                spinnableGun.SpinStarted += OnGunSpinStarted;
                spinnableGun.SpinEnded += OnGunSpinEnded;
                _updated += SpinnableUpdate;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Gun.Shot -= OnGunShot;
            Gun.Activated -= OnGunActivated;
            Gun.Deactivated -= OnGunDeactivated;
            Gun.Equipped -= OnGunEquip;

            if (Gun is IThrowableGun throwableGun)
            {
                throwableGun.OnLost -= OnGunLost;
                throwableGun.OnObtained -= OnGunObtained;
            }

            if (Gun is ISpinnableGun spinnableGun)
            {
                spinnableGun.SpinStarted -= OnGunSpinStarted;
                spinnableGun.SpinEnded -= OnGunSpinEnded;
            }
        }

        /// <summary>
        /// Used if gun is spinnable (ISpinnableGun)
        /// </summary>
        protected virtual void SpinnableUpdate()
        {
        }

        /// <summary>
        /// Used if gun is throwable (IThrowableGun)
        /// </summary>
        protected virtual void ThrowableUpdate()
        {
        }


        private void Update()
        {
            _updated.Invoke();
        }

        protected virtual void OnGunSpinEnded()
        {
        }

        protected virtual void OnGunSpinStarted()
        {
        }

        protected virtual void OnGunObtained()
        {
        }

        protected virtual void OnGunLost()
        {
        }

        protected virtual void OnGunEquip(Player.Player player)
        {
            rootPanel.gameObject.SetActive(true);
            rootPanel.SetParent(player.Data.gunStateUiRoot);
            rootPanel.localRotation = Quaternion.identity;
            rootPanel.localPosition = Vector3.zero;
            rootPanel.localScale = Vector3.one;
            var max = rootPanel.offsetMax;
            max.x = 0;
            max.y = 0;
            rootPanel.offsetMax = max;
            var min = rootPanel.offsetMin;
            min.x = 0;
            min.y = 0;
            rootPanel.offsetMin = min;
        }

        protected virtual void OnGunDeactivated()
        {
            enabled = false;
            rootPanel.gameObject.SetActive(false);
        }

        protected virtual void OnGunActivated()
        {
            enabled = true;
            rootPanel.gameObject.SetActive(true);
        }

        protected virtual void OnGunShot()
        {
        }
    }
}