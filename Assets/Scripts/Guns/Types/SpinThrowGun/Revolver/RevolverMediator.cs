using UnityEngine;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public sealed class RevolverMediator : SpinThrowGunViewMediator
    {
        [SerializeField] private RevolverStateUI _revolverStateUI;

        protected override void OnGunEquip(Player.Player obj)
        {
            base.OnGunEquip(obj);
            _revolverStateUI.OnGunEquip(obj.Data.gunStateUiRoot);
        }

        protected override void OnGunActivated()
        {
            base.OnGunActivated();
            _revolverStateUI.OnGunActivated();
        }

        protected override void OnGunDeactivated()
        {
            base.OnGunDeactivated();
            _revolverStateUI.OnGunDeactivated();    
        }

        protected override void OnSpinStarted()
        {
            base.OnSpinStarted();
            _revolverStateUI.OnSpinStarted();
        }

        protected override void OnSpinEnded()
        {
            base.OnSpinEnded();
            _revolverStateUI.OnSpinEnded();
        }
        
        
    }
}