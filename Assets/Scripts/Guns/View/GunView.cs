using Guns.Data;
using Guns.General;
using UnityEngine;

namespace Guns.View
{
    public class GunView : MonoBehaviour
    {
        [Header("Hold view")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject holdViewRoot;
        [Header("PropView")]
        [SerializeField] private GameObject propViewRoot;
         
        //Bools
        private static readonly int IsSpinning = Animator.StringToHash("IsSpinning");
        //Triggers
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        public void Init(Gun gun, GunData data)
        {
            gun.Shot += OnGunShot;
        }

        private void OnGunShot()
        {
            animator.SetTrigger(Shoot);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger(Shoot);
            }

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