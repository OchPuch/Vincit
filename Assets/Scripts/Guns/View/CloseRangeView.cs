using Guns.Data;
using Guns.General;
using Guns.Projectiles;
using UnityEngine;
using Zenject;

namespace Guns.View
{
    public class CloseRangeView : GunView
    {
        [Header("Hold view")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject holdViewRoot;
        
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Equip = Animator.StringToHash("Equip");
        
        [Inject]
        private void Construct(Gun gun, GunData data)
        {
            gun.Shot += OnGunShot;
            gun.Activated += OnGunActivated;
            gun.Deactivated += OnGunDeactivated;
            gun.Equipped += OnGunEquip;
            holdViewRoot.SetActive(false);
        }

        private void OnGunEquip(Player.Player player)
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

        private void OnGunShot(ProjectileConfig projectileConfig)
        {
            animator.SetTrigger(Shoot);
        }
    }
}