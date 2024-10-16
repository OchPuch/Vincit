﻿using Guns.Projectiles.Interactions;
using RayFire;
using UnityEngine;

namespace Guns.Projectiles.Types
{
    public class KickSphere : CloseRangeProjectileSphere
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

        protected override void CrushWall(float f)
        {
            if (NeedApprove)
            {
                SeriousKick();
            }

            base.CrushWall(f);
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
            if (hitCollider.TryGetComponent(out IKickable kickable))
            {
                kickable.Kick();
            }

            if (!hitCollider.isTrigger)
            {
                SmallKick();
            }
        }

        protected override void OnPreProcessRayFireRigid(Collider hitCollider, RayfireRigid rayfireRigid)
        {
            base.OnPreProcessRayFireRigid(hitCollider, rayfireRigid);

            if (rayfireRigid.limitations.currentDepth == 0)
            {
                NeedApprove = true;
            }
        }


        private void SmallKick()
        {
            if (_kicked) return;
            Origin.Owner.RequestPush(-transform.forward * (Config.PushPower * smallKickForceMultiplier),
                ForceMode.Impulse);
            _kicked = true;
        }

        private void MediumKick()
        {
            if (_kicked) return;
            Origin.Owner.RequestPush(-transform.forward * (Config.PushPower * mediumKickForceMultiplier),
                ForceMode.Impulse);
            _kicked = true;
        }

        private void SeriousKick()
        {
            if (_kicked) return;
            Origin.Owner.RequestPush(-transform.forward * (Config.PushPower * seriousKickForceMultiplier),
                ForceMode.Impulse);
            _kicked = true;
        }
    }
}