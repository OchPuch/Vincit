using GlobalManagers;
using Guns.General;
using TimeStop;
using UnityEngine;

namespace Guns.Bullets.Types
{
    public class PunchSphere : Bullet
    {
        private float _destroyTime;
        public override void Init(Gun origin)
        {
            base.Init(origin);
            var hitColliders = Physics.OverlapSphere(transform.position, Config.StartRadius);
            foreach (var hitCollider in hitColliders)
            {
                ProcessHit(hitCollider);
            }

            if (!TimeManager.Instance.IsTimeStopped)
            {
                DestroyBullet();
            }
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