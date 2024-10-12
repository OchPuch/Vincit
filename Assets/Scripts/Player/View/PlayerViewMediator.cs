using General;
using Player.Data;
using Player.StateMachine;
using Player.States.DefaultState.Special;
using UnityEngine;
using Zenject;

namespace Player.View
{
    public class PlayerViewMediator : GamePlayBehaviour
    {
        [SerializeField] private SpeedLinesView _dashLines;
        [SerializeField] private SpeedLinesView _groundSlamLines;
        [SerializeField] private SpeedLinesView _slideLines;

        
        private PlayerController _playerController;
        private PlayerData _playerData;

        private Vector3 PlayerVelocityDirection => _playerData.motor.Velocity.normalized;
        private Vector3 PlayerPosition => _playerData.motor.transform.position;
        private Vector3 PlayerUp => _playerData.motor.CharacterUp;
        
        [Inject]
        private void Construct(PlayerController playerController, PlayerData playerData)
        {
            _playerController = playerController;
            _playerData = playerData;
            playerController.StateMachine.StateEntered += OnStateEntered;
            playerController.StateMachine.StateExited += OnStateExited;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _playerController.StateMachine.StateEntered -= OnStateEntered;
            _playerController.StateMachine.StateExited -= OnStateExited;
        }

        private void OnStateExited(IState obj)
        {
            switch (obj)
            {
                case DefaultDashState:
                    _dashLines.Disable();
                    break;
                case DefaultSlamingState:
                    _groundSlamLines.Disable();
                    break;
                case DefaultSlideState:
                    _slideLines.Disable();
                    break;
            }
        }

        private void OnStateEntered(IState obj)
        {
            switch (obj)
            {
                case DefaultDashState:
                    if (PlayerVelocityDirection == Vector3.zero) break;
                    _dashLines.Activate(PlayerPosition, PlayerVelocityDirection);
                    break;
                case DefaultSlamingState:
                    _groundSlamLines.Activate(PlayerPosition, -PlayerUp);
                    break;
                case DefaultSlideState:
                    if (PlayerVelocityDirection == Vector3.zero) break;
                    _slideLines.Activate(PlayerPosition, PlayerVelocityDirection);
                    break;
            }
        }
        
        private void Update()
        {
            if (_playerController.StateMachine.IsInState<DefaultSlamingState>())
            {
                _groundSlamLines.UpdatePosition(PlayerPosition);
            }
            if (_playerController.StateMachine.IsInState<DefaultSlideState>())
            {
                _slideLines.UpdatePosition(PlayerPosition);
                _slideLines.UpdateUpDirection(PlayerUp);
                _slideLines.UpdateDirection(PlayerVelocityDirection);
            }
            else if (_playerController.StateMachine.IsInState<DefaultDashState>())
            {
                _dashLines.UpdatePosition(PlayerPosition);
                _dashLines.UpdateDirection(PlayerVelocityDirection);
            }
        }
    }
}