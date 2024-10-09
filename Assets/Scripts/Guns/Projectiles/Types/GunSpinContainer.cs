using Guns.General;
using Guns.Projectiles.Interactions;
using Guns.Types.SpinThrowGun;
using PrimeTween;
using UnityEngine;


namespace Guns.Projectiles.Types
{
    public class GunSpinContainer : Projectile, IPunchable
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider spinCollider;
        [Header("Returning")] 
        [SerializeField] private float minTimeToFlyForward;
        [SerializeField] private float maxTimeToFlyForward;
        [SerializeField] private float lameSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float coolReturnTime;

        private bool _returning;
        private Sequence _flyingSequence;
        private Vector3 _endPoint;
        
        private Vector3 DirectionToOrigin => Origin.transform.position - transform.position;

        
        public override void Init(Gun origin)
        {
            base.Init(origin);

            _endPoint = transform.position + transform.forward * Config.MaxDistance;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var stopHit, Config.MaxDistance, Config.BulletStopMask))
            {
                _endPoint = stopHit.point;
            }

            float timeForward = Mathf.Clamp(Vector3.Distance(transform.position, _endPoint) / Config.PushPower, minTimeToFlyForward, maxTimeToFlyForward);
            _flyingSequence = Sequence.Create().Chain(Tween.Position(transform, _endPoint, timeForward, Ease.OutBounce).OnComplete(StartFlyingBack));
        }

        public override void ResetBullet()
        {
            rb.isKinematic = true;
            spinCollider.enabled = true;
            _returning = false;
            _flyingSequence.Stop();
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                Return();
            }
        }
        
        private void StartFlyingBack()
        {
            rb.isKinematic = false;
            spinCollider.enabled = false;
            _returning = true;
        }
        
        private void FixedUpdate()
        {
            if (_returning && !TimeNotifier.IsTimeStopped)
            {
                var speed = lameSpeed * DirectionToOrigin;
                if (speed.magnitude < lameSpeed) speed = speed.normalized * lameSpeed;
                rb.velocity = speed;
                if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
            }
            
        }
        private void Return()
        {
            if (!gameObject.activeSelf) return;
            if (!_returning) return;
            if (Origin is IThrowableGun throwableGun)
            {
                throwableGun.Catch();
            }
            DestroyProjectile();
        }

        public void Punch(Vector3 force)
        { 
            if (!_returning) return;
            transform.forward = force;
            ResetBullet();
            Init(Origin);
        }
        
    }
}