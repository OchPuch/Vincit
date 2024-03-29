using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 gravity;
    [SerializeField] private GravityType gravityType;
    
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
