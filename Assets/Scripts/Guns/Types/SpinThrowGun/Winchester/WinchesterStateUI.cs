using System.Collections.Generic;
using Guns.General;
using Guns.Projectiles;
using Guns.View;
using UnityEngine;

namespace Guns.Types.SpinThrowGun.Winchester
{
    public class WinchesterStateUI : GunStateUI
    {
        [SerializeField] private List<CapsuleHolderView> _capsuleHolderViews;
        
        public void InitCapsuleHolderViews(List<CapsuleHolder> capsuleHolders)
        {
            for (int i = 0; i < capsuleHolders.Count; i++)
            {
                _capsuleHolderViews[i].Init(capsuleHolders[i]);
            }
        }
        
        public override void OnGunReloaded()
        {
            
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            
        }
    }
}