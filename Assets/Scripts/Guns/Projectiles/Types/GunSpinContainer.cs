using System;
using System.Collections;
using Guns.General;
using Guns.Projectiles.Interactions;
using UnityEngine;

namespace Guns.Projectiles.Types
{
    public class GunSpinContainer : Projectile, IPunchable
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider spinCollider;
        [SerializeField] private Collider pickupCollider;
        [Header("Timings")] 
        [SerializeField] private float lameSpeed;
        [SerializeField] private float coolReturnTime;
        private Coroutine _returnRoutine;
        private bool _collided;
        private float DistanceToGun => Vector3.Distance(transform.position, Origin.transform.position);
        
        public override void Init(Gun origin)
        {
            base.Init(origin);
            rb.velocity = transform.forward * Config.PushPower;
        }

        public override void ResetBullet()
        {
            base.ResetBullet();
            pickupCollider.enabled = true;
            pickupCollider.isTrigger = true;
            spinCollider.enabled = true;
            spinCollider.isTrigger = false;
            if (_returnRoutine is not null) StopCoroutine(_returnRoutine);
            _returnRoutine = null;
            rb.velocity = Vector3.zero;
        }

        private float GetLameTime()
        {
            return DistanceToGun / lameSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Return();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_returnRoutine is not null) return;
            _returnRoutine = StartCoroutine(ReturnRoutine(GetLameTime()));
        }

        private IEnumerator ReturnRoutine(float time)
        {
            spinCollider.isTrigger = true;
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            while (elapsedTime < time)
            {
                if (PauseNotifier.IsPaused)
                {
                    yield return null;
                    continue;
                }
                transform.forward = Origin.transform.position - transform.position;
                transform.position = Vector3.Lerp(startPosition, Origin.transform.position, elapsedTime / time);
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            Return();
        }

        private void FixedUpdate()
        {
            if (DistanceToGun > Config.MaxDistance)
            {
                _returnRoutine = StartCoroutine(ReturnRoutine(GetLameTime()));
            }
        }

        private void Return()
        {
            if (!gameObject.activeSelf) return;
            if (!_collided) return;
            if (_returnRoutine is not null) StopCoroutine(_returnRoutine);
            if (Origin is IThrowableGun throwableGun)
            {
                throwableGun.Catch();
            }
            DestroyProjectile();
        }

        public void Punch()
        {
            ResetBullet();
            Init(Origin);
        }
    }
}