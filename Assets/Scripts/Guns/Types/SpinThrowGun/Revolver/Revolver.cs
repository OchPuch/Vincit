using System;
using System.Linq;
using Guns.General;
using Guns.Projectiles;
using Random = UnityEngine.Random;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class Revolver : SpinThrowGun
    {
        protected override ProjectileConfig OnShot()
        {
            try
            {
                var loadedHolders = Data.CapsuleHolders.Where(x => x.IsLoaded).ToList();
                if (loadedHolders.Count <= 0)
                {
                    Reload();
                    return null;   
                }
                CapsuleHolder capsuleHolder = IsSpinning ? loadedHolders[Random.Range(0, loadedHolders.Count)] : loadedHolders[0];
                var bullet = capsuleHolder.Shoot(transform.position, transform.forward);
                if (bullet is null)
                {
                    Reload();
                    return null;
                }
                bullet.Init(this);
                return bullet.Config;

            }
            catch (Exception)
            {
                Reload();
                return null;
            }
        }
    }
}