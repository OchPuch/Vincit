using System;
using GlobalManagers;
using Guns.General;
using UnityEngine;

namespace Guns.Types.Hand
{
    public class Hand : Gun
    {
        private float _currentApproveTime;
        private bool _approveRequested;
        private float _approveTimer;
        public event Action PunchApproved;

        protected override void Shoot()
        {
            _approveTimer = 0;
            _approveRequested = false;
            base.Shoot();
        }

        protected override void Update()
        {
            base.Update();
            if (_approveRequested)
            {
                _approveTimer += Time.unscaledDeltaTime;
                if (_approveTimer > _currentApproveTime)
                {
                    ApprovePunch();
                }
            }
        }
        
        public override void HandleInput(GunInput input)
        {
            if (input.HandPunchRequest && Data.fireTimer > Data.Config.FireRate)
            {
                Shoot();
                InvokeShot();
                Data.fireTimer = 0;
            }
        }

        public void RequestApprove(float approveTime)
        {
            if (_approveRequested) return;
            _approveRequested = true;
            _currentApproveTime = approveTime;
            TimeManager.Instance.FreezeTimeEffectStart(approveTime);
        }

        private void ApprovePunch()
        {
            if (!_approveRequested) return;
            _approveRequested = false;
            TimeManager.Instance.StopFreezeTimeEffect();
            PunchApproved?.Invoke();
        }
        

    }
}