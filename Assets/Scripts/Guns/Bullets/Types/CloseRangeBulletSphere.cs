using System.Collections.Generic;
using Entities;
using Guns.General;
using Guns.Types.CloseRange;
using RayFire;
using TimeStop;
using UnityEngine;
using Zenject;

namespace Guns.Bullets.Types
{
    public class CloseRangeBulletSphere : Bullet
    {
        [SerializeField] private WeaklingBullet weakBulletPrefab;
        [SerializeField] private float maxRayFireRigidShootSize;
        [SerializeField] private float playerPushMultiplier = 0.3f;
        [SerializeField] private float smallApproveTime = 0.4f;
        [SerializeField] private float maxApproveTime = 0.3f;
        [SerializeField] private float extraApproveTimePerBullet = 0.025f;
        [SerializeField] private RayfireGun rayfireGun;
        
        protected bool NeedApprove;
        protected readonly List<HitscanBullet> BulletsToCombine = new();
        
        private Collider[] _hitColliders;
        private readonly List<Vector3> _positionsToSpawnBullets = new();
        
        private BulletFactory _hitscanBulletFactory;
        private float _destroyTime;
        private bool _crushWallPunch;
        private CloseRange CloseRangeGun => Origin as CloseRange;
        
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _hitscanBulletFactory = diContainer.ResolveId<BulletFactory>(weakBulletPrefab.Config.FactoryId);
        }
        
        public override void Init(Gun origin)
        {
            base.Init(origin);
            
            Vector3 point1 = transform.position;
            Vector3 point2 = transform.position + transform.forward * Config.MaxDistance;
            _hitColliders = Physics.OverlapCapsule(point1, point2, Config.StartRadius);
            foreach (var hitCollider in _hitColliders)
            {
                PreProcessHit(hitCollider);
            }
            
            if (NeedApprove)
            {
                float extraApproveTimeForBullets = BulletsToCombine.Count > 1 ? BulletsToCombine.Count * extraApproveTimePerBullet : 0;
                float approveTime = Mathf.Clamp(smallApproveTime + extraApproveTimeForBullets, smallApproveTime, maxApproveTime);
                if (_crushWallPunch)
                {
                    CloseRangeGun.PunchApproved += CrushWall;
                }
                
                CloseRangeGun.PunchApproved += OnPunchApproved;
                CloseRangeGun.RequestApprove(approveTime);
                return;
            }
            
            if (_crushWallPunch) CrushWall();
            FinishShoot();
        }

        protected virtual void CrushWall()
        {
            _crushWallPunch = false;
            if (NeedApprove) 
            {
                CloseRangeGun.PunchApproved -= CrushWall;
            }
        }
        
        private void FinishShoot()
        {
            CombineBullets();
            foreach (var spawnPosition in _positionsToSpawnBullets)
            {
               var bullet = _hitscanBulletFactory.CreateBullet(spawnPosition, spawnPosition - transform.position);
               bullet.Init(Origin);
            }
            foreach (var hitCollider in _hitColliders)
            {
                ProcessHit(hitCollider);
            }
            if (!TimeNotifier.IsTimeStopped && !NeedApprove)
            {
                DestroyBullet();
            }
        }

        protected virtual void CombineBullets()
        {
            if (BulletsToCombine.Count > 1)
            {
                BulletsToCombine[0].PunchCurveConsume(transform.position, Origin.transform.forward ,BulletsToCombine);
            }
            else if (BulletsToCombine.Count > 0)
            {
                BulletsToCombine[0].PunchCurve(transform.position, Origin.transform.forward);
            }
        }

        private void OnPunchApproved()
        {
            CloseRangeGun.PunchApproved -= OnPunchApproved;
            FinishShoot();
        }
        
        public override void ResetBullet()
        {
            base.ResetBullet();
            NeedApprove = false;
            _crushWallPunch = false;
            _positionsToSpawnBullets.Clear();
            BulletsToCombine.Clear();
        }

        private void Update()
        {
            if (TimeNotifier.IsTimeStopped) return;
            _destroyTime += Time.deltaTime;
            if (_destroyTime > Config.DestroyTime)
            {
                DestroyBullet();
            }
        }
        
        private void PreProcessHit(Collider hitCollider)
        {
            if (hitCollider.gameObject.TryGetComponent<HitscanBullet>(out var bullet))
            {
                if (!bullet.IsOverloaded)
                {
                    BulletsToCombine.Add(bullet);
                    if (BulletsToCombine.Count > 1) NeedApprove = true;
                }
            }
            
            if (hitCollider.gameObject.TryGetComponent<RayfireRigid>(out var rayfireRigid))
            {
                _crushWallPunch = true;
                if (rayfireRigid.limitations.currentDepth == 0)
                {
                    NeedApprove = true;
                }
                else if (rayfireRigid.limitations.bboxSize < maxRayFireRigidShootSize)
                {
                    _positionsToSpawnBullets.Add(rayfireRigid.transform.position);
                    Destroy(rayfireRigid);
                }
            }
        }

        private void ProcessHit(Collider hitCollider)
        {
            if (hitCollider.gameObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(transform.forward * Config.PushPower, ForceMode.Impulse);
                if (TimeNotifier.IsTimeStopped)
                {
                    if (hitCollider.gameObject.TryGetComponent<StoppableRigid>(out var stoppableRigid))
                    {
                        stoppableRigid.AddForce(transform.forward * Config.PushPower);
                    }
                }
            }

            if (hitCollider.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable is not Player.Player)
                {
                    damageable.Damage(Config.Damage);
                }
            }
        }
    }
}