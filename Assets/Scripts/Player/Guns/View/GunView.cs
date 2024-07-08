using Player.Guns.Data;
using Player.Guns.General;
using UnityEngine;

namespace Player.Guns.View
{
    public class GunView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
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
