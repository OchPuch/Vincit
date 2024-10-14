using KinematicCharacterController.Core;
using TimeStop;
using UnityEngine;

namespace KinematicCharacterController.Examples
{
    public class ExampleMovingPlatform : TimeStoppableBehaviour, IMoverController
    {
        public PhysicsMover Mover;

        public Vector3 TranslationAxis = Vector3.right;
        public float TranslationPeriod = 10;
        public float TranslationSpeed = 1;
        public Vector3 RotationAxis = Vector3.up;
        public float RotSpeed = 10;
        public Vector3 OscillationAxis = Vector3.zero;
        public float OscillationPeriod = 10;
        public float OscillationSpeed = 10;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        private float _timer;
        
        protected override void Start()
        {
            base.Start();
            _originalPosition = Mover.Rigidbody.position;
            _originalRotation = Mover.Rigidbody.rotation;

            Mover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            if (TimeNotifier.IsTimeStopped)
            {
                goalPosition = transform.position;
                goalRotation = transform.rotation;
                return;
            }
            _timer += deltaTime;
            goalPosition = (_originalPosition + (TranslationAxis.normalized * (Mathf.Sin(_timer * TranslationSpeed) * TranslationPeriod)));
            Quaternion targetRotForOscillation = Quaternion.Euler(OscillationAxis.normalized * (Mathf.Sin(_timer * OscillationSpeed) * OscillationPeriod)) * _originalRotation;
            goalRotation = Quaternion.Euler(RotationAxis * (RotSpeed * _timer)) * targetRotForOscillation;
        }
        
    }
}