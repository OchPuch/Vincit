using System;
using System.Collections.Generic;
using System.Linq;
using Guns.General;
using Guns.Projectiles;
using Guns.Projectiles.Interactions;
using Guns.Projectiles.Types;
using Player.Data;
using UnityEngine;
using Utils;
using Zenject;

namespace Guns.Types.SpinThrowGun
{
    public class SpinThrowGun : Gun, IPunchable, ISpinnableGun, IThrowableGun
    {
        [SerializeField] private GunSpinContainer gunSpinContainer;
        [SerializeField] private Animator animator;
        [SerializeField] private Dictionary<AnimationClip, Vector2> animationsAndSpinThresholds;
        public bool IsSecondAbilityActive { get; private set; }
        public bool IsLost { get; private set; }
        public bool IsSpinning { get; private set; }
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
            if (IsSecondAbilityActive)
            {
                Data.fireTimer += Data.Config.SpinFireSpeedAdd * Time.deltaTime;
            }
            else
            {
                Data.currentSpinSpeed -= Data.Config.SpinSpeed / Data.Config.SpinStopTime * Time.deltaTime;
                if (Data.currentSpinSpeed <= 0) Data.currentSpinSpeed = 0;
            }
        }

        private void FixedUpdate()
        {
            if (IsSecondAbilityActive)
            {
                Owner.RequestPush(Owner.Data.motor.CharacterUp * Data.Config.HelicopterForce, ForceMode.Force, false, PushBasedOnGroundStatus.OnlyIfUnstable);
            }
        }
        

        public void StartSpin()
        {
            if (IsLost) return;
            if (IsSecondAbilityActive) return;
            IsSecondAbilityActive = true;
            Data.currentSpinSpeed = Data.Config.SpinSpeed;
            
            SpinStarted?.Invoke();
        }

        public void EndSpin()
        {
            if (!IsSecondAbilityActive) return;
            IsSecondAbilityActive = false;
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
            Data.fireTimer = 0;
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