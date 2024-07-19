using Guns.Data;
using Guns.General;
using Guns.View;
using UnityEngine;

namespace Guns.Types.Hand
{
    public class HandView : GunView
    {
        [Header("Hold view")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject holdViewRoot;
        
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Equip = Animator.StringToHash("Equip");
        
        public override void Init(Gun gun, GunData data)
        {
            gun.Shot += OnGunShot;
            gun.Activated += OnGunActivated;
            gun.Deactivated += OnGunDeactivated;
            gun.Equipped += OnGunEquip;
            holdViewRoot.SetActive(false);
        }

        private void OnGunEquip()
        {
            holdViewRoot.SetActive(true);
            animator.SetTrigger(Equip);
        }

        private void OnGunDeactivated()
        {
            holdViewRoot.SetActive(false);
        }

        private void OnGunActivated()
        {
            holdViewRoot.SetActive(true);
        }

        private void OnGunShot()
        {
            animator.SetTrigger(Shoot);
        }
    }
}