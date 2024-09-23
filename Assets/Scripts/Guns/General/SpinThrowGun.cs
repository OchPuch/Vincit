using System;
using Guns.Projectiles;
using Guns.Projectiles.Interactions;
using Guns.Projectiles.Types;
using UnityEngine;
using Zenject;

namespace Guns.General
{
    public class SpinThrowGun : Gun, ISpinnableGun, IThrowableGun, IPunchable
    {
        [SerializeField] private GunSpinContainer gunSpinContainer;
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
            if (IsSpinning)
            {
                Data.spinTimer += Time.deltaTime;
            }
        }

        public void StartSpin()
        {
            if (IsSpinning) return;
            IsSpinning = true;
            
            SpinStarted?.Invoke();
        }

        public void EndSpin()
        {
            if (!IsSpinning) return;
            Data.spinTimer = 0f;
            
            SpinEnded?.Invoke();
        }

        public void Throw()
        {
            if (IsLost) return;
            if (IsSpinning && Data.spinTimer > Data.Config.TimeBeforeAbleToThrow)
            {
                IsLost = true;
                var spinContainer= _gunSpinContainerFactory.CreateProjectile(transform.position, transform.forward);
                spinContainer.Init(this);
                OnLost?.Invoke();
            }
        }

        public void Catch()
        {
            if (!IsLost) return;
            IsLost = false;
            
            OnObtained?.Invoke();
        }

        public void Punch()
        {
            Throw();
        }
    }
}