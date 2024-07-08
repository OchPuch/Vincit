using Player.Data;
using Player.StateMachine;
using Player.States.DefaultState.Airborne;
using UnityEngine;
using Utils;

namespace Player.AdditionalPhysics
{
    public class WallDetector : MonoBehaviour
    {
        [SerializeField] private BoxCollider nearCollider;
        private PlayerData _playerData;
        private int _objectCount;
  
        
        public void Init(PlayerData data, StateMachine.StateMachine stateMachine)
        {
            _playerData = data;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _playerData.levelLayerMask))
            {
                _playerData.lastWallJumpCollider = other;
                _playerData.isNearWall = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, _playerData.levelLayerMask))
            {
                _playerData.isNearWall = false;
                _playerData.wallNormal = Vector3.zero;
                _playerData.lastWallJumpCollider = null;
            }
        }
    }
}