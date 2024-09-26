using System;
using System.Collections.Generic;
using System.Linq;
using Guns.Projectiles;
using Guns.Projectiles.Interactions;
using Guns.Projectiles.Types;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using Zenject;

namespace Guns.General
{
    public class SpinThrowGun : Gun, ISpinnableGun, IThrowableGun, IPunchable
    {
        [SerializeField] private GunSpinContainer gunSpinContainer;
        [SerializeField] private Animator animator;
        [SerializeField] private Dictionary<AnimationClip, Vector2> animationsAndSpinThresholds;
        public bool IsSpinning { get; private set; }
        public bool IsLost { get; private set; }
        public event Action OnLost;
        public event Action OnObtained;
        public event Action SpinStarted;
        public event Action SpinEnded;

        private ProjectileFactory _gunSpinContainerFactory;
        
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _gunSpinContainerFactory = diContainer.ResolveId<ProjectileFactory>(gunSpinContainer.Config.FactoryId);
        }

        public override void Shoot()
        {
            if (IsLost) return;
            base.Shoot();
        }

        protected override void Update()
        {
            if (IsLost) return;
            base.Update();
        }

        public void StartSpin()
        {
            if (IsLost) return;
            if (IsSpinning) return;
            IsSpinning = true;
            
            SpinStarted?.Invoke();
        }

        public void EndSpin()
        {
            if (!IsSpinning) return;
            IsSpinning = false;
            SpinEnded?.Invoke();
        }

        public override void Activate()
        {
            if (IsActive) return;
            base.Activate();
            if (IsLost) Data.GunPunchCollider.enabled = false;
        }

        public void Throw()
        {
            if (IsLost) return;
            if (!CanBeThrown()) return;
            EndSpin();
            IsLost = true;
            if (IsActive) Data.GunPunchCollider.enabled = false;
            var spinContainer= _gunSpinContainerFactory.CreateProjectile(transform.position, transform.forward);
            spinContainer.Init(this);
            OnLost?.Invoke();
        }

        public void Catch()
        {
            if (!IsLost) return;
            IsLost = false;
            if (IsActive) Data.GunPunchCollider.enabled = true;
            OnObtained?.Invoke();
        }

        public void Punch(Vector3 force)
        {
            Throw();
        }

        private bool CanBeThrown()
        {
            if (IsLost) return false;
            AnimatorClipInfo clipInfo = (animator.GetCurrentAnimatorClipInfo(0)[0]);
            var time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (!animationsAndSpinThresholds.Keys.Contains(clipInfo.clip)) return false;
            int currentFrame = AnimationUtils.CurrentAnimationClipFrame(clipInfo, time);
            Vector2 animationThresholds = animationsAndSpinThresholds[clipInfo.clip];
            return animationThresholds.x <= currentFrame && animationThresholds.y >= currentFrame;
        }
    }
}