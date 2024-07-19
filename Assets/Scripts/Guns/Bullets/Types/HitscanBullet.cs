using System;
using GlobalManagers;
using UnityEngine;

namespace Guns.Bullets.Types
{
    public class HitscanBullet : Bullet
    {
        private Vector3 _endPoint;
        private float _destroyTimer;
        
        public override void Init()
        {
            base.Init();
            _endPoint = transform.position + transform.forward * GameManager.Instance.GameSettings.MaxShootingDistance;
            Ray ray = new Ray(transform.position, transform.forward);
            float maxDistance = GameManager.Instance.GameSettings.MaxShootingDistance;
            
            if (Physics.Raycast(ray, out var stopHit, maxDistance, GameManager.Instance.GameSettings.BulletStopMask))
            {
                _endPoint = stopHit.point;
                maxDistance = Vector3.Distance(transform.position, _endPoint) + 1;
            }

            transform.position += ((_endPoint - transform.position) / 2f);
            transform.localScale = new Vector3(Config.StartScaleX, Config.StartScaleX, maxDistance);

            //var hits = Physics.RaycastAll(ray, maxDistance, GameManager.Instance.GameSettings.BulletHitMask);
        }

        public override void ResetBullet()
        {
            base.ResetBullet();
            _destroyTimer = 0f;
            transform.localScale = new Vector3(Config.StartScaleX, Config.StartScaleX, transform.localScale.z);
        }

        protected override void PostTimeStop()
        {
            base.PostTimeStop();
            _destroyTimer = 0;
            transform.localScale = new Vector3(Config.StartScaleX, Config.StartScaleX, transform.localScale.z);
        }

        private void Update()
        {
            if (TimeManager.Instance.IsTimeStopped) return;
            _destroyTimer += Time.deltaTime;
            float scale = Mathf.Lerp(Config.StartScaleX, 0, _destroyTimer / Config.DestroyTime);
            transform.localScale = new Vector3(scale, scale, transform.localScale.z);
            if (_destroyTimer > Config.DestroyTime) {
                DestroyBullet();
            }
        }
    }
}