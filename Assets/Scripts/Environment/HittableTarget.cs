using Entities;
using General;
using Guns.Projectiles.Interactions;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class HittableTarget : GamePlayBehaviour, IPunchable, IKickable, IDamageable, ISpinContainerTarget
    {
        [SerializeField] private UnityEvent _onHitEvent;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _hitColor;
        [SerializeField] private Transform _model;
        [SerializeField] private TweenSettings<Vector3> _rotationSettings;

        private bool _hitted;
    
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
        
        private void Hit()
        {
            if (_hitted) return;
            _hitted = true;
            _onHitEvent?.Invoke();
            Tween.LocalRotation(_model, _rotationSettings);
            _renderer.material.color = _hitColor;
        }

       
    }
}
