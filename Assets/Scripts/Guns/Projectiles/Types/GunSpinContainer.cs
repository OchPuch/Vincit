using Guns.General;
using Guns.Projectiles.Interactions;
using UnityEngine;
using Utils;

namespace Guns.Projectiles.Types
{
    public class GunSpinContainer : Projectile, IPunchable
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider spinCollider;
        [Header("Returning")] 
        [SerializeField] private float distanceForLeavingPlayer;
        [SerializeField] private float lameSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float coolReturnTime;
        private bool _collided;
        private bool _leavedPlayer;
        private bool _forceReturn;

        private bool MustReturn => DistanceToGun > Config.MaxDistance|| (_collided && _leavedPlayer) || _forceReturn;
        private bool ReadyToReturn => ((_collided || TimeNotifier.IsTimeStopped) && _leavedPlayer) || _forceReturn;
        private Vector3 DirectionToOrigin => Origin.transform.position - transform.position;
        private float DistanceToGun => Vector3.Distance(transform.position, Origin.transform.position);
        
        public override void Init(Gun origin)
        {
            base.Init(origin);
            spinCollider.enabled = false;
            rb.velocity = transform.forward * Config.PushPower;
            rb.velocity += Origin.Owner.Data.motor.Velocity;
        }

        public override void ResetBullet()
        {
            base.ResetBullet();
            _forceReturn = false;
            _collided = false;
            _leavedPlayer = false;
            spinCollider.enabled = true;
            spinCollider.isTrigger = false;
            rb.velocity = Vector3.zero;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, Config.BulletStopMask))
            {
                if (_leavedPlayer)
                {
                    _collided = true;
                    StartReturn();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                Return();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_leavedPlayer) return;
            if (other.TryGetComponent(out Player.Player player))
            {
                LeavePlayer();
            }
        }

        private void FixedUpdate()
        {
            if (DistanceToGun > 5)
            {
                LeavePlayer();
            }
            
            if (MustReturn)
            {
                ForceStartReturn();
                var speed = lameSpeed * DirectionToOrigin;
                if (speed.magnitude < lameSpeed) speed = speed.normalized * lameSpeed;
                rb.velocity = speed;
            }
            
            if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;

        }

        private void Return()
        {
            if (!ReadyToReturn) return;
            if (!gameObject.activeSelf) return;
            if (Origin is IThrowableGun throwableGun)
            {
                throwableGun.Catch();
            }
            DestroyProjectile();
        }

        public void Punch(Vector3 force)
        {
            if (!ReadyToReturn) return;
            transform.forward = force;
            ResetBullet();
            Init(Origin);
        }

        private void ForceStartReturn()
        {
            _forceReturn = true;
            StartReturn();
        }

        private void StartReturn()
        {
            spinCollider.enabled = false;
        }

        private void InstantReturn()
        {
            
        }
        
        private void LeavePlayer()
        {
            if (_leavedPlayer) return;
            _leavedPlayer = true;
            spinCollider.enabled = true;
        }
        
    }
}