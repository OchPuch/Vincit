namespace Guns.Types.SpinThrowGun
{
    public interface ISpinReportListener
    {
        public void UpdateSpinState(SpinReport spinReport);
    }
    
    public struct SpinReport
    {
        public bool IsSpinning;
        public float SpinSpeed;
    }
}