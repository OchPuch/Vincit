using GlobalManagers;
using Guns.General;
using Guns.Types.Hand;
using RayFire;
using TimeStop;
using UnityEngine;

namespace Guns.Bullets.Types
{
    public class PunchSphere : Bullet
    {
        [SerializeField] private float playerPushMultiplier = 0.3f;
        [SerializeField] private float smallApproveTime = 0.4f;
        [SerializeField] private RayfireGun rayfireGun;
        private float _destroyTime;
        private bool _needApprove;
        private bool _crushWallPunch;
        private Collider[] _hitColliders;
        private Hand HandGun => Origin as Hand;
        
        
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
            
            if (_needApprove)
            {
                HandGun.PunchApproved += OnPunchApproved;
                if (_crushWallPunch) HandGun.PunchApproved += CrushWall;
                HandGun.RequestApprove(smallApproveTime);
                return;
            }
            
            if (_crushWallPunch) CrushWall();
            FinishShoot();
        }

        private void CrushWall()
        {
            _crushWallPunch = false;
            if (_needApprove) 
            {
                HandGun.PunchApproved -= CrushWall;
                Origin.Owner.RequestPush(-transform.forward * Config.PushPower/5f, ForceMode.Impulse);
            }
            rayfireGun.Shoot();
        }

        private void FinishShoot()
        {
            foreach (var hitCollider in _hitColliders)
            {
                ProcessHit(hitCollider);
            }
            if (!TimeManager.Instance.IsTimeStopped && !_needApprove)
            {
                DestroyBullet();
            }
        }
        
        private void OnPunchApproved()
        {
            HandGun.PunchApproved -= OnPunchApproved;
            FinishShoot();
        }
        
        public override void ResetBullet()
        {
            base.ResetBullet();
            _needApprove = false;
            _crushWallPunch = false;
        }

        private void Update()
        {
            if (TimeManager.Instance.IsTimeStopped) return;
            _destroyTime += Time.deltaTime;
            if (_destroyTime > Config.DestroyTime)
            {
                DestroyBullet();
            }
        }
        
        private void PreProcessHit(Collider hitCollider)
        {
            if (hitCollider.gameObject.TryGetComponent<RayfireRigid>(out var rayfireRigid))
            {
                _crushWallPunch = true;
                
                if (rayfireRigid.limitations.currentDepth == 0)
                {
                    _needApprove = true;
                }
            }
        }

        private void ProcessHit(Collider hitCollider)
        {
            if (hitCollider.gameObject.TryGetComponent<HitscanBullet>(out var bullet))
            {
                bullet.PunchCurve(transform.position);
            }
            
            if (hitCollider.gameObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(transform.forward * Config.PushPower, ForceMode.Impulse);
                if (TimeManager.Instance.IsTimeStopped)
                {
                    if (hitCollider.gameObject.TryGetComponent<StoppableRigid>(out var stoppableRigid))
                    {
                        stoppableRigid.AddForce(transform.forward * Config.PushPower);
                    }
                }
            }
        }
    }
}