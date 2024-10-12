using Guns.Projectiles;
using UnityEngine;

namespace Guns.View
{
    public class GunAnimationView : GeneralGunView
    {
        [Header("Hold view")]
        [SerializeField] protected Animator Animator;
        [SerializeField] private GameObject holdViewRoot;
        [Header("PropView")]
        [SerializeField] private GameObject propViewRoot;

        private static readonly int Reload = Animator.StringToHash("IsReloading");
        //Triggers
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int Equip = Animator.StringToHash("Equip");
        private static readonly int ShellLoaded = Animator.StringToHash("ShellLoaded");
        
        private bool _isEquipped;
        
        private void Awake()
        {
            if (_isEquipped) return;
            holdViewRoot.SetActive(false);
            if (propViewRoot) propViewRoot.SetActive(true);
        }
        

        public override void OnGunEquip(Transform root)
        {
            _isEquipped = true;
            holdViewRoot.SetActive(true);
            if (propViewRoot) propViewRoot.SetActive(false);
            Animator.SetTrigger(Equip);
        }

        public override void OnGunReloaded()
        {
            Animator.SetBool(Reload, true);
        }

        public void StopReload()
        {
            Animator.SetBool(Reload, false);
        }
        
        public override void OnGunDeactivated()
        {
            holdViewRoot.SetActive(false);
        }

        public override void OnGunActivated()
        {
            holdViewRoot.SetActive(true);
            Animator.SetTrigger(Equip);
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            Animator.SetTrigger(Shoot);
        }
        
        public virtual void OnShellLoaded()
        {
            Animator.SetTrigger(ShellLoaded);
        }
    }
}