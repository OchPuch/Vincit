using Guns.Projectiles;
using Guns.View;
using UnityEngine;

namespace Guns.Types.CloseRange
{
    public sealed class CloseRangeViewMediator : GunViewMediator
    {
        [SerializeField] private CloseRangeView _gunAnimationView;
        [SerializeField] private GunAudio _gunAudio;
        
        protected override void OnGunEquip(Player.Player obj)
        {
            _gunAnimationView.OnGunEquip(obj.transform);
            _gunAudio.OnGunEquip(obj.transform);
        }

        protected override void OnGunShot(ProjectileConfig obj)
        {
            _gunAnimationView.OnGunShot(obj);
            _gunAudio.OnGunShot(obj);
        }

        protected override void OnGunActivated()
        {
            base.OnGunActivated();
            _gunAnimationView.OnGunActivated();
            _gunAudio.OnGunActivated();
        }

        protected override void OnGunDeactivated()
        {
            base.OnGunDeactivated();
            _gunAnimationView.OnGunDeactivated();
            _gunAudio.OnGunDeactivated();
        }
    }
}