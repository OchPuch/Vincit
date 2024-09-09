using Guns.General;
using UnityEngine;
using UnityEngine.Pool;

namespace Guns.Bullets
{
    public class BulletFactory<T> where T: Bullet
    {
        private readonly T _bulletPrefab;
        private readonly Gun _origin;
        
        private readonly ObjectPool<T> _bulletPool;

        public BulletFactory(T bullet, Gun origin)
        {
            _bulletPrefab = bullet;
            _origin = origin;
            _bulletPool = new ObjectPool<T>(CreateNewBullet, OnTake, OnReturn, OnDestroyBullet, true, 20, 1000);
        }

        private void OnDestroyBullet(T obj)
        {
            Object.Destroy(obj.gameObject);
        }

        private void OnReturn(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnTake(T obj)
        {
            obj.gameObject.SetActive(true);
            obj.ResetBullet();
        }

        private T CreateNewBullet()
        {
            var bullet = Object.Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            bullet.BulletDestroyed += () =>
            {
                _bulletPool.Release(bullet);
            };
            return bullet;
        }

        public T CreateBullet()
        {
            var bullet = _bulletPool.Get();
            bullet.transform.position = _origin.transform.position;
            bullet.transform.forward = _origin.transform.forward;
            return bullet;
        }

        public T CreateBullet(Vector3 position)
        {
            var bullet = _bulletPool.Get();
            bullet.transform.position = position;
            bullet.transform.forward = _origin.transform.forward;
            return bullet;
        }
        
        public T CreateBullet(Vector3 position, Vector3 lookVector)
        {
            var bullet = _bulletPool.Get();
            bullet.transform.position = position;
            bullet.transform.forward = lookVector;
            return bullet;
        }
    }
}