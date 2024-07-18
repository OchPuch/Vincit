using Guns.General;
using UnityEngine;
using UnityEngine.Pool;

namespace Guns.Bullets
{
    public class BulletFactory
    {
        private readonly Bullet _bulletPrefab;
        private readonly Gun _origin;
        
        private readonly ObjectPool<Bullet> _bulletPool;

        public BulletFactory(Bullet bullet, Gun origin)
        {
            _bulletPrefab = bullet;
            _origin = origin;
            _bulletPool = new ObjectPool<Bullet>(CreateNewBullet, OnTake, OnReturn, OnDestroyBullet, true, 20, 1000);
        }

        private void OnDestroyBullet(Bullet obj)
        {
            Object.Destroy(obj.gameObject);
        }

        private void OnReturn(Bullet obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnTake(Bullet obj)
        {
            obj.gameObject.SetActive(true);
            obj.ResetBullet();
        }

        private Bullet CreateNewBullet()
        {
            var bullet = Object.Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            bullet.BulletDestroyed += () =>
            {
                _bulletPool.Release(bullet);
            };
            return bullet;
        }

        public Bullet CreateBullet(Vector2 position)
        {
            var bullet = _bulletPool.Get();
            bullet.transform.position = position;
            return bullet;
        }
    }
}