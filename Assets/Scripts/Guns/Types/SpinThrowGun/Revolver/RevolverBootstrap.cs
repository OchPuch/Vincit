using Guns.General;

namespace Guns.Types.SpinThrowGun.Revolver
{
    public class RevolverBootstrap : GunBootstrap
    {
        protected override void BindGun()
        {
            base.BindGun();
            Container.BindInterfacesAndSelfTo<SpinThrowGun>().FromInstance(Gun);
            Container.BindInterfacesAndSelfTo<Revolver>().FromInstance(Gun);
        }
    }
}