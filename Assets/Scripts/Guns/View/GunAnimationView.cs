using Guns.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Guns.View
{
    public class GunAnimationView : GeneralGunView
    {
        [Header("Hold view")]
        [SerializeField] protected Animator Animator;
        [SerializeField] private GameObject holdViewRoot;
        [Header("PropView")]
        [SerializeField] private GameObject propViewRoot;
        
        
        //Triggers
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Equip = Animator.StringToHash("Equip");

        [Inject]
        private void Awake()
        {
            holdViewRoot.SetActive(false);
            propViewRoot.SetActive(true);
        }
        
        public override void OnGunEquip(Transform root)
        {
            holdViewRoot.SetActive(true);
            propViewRoot.SetActive(false);
            Animator.SetTrigger(Equip);
        }

        public override void OnGunDeactivated()
        {
            holdViewRoot.SetActive(false);
        }

        public override void OnGunActivated()
        {
            holdViewRoot.SetActive(true);
            Animator.SetTrigger(Equip);
            // if (Gun is ISpinnableGun spinnableGun)
            // {
            //     animator.SetBool(IsSpinning, spinnableGun.IsSpinning);
            // }
            // if (Gun is IThrowableGun throwableGun)
            // {
            //     animator.SetBool(IsLost, throwableGun.IsLost);
            // }
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            Animator.SetTrigger(Shoot);
        }
    }
}