﻿using System;
using System.Collections.Generic;
using Entities;
using Guns.General;
using Guns.Projectiles.Interactions;
using RayFire;
using TimeStop;
using UnityEngine;
using Zenject;

namespace Guns.Projectiles.Types
{
    public class ConsumeData
    {
        public float Scale = 1f;
        public int Overloads = 0;
        public static ConsumeData operator +(ConsumeData a, ConsumeData b)
        {
            return new ConsumeData
            {
                Scale = a.Scale + b.Scale,
                Overloads = Mathf.Min(a.Overloads,b.Overloads)
            };
        }
    }
    
    public class HitscanProjectile : Projectile, IOverloadable
    {
        [SerializeField] private RayfireGun rayfireGun;
        
        private ProjectileFactory _hitscanProjectileFactory;
        private Vector3 _endPoint;
        private Vector3 _startPoint;
        private float _destroyTimer;
        
        public bool IsOverloaded { get; private set;}
        public event Action Overloaded;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _hitscanProjectileFactory = diContainer.ResolveId<ProjectileFactory>(Config.FactoryId);
        }
        
        public override void Init(Gun origin)
        {
            base.Init(origin);
            
            _startPoint = transform.position;
            _endPoint = transform.position + transform.forward * Config.MaxDistance;
            
            Ray ray = new Ray(transform.position, transform.forward);
            float maxDistance = Config.MaxDistance;
            
            if (Physics.Raycast(ray, out var stopHit, maxDistance, Config.BulletStopMask))
            {
                _endPoint = stopHit.point;
                maxDistance = Vector3.Distance(transform.position, _endPoint) + 1;
            }
            
            var hits = Physics.RaycastAll(ray, maxDistance, Config.BulletHitMask);
            foreach (var hit in hits)
            {
                ProcessHit(hit);
            }
            
            rayfireGun.Shoot();
            
            UpdateTransform();
        }
        

        public void OverloadEndPosition(Vector3 endPoint)
        {
            if (IsOverloaded) return;
            IsOverloaded = true;
            _endPoint = endPoint;
            UpdateTransform();
            Overloaded?.Invoke();
        }

        public void Overload()
        {
            if (IsOverloaded) return;
            IsOverloaded = true;
            OnOverload();
            Overloaded?.Invoke();
        }

        protected virtual void OnOverload()
        {
            
        }

        public void UpdateTransform()
        {
            float maxDistance = Vector3.Distance(_startPoint, _endPoint);
            transform.forward = (_endPoint - _startPoint).normalized;
            transform.position = _startPoint + ((_endPoint - _startPoint) / 2f);
            transform.localScale = new Vector3(Config.StartRadius * ConsumeData.Scale, Config.StartRadius * ConsumeData.Scale, maxDistance);
        }

        private void ProcessHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(Config.Damage);
            }
            
            if (hit.collider.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(transform.forward * Config.PushPower, ForceMode.Impulse);
                if (TimeNotifier.IsTimeStopped)
                {
                    if (hit.collider.gameObject.TryGetComponent<StoppableRigid>(out var stoppableRigid))
                    {
                        stoppableRigid.AddForce(transform.forward * Config.PushPower);
                    }
                }
            }
        }
        
        /// <summary>
        /// When projectile is being punched (by hand) 
        /// </summary>
        /// <param name="projectile"> new projectile produced by punch </param>
        protected virtual void OnBulletPunchedWithNewBullet(Projectile projectile)
        {
            projectile.ConsumeData.Overloads += 1;
        }

        /// <summary>
        ///  Punch projectile
        /// </summary>
        /// <param name="punchPoint"> point at which it will be overloaded</param>
        /// <param name="forward"> direction of overload</param>
        public virtual void PunchCurve(Vector3 punchPoint, Vector3 forward)
        {
            if (IsOverloaded) return;
            var bullet = (HitscanProjectile) _hitscanProjectileFactory.CreateProjectile(punchPoint, forward);
            bullet.ConsumeData = ConsumeData;
            bullet.Init(Origin);
            OnBulletPunchedWithNewBullet(bullet);
            OverloadEndPosition(punchPoint);
        }
        
        /// <summary>
        ///  Punch projectile and merge it with other punched projectiles 
        /// </summary>
        /// <param name="punchPoint"> point at which it will be overloaded</param>
        /// <param name="forward"> direction of overload</param>
        /// <param name="bulletsToCombine"> list of bullets whose were punched</param>
        public virtual void PunchCurveConsume(Vector3 punchPoint, Vector3 forward ,List<HitscanProjectile> bulletsToCombine)
        {
            if (IsOverloaded) return;
            var bullet =  _hitscanProjectileFactory.CreateProjectile(punchPoint, forward);
            bullet.ConsumeData = ConsumeData;
            foreach (HitscanProjectile bulletCombine in bulletsToCombine)
            {
                bulletCombine.OverloadEndPosition(punchPoint);
                if (bulletCombine == this)
                {
                    continue;
                }
                bullet.ConsumeData += bulletCombine.ConsumeData;
            }
            OnBulletPunchedWithNewBullet(bullet);
            bullet.Init(Origin);
        }

        public override void ResetBullet()
        {
            ConsumeData = new ConsumeData();
            _destroyTimer = 0f;
            IsOverloaded = false;
            transform.localScale = new Vector3(Config.StartRadius * ConsumeData.Scale, Config.StartRadius * ConsumeData.Scale, transform.localScale.z);
        }

        protected override void PostTimeStop()
        {
            base.PostTimeStop();
            _destroyTimer = 0;
            transform.localScale = new Vector3(Config.StartRadius * ConsumeData.Scale, Config.StartRadius * ConsumeData.Scale, transform.localScale.z);
        }

        private void Update()
        {
            if (TimeNotifier.IsTimeStopped) return;
            _destroyTimer += Time.deltaTime;
            float scale = Mathf.Lerp(0, Config.StartRadius, Config.DisappearAnimation.Evaluate(_destroyTimer / Config.DestroyTime)) * ConsumeData.Scale;
            transform.localScale = new Vector3(scale, scale, transform.localScale.z);
            if (_destroyTimer > Config.DestroyTime) {
                DestroyProjectile();
            }
        }

        
    }
}