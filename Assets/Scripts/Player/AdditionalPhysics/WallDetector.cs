using Player.Data;
using Player.StateMachine;
using Player.States.DefaultState.Airborne;
using UnityEngine;
using Utils;

namespace Player.AdditionalPhysics
{
    public class WallDetector : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider nearCollider;
        private PlayerData _playerData;
        private int _objectCount;
  
        
        public void Init(PlayerData data, StateMachine.StateMachine stateMachine)
        {
            _playerData = data;
            WallCheck();
            stateMachine.StateEntered += OnStateEntered;
        }

        private void OnStateEntered(IState obj)
        {
            if (obj is DefaultAirborneState)
            {
                WallCheck();
            }
        }
        
        private void WallCheck()
        {
            //Overlap sphere same as collider and check if theres any level layer object
            _objectCount = 0;
            _playerData.isNearWall = false;
            _playerData.wallNormal = Vector3.zero;
            _playerData.lastWallJumpCollider = null;
            
            //Create overlapp sphere
            Collider[] colliders = new Collider[1];
            _objectCount = Physics.OverlapSphereNonAlloc(transform.position, nearCollider.radius, colliders, _playerData.levelLayerMask); 
            if (_objectCount > 0)
            {
                _playerData.isNearWall = true;
                _playerData.lastWallJumpCollider = colliders[0];
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _playerData.levelLayerMask))
            {
                _playerData.lastWallJumpCollider = other;
                _playerData.isNearWall = true;
                _objectCount++;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _playerData.levelLayerMask))
            {
                _objectCount--;
                if (_objectCount > 0) return;
                _playerData.isNearWall = false;
                _playerData.wallNormal = Vector3.zero;
                _objectCount = 0;
            }
        }
    }
}