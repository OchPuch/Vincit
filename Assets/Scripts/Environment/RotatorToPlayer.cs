using UnityEngine;
using Utils;

namespace Environment
{
    public class RotatorToPlayer : GamePlayBehaviour
    {
        void LateUpdate()
        {
            var target = Player.Player.Instance.transform;

            Vector3 targetPosition = target.position;
            Vector3 currentPosition = transform.position;

            Vector3 direction = targetPosition - currentPosition;
            
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(-rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            
        }
    }
}
