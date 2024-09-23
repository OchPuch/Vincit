using System;

namespace Guns.General
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