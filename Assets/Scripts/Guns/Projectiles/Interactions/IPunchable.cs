using UnityEngine;

namespace Guns.Projectiles.Interactions
{
    public interface IPunchable
    {
        public void Punch(Vector3 force);
    }
}