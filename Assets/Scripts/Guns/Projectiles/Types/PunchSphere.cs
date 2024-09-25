using Guns.Projectiles.Interactions;
using UnityEngine;

namespace Guns.Projectiles.Types
{
    public class PunchSphere : CloseRangeProjectileSphere
    {
        protected override void PostProcessCollider(Collider hitCollider)
        {
            if (hitCollider.TryGetComponent(out IPunchable punchable))
            {
                punchable.Punch(Origin.transform.forward * Config.PushPower);
                return;
            }
            base.PostProcessCollider(hitCollider);
        }
        
    }
}