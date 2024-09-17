using System;
using General.GlobalManagers;
using Guns.General;
using UnityEngine;
using Zenject;

namespace Guns.Types.Hand
{
    public class CloseRange : Gun
    {
        private float _currentApproveTime;
        private bool _approveRequested;
        private float _approveTimer;

        private TimeController _timeController;
        public event Action PunchApproved;
        
        [Inject]
        public void Construct(TimeController timeController)
        {
            _timeController = timeController;
        }
        
        
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
            _timeController.RequestTimeFreezeEffect(approveTime);
        }

        private void ApprovePunch()
        {
            if (!_approveRequested) return;
            _approveRequested = false;
            _timeController.RequestTimeUnfreezeEffect();
            PunchApproved?.Invoke();
        }
        

    }
}