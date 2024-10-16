﻿using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Guns.Projectiles
{
    public class ProjectileFactory : PlaceholderFactory<Projectile>
    {
        private readonly Projectile _projectilePrefab;
        private readonly ObjectPool<Projectile> _projectilePool;
        private DiContainer _diContainer;

        public ProjectileConfig ProjectileConfig => _projectilePrefab.Config;
        
        public ProjectileFactory(Projectile projectile)
        {
            _projectilePrefab = projectile;
            _projectilePool = new ObjectPool<Projectile>(CreateNewProjectile, OnProjectileTake, OnReturn, OnDestroyBullet, true, 20, 1000);
        }

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void OnDestroyBullet(Projectile obj)
        {
            Object.Destroy(obj.gameObject);
        }

        private void OnReturn(Projectile obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnProjectileTake(Projectile obj)
        {
            obj.gameObject.SetActive(true);
            obj.ResetBullet();
        }

        private Projectile CreateNewProjectile()
        {
            var bullet = _diContainer.InstantiatePrefabForComponent<Projectile>(_projectilePrefab);
            bullet.gameObject.SetActive(false);
            bullet.BulletDestroyed += () =>
            {
                _projectilePool.Release(bullet);
            };
            return bullet;
        }
        
        
        public Projectile CreateProjectile(Vector3 position, Vector3 lookVector)
        {
            var bullet = _projectilePool.Get();
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.position = position;
            bullet.transform.forward = lookVector;
            return bullet;
        }
        
        public Projectile CreateProjectile(Transform copyFrom)
        {
            var bullet = _projectilePool.Get();
            bullet.transform.rotation = copyFrom.rotation;
            bullet.transform.position = copyFrom.position;
            bullet.transform.forward = copyFrom.forward;
            bullet.transform.up = copyFrom.up;
            bullet.transform.right = copyFrom.right;
            return bullet;
        }
    }
}