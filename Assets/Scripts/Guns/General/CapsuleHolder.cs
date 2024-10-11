using System;
using Guns.Projectiles;
using TMPro;
using UniRx;
using UnityEngine;

namespace Guns.General
{
    public class CapsuleHolder
    {
        private ProjectileFactory _projectileFactory; 
        public bool IsLoaded { get; private set; }
        
        public ProjectileConfig ProjectileConfig { get; private set; }

        private BoolReactiveProperty _isLoaded;
        public event Action<ProjectileConfig> Reloaded;

        public event Action Shot;

        public void Reload(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
            ProjectileConfig = projectileFactory.ProjectileConfig;
            IsLoaded = true;
            Reloaded?.Invoke(projectileFactory.ProjectileConfig);
        }

        public void ReloadSame()
        {
            Reload(_projectileFactory);
        }

        public Projectile Shoot(Vector3 position, Vector3 forward)
        {
            if (!IsLoaded) return null;
            IsLoaded = false;
            var projectile = _projectileFactory.CreateProjectile(position, forward);
            Shot?.Invoke();
            return projectile;
        }

        public void Shoot(Transform copyFrom)
        {
            Shoot(copyFrom.position, copyFrom.forward);
        }
    }
}