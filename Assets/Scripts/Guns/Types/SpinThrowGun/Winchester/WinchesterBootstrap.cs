using Guns.General;

namespace Guns.Types.SpinThrowGun.Winchester
{
    public class WinchesterBootstrap : GunBootstrap
    {
        protected override void BindGun()
        {
            base.BindGun();
            Container.BindInterfacesAndSelfTo<SpinThrowGun>().FromInstance(Gun);
            Container.BindInterfacesAndSelfTo<Winchester>().FromInstance(Gun);
        }
    }
}