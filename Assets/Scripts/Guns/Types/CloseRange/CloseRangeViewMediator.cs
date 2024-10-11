using Guns.Projectiles;
using Guns.View;
using UnityEngine;
using Zenject;

namespace Guns.Types.CloseRange
{
    public sealed class CloseRangeViewMediator : GunViewMediator
    {
        [SerializeField] private CloseRangeView _gunAnimationView;
        [SerializeField] private GunAudio _gunAudio;

        private CloseRange _closeRange;
        
        [Inject]
        private void Construct(CloseRange closeRange)
        {
            _closeRange = closeRange;
            _closeRange.PunchApproved += OnPunchApproved;
        }

        private void OnDestroy()
        {
            _closeRange.PunchApproved -= OnPunchApproved;
        }
        
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
        
        private void OnPunchApproved(float f)
        {
            _gunAnimationView.OnPunchApproved(f);
        }
    }
}