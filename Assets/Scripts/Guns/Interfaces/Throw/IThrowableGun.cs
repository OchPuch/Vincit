using System;

namespace Guns.Interfaces.Throw
{
    public interface IThrowableGun
    {
        public bool IsLost { get; }
        public event Action OnLost;
        public event Action OnObtained;

        public void Throw();
        public void Catch();
    }
}