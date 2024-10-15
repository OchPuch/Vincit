using Entities;
using Guns.Projectiles.Interactions;
using PrimeTween;
using TimeStop;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class HittableTarget : TimeStoppableBehaviour, IPunchable, IKickable, IDamageable, ISpinContainerTarget
    {
        [SerializeField] private float _resetTime;
        [SerializeField] private UnityEvent _resetEvent;
        [SerializeField] private UnityEvent _onHitEvent;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _hitColor;
        [SerializeField] private Transform _model;
        [SerializeField] private TweenSettings<Vector3> _hitRotaitonSettings;
        [SerializeField] private TweenSettings<Vector3> _resetRotaitonSettings;


        private Sequence _animation;
        private Color _startColor;
        private float _resetTimer;
        private bool _hitted;

        private void Awake()
        {
            _startColor = _renderer.material.color;
            _resetRotaitonSettings.settings.duration = _resetTime;
        }

        public void Punch(Vector3 force)
        {
            Hit();
        }

        public void Kick()
        {
            Hit();
        }

        public void Damage(float damage)
        {
            Hit();
        }

        public bool Steal()
        {
            if (_hitted) return false;
            Hit();
            return true;
        }

        private void Reset()
        {
            _hitted = false;
            _resetTimer = 0f;
            _resetEvent?.Invoke();
            _renderer.material.color = _startColor;
        }

        private void Hit()
        {
            if (_hitted) return;
            _hitted = true;
            _onHitEvent?.Invoke();
            _animation = Sequence.Create().Chain(Tween.LocalRotation(_model, _hitRotaitonSettings)).Chain(Tween.LocalRotation(_model, _resetRotaitonSettings)).OnComplete(Reset);
            _renderer.material.color = _hitColor;
        }

        protected override void PostTimeStop()
        {
            base.PostTimeStop();
            if (_animation.isAlive) _animation.isPaused = true;
        }

        protected override void PostTimeContinue()
        {
            base.PostTimeContinue();
            if (_animation.isAlive)  _animation.isPaused = false;    
        }
    }
}