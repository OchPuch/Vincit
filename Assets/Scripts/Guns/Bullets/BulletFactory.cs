using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Guns.Bullets
{
    public class BulletFactory : PlaceholderFactory<Bullet>
    {
        private readonly Bullet _bulletPrefab;

        private DiContainer _diContainer;
        private readonly ObjectPool<Bullet> _bulletPool;

        public BulletFactory(Bullet bullet)
        {
            _bulletPrefab = bullet;
            _bulletPool = new ObjectPool<Bullet>(CreateNewBullet, OnBulletake, OnReturn, OnDestroyBullet, true, 20, 1000);
        }

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void OnDestroyBullet(Bullet obj)
        {
            Object.Destroy(obj.gameObject);
        }

        private void OnReturn(Bullet obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnBulletake(Bullet obj)
        {
            obj.gameObject.SetActive(true);
            obj.ResetBullet();
        }

        private Bullet CreateNewBullet()
        {
            var bullet = _diContainer.InstantiatePrefabForComponent<Bullet>(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            bullet.BulletDestroyed += () =>
            {
                _bulletPool.Release(bullet);
            };
            return bullet;
        }
        
        
        public Bullet CreateBullet(Vector3 position, Vector3 lookVector)
        {
            var bullet = _bulletPool.Get();
            bullet.transform.position = position;
            bullet.transform.forward = lookVector;
            return bullet;
        }
    }
}