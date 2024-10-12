using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guns.General;
using Guns.Interfaces.Spin;
using Guns.Interfaces.Throw;
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
        public bool IsSpinning => Data.CurrentSpinSpeed > 0;
        private bool _spinRequest;
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

        protected override bool CanShot()
        {
            return base.CanShot() && !IsLost;
        }

        protected override IEnumerator ReloadRoutine()
        {
            var waitTime = Data.Config.ReloadTIme / Data.Config.MagSize;
            if (Data.FireTimer < 0)
            {
                waitTime = Mathf.Abs(Data.FireTimer) / Data.Config.MagSize;
            }

            foreach (var capsuleHolder in Data.CapsuleHolders)
            {
                float elapsedTime = 0;
                while (elapsedTime < waitTime)
                {
                    if (IsSpinning)
                    {
                        yield return null;
                        continue;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                capsuleHolder.ReloadSame();
            }
            
            InvokeStopReload();
        }

        protected override void Update()
        {
            if (IsLost) return;
            if (Data.FireTimer >= 0 || !IsSpinning) base.Update();
            ProcessSpinRequest(_spinRequest);
            if (Data.Config.SpinMaxSpeed > 0 && Data.FireTimer >= 0)
            {
                Data.FireTimer += Data.Config.SpinFireSpeedAdd * Data.CurrentSpinSpeed/Data.Config.SpinMaxSpeed  * Time.deltaTime;
            }
        }

        protected virtual void ProcessSpinRequest(bool spinRequest)
        {
            if (spinRequest)
            {
                Data.CurrentSpinSpeed += Data.Config.SpinAcceleration * Time.deltaTime;
                if (Data.CurrentSpinSpeed >= Data.Config.SpinMaxSpeed) Data.CurrentSpinSpeed = Data.Config.SpinMaxSpeed;
            }
            else
            {
                Data.CurrentSpinSpeed -= Data.Config.SpinDeacceleration * Time.deltaTime;
                if (Data.CurrentSpinSpeed <= 0) Data.CurrentSpinSpeed = 0;
            }
        }

        private void FixedUpdate()
        {
            if (!(Data.Config.SpinMaxSpeed > 0)) return;
            if (!IsSpinning) return;
            Owner.RequestPush(Owner.Data.motor.CharacterUp * (Data.Config.HelicopterForce * Data.CurrentSpinSpeed/Data.Config.SpinMaxSpeed), ForceMode.Force, false, PushBasedOnGroundStatus.OnlyIfUnstable);

        }

        public void StartSpin()
        {
            if (IsLost) return;
            _spinRequest = true;
            Data.CurrentSpinSpeed = Data.Config.SpinMaxSpeed;
            SpinStarted?.Invoke();
        }

        public void EndSpin()
        {
            _spinRequest = false;
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
            Data.FireTimer = 0;
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