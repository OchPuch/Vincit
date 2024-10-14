using Guns.General;
using UnityEngine;

namespace Guns.Projectiles.Types
{
    public class RockSolidHitscan : HitscanProjectile
    {
        public override void Init(Gun origin)
        {
            base.Init(origin);
        }

        public override void PunchCurve(Vector3 punchPoint, Vector3 forward)
        {
            Overload();
        }

        public override void ResetBullet()
        {
            base.ResetBullet();
            BulletCollider.isTrigger = true;
        }

        protected override void OnOverload()
        {
            BulletCollider.isTrigger = false;
        }
    }
}