﻿using System.Collections.Generic;
using System.Linq;
using Entities;
using Guns.General;
using Guns.Projectiles.View;
using Guns.Types.CloseRange;
using RayFire;
using TimeStop;
using UnityEngine;


namespace Guns.Projectiles.Types
{
    public class CloseRangeProjectileSphere : Projectile
    {
        [SerializeField] private float playerPushMultiplier = 0.3f;
        [SerializeField] private float smallApproveTime = 0.4f;
        [SerializeField] private float maxApproveTime = 0.3f;
        [SerializeField] private float extraApproveTimePerBullet = 0.025f;
        [SerializeField] private RayfireGun rayfireGun;
        [SerializeField] private CloseRangeProjectileAudio _projectileAudio;
        
        protected bool NeedApprove;
        protected bool CrushWallPunch;
        protected readonly List<HitscanProjectile> BulletsToCombine = new();

        protected List<Collider> HitColliders = new();
        protected Queue<GameObject> DestroySchedule = new();
        
        private float _destroyTime;
        private CloseRange CloseRangeGun => Origin as CloseRange;
        
        public override void Init(Gun origin)
        {
            base.Init(origin);
            Vector3 point1 = transform.position;
            Vector3 point2 = transform.position + transform.forward * Config.MaxDistance;
            HitColliders = Physics.OverlapCapsule(point1, point2, Config.StartRadius).ToList();
            foreach (var hitCollider in HitColliders.ToList())
            {
                if (hitCollider.gameObject.CompareTag("Player"))
                {
                    HitColliders.Remove(hitCollider);
                    continue;
                }
                PreProcessHit(hitCollider);
            }

            float approveForce = BulletsToCombine.Count * 0.2f;
            if (NeedApprove)
            {
                float extraApproveTimeForBullets = BulletsToCombine.Count > 1 ? BulletsToCombine.Count * extraApproveTimePerBullet : 0;
                float approveTime = Mathf.Clamp(smallApproveTime + extraApproveTimeForBullets, smallApproveTime, maxApproveTime);
                if (CrushWallPunch)
                {
                    approveForce += 1f;
                    CloseRangeGun.PunchApproved += CrushWall;
                }
                
                CloseRangeGun.PunchApproved += OnPunchApproved;
                CloseRangeGun.RequestApprove(approveTime, approveForce);
                return;
            }
            
            if (CrushWallPunch) CrushWall(approveForce);
            FinishShoot();
        }

        protected virtual void CrushWall(float f)
        {
            CrushWallPunch = false;
            rayfireGun.Shoot();
            if (NeedApprove) 
            {
                CloseRangeGun.PunchApproved -= CrushWall;
            }
        }
        
        private void FinishShoot()
        {
            CombineBullets();
            _projectileAudio.PlaySwing();
            if (HitColliders.Any(x => !x.isTrigger))
            {
                _projectileAudio.PlayPunch();
            }
            foreach (var hitCollider in HitColliders.ToList())
            {
                //Debug.Log(hitCollider.gameObject.name);
                ProcessHit(hitCollider);
            }
            OnFinishShot();
            if (!TimeNotifier.IsTimeStopped && !NeedApprove)
            {
                DestroyProjectile();
            }

        }

        protected virtual void OnFinishShot()
        {
            for (int i = 0; i < DestroySchedule.Count; i++)
            {
                if (DestroySchedule.TryDequeue(out var objectToDestroy))
                {
                    Destroy(objectToDestroy);
                }
                else
                {
                    break;
                }
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

        private void OnPunchApproved(float f)
        {
            CloseRangeGun.PunchApproved -= OnPunchApproved;
            FinishShoot();
        }
        
        public override void ResetBullet()
        {
            NeedApprove = false;
            CrushWallPunch = false;
            BulletsToCombine.Clear();
        }

        private void Update()
        {
            if (TimeNotifier.IsTimeStopped) return;
            _destroyTime += Time.deltaTime;
            if (_destroyTime > Config.DestroyTime)
            {
                DestroyProjectile();
            }
        }
        
        private void PreProcessHit(Collider hitCollider)
        {
            if (hitCollider.gameObject.TryGetComponent<HitscanProjectile>(out var bullet))
            {
                if (!bullet.IsOverloaded)
                {
                    BulletsToCombine.Add(bullet);
                    if (BulletsToCombine.Count > 1) NeedApprove = true;
                }
            }
            
            if (hitCollider.gameObject.TryGetComponent<RayfireRigid>(out var rayfireRigid))
            {
                CrushWallPunch = true;
                OnPreProcessRayFireRigid(hitCollider, rayfireRigid);
            }
        }

        protected virtual void OnPreProcessRayFireRigid(Collider hitCollider , RayfireRigid rayfireRigid)
        {
            
        }

        private void ProcessHit(Collider hitCollider)
        {
            if (hitCollider.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable is not Player.Player)
                {
                    damageable.Damage(Config.Damage);
                    PostProcessDamageable(damageable);
                }
            }
            PostProcessCollider(hitCollider);
        }
    
        protected virtual void PostProcessRigidbody(Rigidbody rb)
        {
            
        }

        protected virtual void PostProcessStoppableRigid(StoppableRigid stoppableRigid)
        {
            
        }

        protected virtual void PostProcessDamageable(IDamageable damageable)
        {
            
        }

        protected virtual void PostProcessCollider(Collider hitCollider)
        {
            if (!hitCollider.gameObject.TryGetComponent<Rigidbody>(out var rb)) return;
            rb.AddForce(transform.forward * Config.PushPower, ForceMode.Impulse);
            PostProcessRigidbody(rb);
            if (TimeNotifier.IsTimeStopped)
            {
                if (hitCollider.gameObject.TryGetComponent<StoppableRigid>(out var stoppableRigid))
                {
                    stoppableRigid.AddForce(transform.forward * Config.PushPower);
                    PostProcessStoppableRigid(stoppableRigid);
                }
            }
        }
    }
}