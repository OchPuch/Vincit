using System;
using GlobalManagers;
using Guns.General;
using TimeStop;
using UnityEngine;

namespace Guns.Bullets.Types
{
    public class HitscanBullet : Bullet
    {
        private Vector3 _endPoint;
        private Vector3 _startPoint;
        private float _destroyTimer;
        private bool _isOverloaded;

        public event Action Overload;
        
        
        public override void Init(Gun origin)
        {
            base.Init(origin);
            
            _startPoint = transform.position;
            _endPoint = transform.position + transform.forward * GameManager.Instance.GameSettings.MaxShootingDistance;
            
            Ray ray = new Ray(transform.position, transform.forward);
            float maxDistance = GameManager.Instance.GameSettings.MaxShootingDistance;
            
            if (Physics.Raycast(ray, out var stopHit, maxDistance, GameManager.Instance.GameSettings.BulletStopMask))
            {
                _endPoint = stopHit.point;
                maxDistance = Vector3.Distance(transform.position, _endPoint) + 1;
            }
            
            var hits = Physics.RaycastAll(ray, maxDistance, GameManager.Instance.GameSettings.BulletHitMask);
            foreach (var hit in hits)
            {
                ProcessHit(hit);
            }
            
            UpdateTransform();
        }

        private void UpdateTransform()
        {
            float maxDistance = Vector3.Distance(_startPoint, _endPoint);
            transform.position = _startPoint + ((_endPoint - _startPoint) / 2f);
            transform.localScale = new Vector3(Config.StartRadius, Config.StartRadius, maxDistance);
        }

        private void ProcessHit(RaycastHit hit)
        {
            if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(transform.forward * Config.PushPower, ForceMode.Impulse);
                if (TimeManager.Instance.IsTimeStopped)
                {
                    if (hit.collider.gameObject.TryGetComponent<StoppableRigid>(out var stoppableRigid))
                    {
                        stoppableRigid.AddForce(transform.forward * Config.PushPower);
                    }
                }
            }
        }

        public void PunchCurve(Vector3 punchPoint)
        {
            if (_isOverloaded) return;
            _isOverloaded = true;
            _endPoint = punchPoint;
            UpdateTransform();
            var bullet = Origin.BulletFactory.CreateBullet(punchPoint);
            bullet.Init(Origin);
            Overload?.Invoke();
        }

        public override void ResetBullet()
        {
            base.ResetBullet();
            _destroyTimer = 0f;
            _isOverloaded = false;
            transform.localScale = new Vector3(Config.StartRadius, Config.StartRadius, transform.localScale.z);
        }

        protected override void PostTimeStop()
        {
            base.PostTimeStop();
            _destroyTimer = 0;
            transform.localScale = new Vector3(Config.StartRadius, Config.StartRadius, transform.localScale.z);
        }

        private void Update()
        {
            if (TimeManager.Instance.IsTimeStopped) return;
            _destroyTimer += Time.deltaTime;
            float scale = Mathf.Lerp(Config.StartRadius, 0, _destroyTimer / Config.DestroyTime);
            transform.localScale = new Vector3(scale, scale, transform.localScale.z);
            if (_destroyTimer > Config.DestroyTime) {
                DestroyBullet();
            }
        }
    }
}