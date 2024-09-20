using UnityEngine;


namespace Guns.Bullets.Types
{
    public class KickSphere : CloseRangeBulletSphere
    {
        [SerializeField] private float seriousKickForceMultiplier = 0.2f;
        [SerializeField] private float mediumKickForceMultiplier = 0.09f;
        [SerializeField] private float smallKickForceMultiplier = 0.01f;

        private bool _kicked;

        public override void ResetBullet()
        {
            base.ResetBullet();
            _kicked = false;
        }

        protected override void CrushWall()
        {
            if (NeedApprove)
            {
                SeriousKick();
            }
            base.CrushWall();
        }

        protected override void CombineBullets()
        {
            base.CombineBullets();
            if (BulletsToCombine.Count > 1) 
                SeriousKick();
            else if (BulletsToCombine.Count > 0)
                MediumKick();
        }

        protected override void PostProcessRigidbody(Rigidbody rb)
        {
            base.PostProcessRigidbody(rb);
            if (!rb.isKinematic && TimeNotifier.IsTimeStopped)
            {
                MediumKick();
            }
        }

        protected override void PostProcessCollider(Collider hitCollider)
        {
            base.PostProcessCollider(hitCollider);
            if (!hitCollider.isTrigger)
            {
                SmallKick();
            }
        }

        private void SmallKick()
        {
            if (_kicked) return;
            Origin.Owner.RequestPush(-transform.forward * (Config.PushPower * smallKickForceMultiplier), ForceMode.Impulse);
            _kicked = true; 
        }
        
        private void MediumKick()
        {
            if (_kicked) return;
            Origin.Owner.RequestPush(-transform.forward * (Config.PushPower * mediumKickForceMultiplier), ForceMode.Impulse);
            _kicked = true; 
        }

        private void SeriousKick()
        {
            if (_kicked) return;
            Origin.Owner.RequestPush(-transform.forward * (Config.PushPower * seriousKickForceMultiplier), ForceMode.Impulse);
            _kicked = true;
        }
    }
}