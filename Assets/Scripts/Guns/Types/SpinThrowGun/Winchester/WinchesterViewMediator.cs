using UnityEngine;
using Zenject;

namespace Guns.Types.SpinThrowGun.Winchester
{
    public class WinchesterViewMediator : SpinThrowGunViewMediator
    {
        [SerializeField] private WinchesterStateUI _winchesterStateUI;

        private Winchester _winchester;
        
        [Inject]
        private void Construct(Winchester winchester)
        {
            _winchester = winchester;
        }
        
        private void Start()
        {
            _winchesterStateUI.InitCapsuleHolderViews(_winchester.Data.CapsuleHolders);
        }

        protected override void OnGunEquip(Player.Player obj)
        {
            base.OnGunEquip(obj);
            _winchesterStateUI.OnGunEquip(obj.Data.gunStateUiRoot);
        }

        protected override void OnGunActivated()
        {
            base.OnGunActivated();
            _winchesterStateUI.OnGunActivated();
        }

        protected override void OnGunDeactivated()
        {
            base.OnGunDeactivated();
            _winchesterStateUI.OnGunDeactivated();
        }
    }
}