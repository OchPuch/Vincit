using Guns.Interfaces.Spin;
using Guns.Interfaces.Throw;
using Guns.Types.SpinThrowGun;
using Guns.View;
using UnityEngine;

namespace Guns.Types.Revolver
{
    public class SpinThrowAnimationView : GunAnimationView, ISpinObserver, IThrowObserver, ISpinReportListener
    {
        private static readonly int IsSpinning = Animator.StringToHash("IsSpinning");
        private static readonly int IsLost = Animator.StringToHash("Lost");

        public void OnSpinStarted()
        {
            Animator.SetBool(IsSpinning, true);
        }

        public void OnSpinEnded()
        {
            Animator.SetBool(IsSpinning, false);
        }

        public void OnThrow()
        {
            Animator.SetBool(IsLost, true);
        }

        public void OnCatch()
        {
            Animator.SetBool(IsLost, false);
        }

        public void UpdateSpinState(SpinReport spinReport)
        {
            Animator.SetBool(IsSpinning, spinReport.IsSpinning);
        }
    }
}