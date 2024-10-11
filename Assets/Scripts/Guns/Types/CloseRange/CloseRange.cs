using System;
using System.Collections;
using General.GlobalManagers;
using Guns.General;
using UnityEngine;
using Zenject;

namespace Guns.Types.CloseRange
{
    public class CloseRange : Gun
    {
        private const float NotFreezeTime = 0.05f;
        private float _currentApproveTime;
        private bool _approveRequested;
        private float _approveTimer;

        private float _approveForce;
        private TimeController _timeController;
        private Camera _camera;
        
        public event Action<float> PunchApproved;
        
        [Inject]
        public void Construct(TimeController timeController)
        {
            _timeController = timeController;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        protected override void OnShot()
        {
            _approveTimer = 0;
            _approveRequested = false;
            base.OnShot();
        }

        protected override void Update()
        {
            base.Update();
            if (_approveRequested)
            {
                _approveTimer += Time.unscaledDeltaTime;
                if (_approveTimer > _currentApproveTime + NotFreezeTime)
                {
                    ApprovePunch();
                }
            }
        }
        
        public void RequestApprove(float approveTime, float approveForce)
        {
            if (_approveRequested) return;
            _approveForce = approveForce;
            _approveRequested = true;
            _currentApproveTime = approveTime;
            StartCoroutine(ApproveRoutine(approveTime));
        }

        private IEnumerator ApproveRoutine(float approveTime)
        {
            yield return new WaitForSecondsRealtime(NotFreezeTime);
            _timeController.RequestTimeFreezeEffect(approveTime, 0.01f);
        }

        private void ApprovePunch()
        {
            if (!_approveRequested) return;
            _approveRequested = false;
            _timeController.RequestTimeUnfreezeEffect();
            PunchApproved?.Invoke(_approveForce);
        }

        
            
        

    }
}