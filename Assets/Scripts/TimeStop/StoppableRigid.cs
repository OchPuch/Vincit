using GlobalManagers;
using UnityEngine;

namespace TimeStop
{
    [RequireComponent(typeof(Rigidbody))]
    public class StoppableRigid : TimeStoppableBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float timeStoppedDrag;
        [SerializeField] private float timeStoppedAngularDrag;

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
            _bufferedDrag = rb.drag;
            _bufferedAngularDrag = rb.angularDrag;
        }

        protected override void PostTimeStop()
        {
            _bufferedVelocity = rb.velocity;
            _bufferedUseGravity = rb.useGravity;
            _bufferedAngularVelocity = rb.angularVelocity;
            
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.drag = timeStoppedDrag;
            rb.angularDrag = timeStoppedAngularDrag;
            _changedVelocity = Vector3.zero;
        }

        private void Update()
        {
            if (!TimeManager.Instance.IsTimeStopped) return;
            if (rb.velocity.magnitude > _changedVelocity.magnitude)
            {
                _changedVelocity = rb.velocity;
            }
        }

        protected override void PostTimeContinue()
        {
            rb.drag = _bufferedDrag;
            rb.angularDrag = _bufferedAngularDrag;
            rb.useGravity = _bufferedUseGravity;
            rb.angularVelocity = _bufferedAngularVelocity;
            if (_changedVelocity != Vector3.zero)
            {
                rb.velocity = _changedVelocity + _changedVelocity.normalized * _bufferedVelocity.magnitude;
            }
            else
            {
                rb.velocity = _bufferedVelocity;
            }
        }
    }
}