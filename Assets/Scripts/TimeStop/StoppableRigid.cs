using UnityEngine;

namespace TimeStop
{
    [RequireComponent(typeof(Rigidbody))]
    public class StoppableRigid : TimeStoppableBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float timeStoppedDrag;
        [SerializeField] private float timeStoppedAngularDrag;
        
        private Vector3 _cumulativeForceInStoppedTime;
        private float _bufferedDrag;
        private float _bufferedAngularDrag;
        private Vector3 _bufferedVelocity = Vector3.zero;
        private Vector3 _bufferedAngularVelocity = Vector3.zero;
        private bool _bufferedUseGravity;
        private Vector3 _changedVelocity = Vector3.zero;
        
        protected override void Start()
        {
            base.Start();
            _bufferedUseGravity = rb.useGravity;
            _bufferedDrag = rb.linearDamping;
            _bufferedAngularDrag = rb.angularDamping;
        }

        protected override void PostTimeStop()
        {
            _bufferedVelocity = rb.linearVelocity;
            _bufferedUseGravity = rb.useGravity;
            _bufferedAngularVelocity = rb.angularVelocity;
            
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.linearDamping = timeStoppedDrag;
            rb.angularDamping = timeStoppedAngularDrag;
            _changedVelocity = Vector3.zero;
        }

        private void Update()
        {
            if (!TimeNotifier.IsTimeStopped) return;
            if (rb.linearVelocity.magnitude > _changedVelocity.magnitude)
            {
                _changedVelocity = rb.linearVelocity;
            }
        }

        protected override void PostTimeContinue()
        {
            rb.linearDamping = _bufferedDrag;
            rb.angularDamping = _bufferedAngularDrag;
            rb.useGravity = _bufferedUseGravity;
            rb.angularVelocity = _bufferedAngularVelocity;
            if (_changedVelocity != Vector3.zero)
            {
                rb.linearVelocity = _changedVelocity + _changedVelocity.normalized * _bufferedVelocity.magnitude;
            }
            else
            {
                rb.linearVelocity = _bufferedVelocity;
            }
            
            rb.AddForce(_cumulativeForceInStoppedTime, ForceMode.Impulse);
            _cumulativeForceInStoppedTime = Vector3.zero;
        }

        public void AddForce(Vector3 force)
        {
            _cumulativeForceInStoppedTime += force;
        }
    }
}