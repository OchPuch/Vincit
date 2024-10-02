using System;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment.Triggers
{
    public class GravityTrigger : MonoBehaviour
    {
        [SerializeField] private Vector3 gravity;
        [SerializeField] private GravityType gravityType;
        [SerializeField] private bool prioritizeIfOverlapping;

        [Button("Rotate gravity to Down")]
        private void RotateGravityDown()
        {
            gravity = -transform.up * gravity.magnitude;
        }

        [Button("Rotate gravity to Up")]
        private void RotateGravityUp()
        {
            gravity = transform.up * gravity.magnitude;
        }
        
        private void SetGravityForPlayer(PlayerController player)
        {
            switch (gravityType)
            {
                case GravityType.Center:
                    //Calculate the direction from the object to the center of the planet
                    var direction = (transform.position - player.transform.position).normalized;
                    player.SetGravity(direction * gravity.magnitude);
                    break;
                case GravityType.Custom:
                    player.SetGravity(gravity);
                    break;
            }
        }
        
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var obj = other.GetComponent<PlayerController>();
                if (obj)
                {
                    SetGravityForPlayer(obj);
                }
            }
        }
        

        private void OnTriggerStay(Collider other)
        {
            if (!prioritizeIfOverlapping || !other.CompareTag("Player")) return;
            var obj = other.GetComponent<PlayerController>();
            if (obj)
            {
                SetGravityForPlayer(obj);
            }
        }

        private enum GravityType
        {
            Center,
            Custom
        }
    }
}
