using System;
using Guns.Data;
using Guns.General;
using Guns.View;
using UniRx;
using UnityEngine;

namespace Guns.Types.Revolver
{
    public class RevolverView : GunView
    {
        [Header("Hold view")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject holdViewRoot;
        [Header("PropView")]
        [SerializeField] private GameObject propViewRoot;
        
        //Bools
        private static readonly int IsSpinning = Animator.StringToHash("IsSpinning");
        private static readonly int IsLost = Animator.StringToHash("Lost");
        //Triggers
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Equip = Animator.StringToHash("Equip");
        
        public override void Init(Gun gun, GunData data)
        {
            gun.Shot += OnGunShot;
            gun.Activated += OnGunActivated;
            gun.Deactivated += OnGunDeactivated;
            gun.Equipped += OnGunEquip;
            
            holdViewRoot.SetActive(false);
            propViewRoot.SetActive(true);
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
        }

        private void OnGunShot()
        {
            animator.SetTrigger(Shoot);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetBool(IsSpinning, true);
            }

            if (Input.GetMouseButtonUp(1))
            {
                animator.SetBool(IsSpinning, false);
            }
        }

      
    }
}