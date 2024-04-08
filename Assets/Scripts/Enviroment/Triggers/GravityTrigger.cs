using NaughtyAttributes;
using UnityEngine;

namespace Enviroment.Triggers
{
    public class GravityTrigger : MonoBehaviour
    {
        [SerializeField] private Vector3 gravity;
        [SerializeField] private GravityType gravityType;

        [Button("Rotate gravity with object")]
        private void RotateGravity()
        {
            gravity = -transform.up * gravity.magnitude;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var obj = other.GetComponent<Player.CharacterController>();
                if (obj)
                {
                    switch (gravityType)
                    {
                        case GravityType.Center:
                            //Calculate the direction from the object to the center of the planet
                            var direction = (transform.position - other.transform.position).normalized;
                            obj.SetGravity(direction * gravity.magnitude);
                            break;
                        case GravityType.Custom:
                            obj.SetGravity(gravity);
                            break;
                    }
                }
            }
        }
    
        private enum GravityType
        {
            Center,
            Custom
        }
    }
}
