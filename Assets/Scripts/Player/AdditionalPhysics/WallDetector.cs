using System;
using UnityEngine;
using Utils;

namespace Player.AdditionalPhysics
{
    public class WallDetector : MonoBehaviour
    {
        [SerializeField]
        private LayerMask levelLayerMask;
        
        private PlayerData _playerData;
        
        private int _objectCount;
  
        
        public void Init(PlayerData data)
        {
            _playerData = data;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, levelLayerMask))
            {
                _objectCount++;
                _playerData.isNearWall = true;
                
                //hit the wall with raycast to get the normal
                RaycastHit hit;
                Vector3 closestPoint = other.ClosestPointOnBounds(transform.position);
                Vector3 direction = closestPoint - transform.position;
                if (Physics.Raycast(transform.position, direction, out hit, 1f + direction.magnitude, levelLayerMask))
                {
                    _playerData.wallNormal = hit.normal;
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, levelLayerMask))
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