using System.Collections.Generic;
using Guns.Projectiles.Interactions;
using RayFire;
using UnityEngine;
using Zenject;

namespace Guns.Projectiles.Types
{
    public class PunchSphere : CloseRangeProjectileSphere
    {
        [SerializeField] private WeaklingProjectile weakProjectilePrefab;
        [SerializeField] private float maxRayFireRigidShootSize;
        
        private readonly List<Vector3> _positionsToSpawnBullets = new();
        private ProjectileFactory _hitscanProjectileFactory;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _hitscanProjectileFactory = diContainer.ResolveId<ProjectileFactory>(weakProjectilePrefab.Config.FactoryId);
        }

        public override void ResetBullet()
        {
            base.ResetBullet();
            _positionsToSpawnBullets.Clear();
        }

        protected override void PostProcessCollider(Collider hitCollider)
        {
            if (hitCollider.TryGetComponent(out IPunchable punchable))
            {
                punchable.Punch(Origin.transform.forward * Config.PushPower);
                return;
            }
            base.PostProcessCollider(hitCollider);
        }

        protected override void OnPreProcessRayFireRigid(Collider hitCollider, RayfireRigid rayfireRigid)
        {
            base.OnPreProcessRayFireRigid(hitCollider, rayfireRigid);
            if (rayfireRigid.limitations.currentDepth == 0)
            {
            }
            else if (rayfireRigid.limitations.bboxSize < maxRayFireRigidShootSize)
            {
                _positionsToSpawnBullets.Add(rayfireRigid.transform.position);
                HitColliders.Remove(hitCollider);
                DestroySchedule.Enqueue(rayfireRigid.gameObject);
            }
        }
        
        protected override void OnFinishShot()
        {
            base.OnFinishShot();
            foreach (var spawnPosition in _positionsToSpawnBullets)
            {
                var bullet = _hitscanProjectileFactory.CreateProjectile(spawnPosition, spawnPosition - transform.position);
                bullet.Init(Origin);
            }
        }
    }
}