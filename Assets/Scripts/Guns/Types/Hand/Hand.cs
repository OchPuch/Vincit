using Guns.General;

namespace Guns.Types.Hand
{
    public class Hand : Gun
    {
        public override void HandleInput(GunInput input)
        {
            if (input.HandPunchRequest && Data.fireTimer > Data.Config.FireRate)
            {
                Shoot();
                InvokeShot();
                Data.fireTimer = 0;
            }
        }

        protected override void Shoot()
        {
            
        }
    }
}