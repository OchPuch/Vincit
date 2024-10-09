using Guns.Projectiles;
using UnityEngine;

namespace Guns.View
{
    public class CloseRangeView : GeneralGunView
    {
        [Header("Hold view")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject holdViewRoot;
        
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Equip = Animator.StringToHash("Equip");

        private void Awake()
        {
            holdViewRoot.SetActive(false);
        }

        public override void OnGunEquip(Transform root)
        {
            holdViewRoot.SetActive(true);
            animator.SetTrigger(Equip);
        }

        public override void OnGunDeactivated()
        {
            holdViewRoot.SetActive(false);
        }

        public override void OnGunActivated()
        {
            holdViewRoot.SetActive(true);
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            animator.SetTrigger(Shoot);
        }
    }
}