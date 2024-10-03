using Guns.Projectiles;
using UnityEngine;

namespace Guns.Types.Revolver
{
    public class CapsuleHolder
    {
        private ProjectileFactory _projectileFactory; 
            
        public bool IsLoaded { get; private set; }

        public void Init(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        public void Shoot(Vector3 position, Vector3 forward)
        {
            if (!IsLoaded) return;
            _projectileFactory.CreateProjectile(position, forward);
        }

        public void Shoot(Transform copyFrom)
        {
            if (!IsLoaded) return;
            _projectileFactory.CreateProjectile(copyFrom);
        }
    }
}