using General;
using TimeStop;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Player.View
{
    public class PlayerStateUI : GamePlayBehaviour
    {
        [SerializeField] private Slider _chargeBar;
        [SerializeField] private RectTransform renderTransform;
        [SerializeField] private float maxOffsetBySpeed;
        [SerializeField] private float offsetBySpeedMultiplier;
        [SerializeField] private float smoothTime = 0.3f; 

        private Vector3 _velocity = Vector3.zero; 
        private Vector3 _startRenderPosition;
        private Player _player;
        private TimeStopAbility _timeStopAbility;

        [Inject]
        private void Construct(Player player, TimeStopAbility timeStopAbility)
        {
            _player = player;
            _timeStopAbility = timeStopAbility;
            _startRenderPosition = renderTransform.localPosition;
        }
        
        private void Update()
        {
            _chargeBar.value = _timeStopAbility.GetCharge01();
            
            var playerSpeedAdd = - _player.Data.motor.Velocity.normalized *
                Mathf.Clamp(_player.Data.motor.Velocity.magnitude * offsetBySpeedMultiplier, 0, maxOffsetBySpeed);

            var newTargetPosition = _startRenderPosition;
            var rotatedVector = _player.Data.cameraFollowPoint.InverseTransformVector(playerSpeedAdd);
             newTargetPosition += rotatedVector;
            
            renderTransform.localPosition = Vector3.SmoothDamp(
                renderTransform.localPosition, 
                newTargetPosition, 
                ref _velocity, 
                smoothTime
            );
            
        }
    }
}