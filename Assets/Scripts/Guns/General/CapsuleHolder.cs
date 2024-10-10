using Guns.Projectiles;
using UniRx;
using UnityEngine;

namespace Guns.General
{
    public class CapsuleHolder
    {
        private ProjectileFactory _projectileFactory; 
        public bool IsLoaded { get; private set; }

        private BoolReactiveProperty _isLoaded;

        public void Reload(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
            IsLoaded = true;
        }

        public void ReloadSame()
        {
            IsLoaded = true;
        }

        public Projectile Shoot(Vector3 position, Vector3 forward)
        {
            if (!IsLoaded) return null;
            IsLoaded = false;
            return _projectileFactory.CreateProjectile(position, forward);
        }

        public void Shoot(Transform copyFrom)
        {
            if (!IsLoaded) return;
            _projectileFactory.CreateProjectile(copyFrom);
            IsLoaded = false;
        }
    }
}