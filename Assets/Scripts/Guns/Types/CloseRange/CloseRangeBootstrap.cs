using Guns.General;

namespace Guns.Types.CloseRange
{
    public class CloseRangeBootstrap : GunBootstrap
    {
        protected override void BindGun()
        {
            base.BindGun();
            Container.BindInterfacesAndSelfTo<CloseRange>().FromInstance(Gun);
        }
    }
}