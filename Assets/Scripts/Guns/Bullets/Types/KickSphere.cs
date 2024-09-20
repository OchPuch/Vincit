using UnityEngine;


namespace Guns.Bullets.Types
{
    public class KickSphere : CloseRangeBulletSphere
    {
        protected override void CrushWall()
        {
            base.CrushWall();
            if (NeedApprove)
            {
                Origin.Owner.RequestPush(-transform.forward * Config.PushPower/5f, ForceMode.Impulse);
            }
        }

        protected override void CombineBullets()
        {
            base.CombineBullets();
            if (BulletsToCombine.Count > 0) 
                Origin.Owner.RequestPush(-transform.forward * Config.PushPower/5f, ForceMode.Impulse);
        }
    }
}