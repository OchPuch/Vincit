using Guns.View;
using PrimeTween;
using UnityEngine;

namespace Guns.Types.CloseRange
{
    public class CloseRangeView : GunAnimationView
    {
        [SerializeField] private float _approvePunchForce;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void OnPunchApproved(float force)
        {
            Tween.ShakeCamera(_camera, _approvePunchForce * force);

        }
    }
}