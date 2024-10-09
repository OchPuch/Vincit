using System;

namespace Guns.Types.SpinThrowGun
{
    public interface ISpinnableGun
    {
        public bool IsSpinning { get; }

        public event Action SpinStarted;
        public event Action SpinEnded;
        public void StartSpin();

        public void EndSpin();
    }
}