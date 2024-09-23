using System;

namespace Guns.Projectiles.Interactions
{
    public interface IOverloadable
    {
        public bool IsOverloaded { get; }
        public event Action Overloaded;
        public void Overload();
    }
}