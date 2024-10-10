using System.Collections.Generic;
using General;
using General.GlobalManagers;
using Guns.Interfaces.Spin;
using Guns.Types.CloseRange;
using Guns.Types.SpinThrowGun;
using TimeStop;
using UnityEngine;
using Zenject;

namespace Guns.General
{
    public struct GunInput
    {
        public bool AbilityRequest;
        public bool ShootRequest;
        public bool ReloadRequest;
        public bool HandPunchRequest;
        public bool LegPunchRequest;
        public bool StartSpinRequest;
        public bool EndSpinRequest;
    }

    public class GunSwitchInput
    {
        public bool PreciseSwitch { get; private set; }
        public int PreciseSwitchIndex { get; private set; }
        public bool ScrollGunUp { get; private set; }
        public bool ScrollGunDown { get; private set; }

        public bool SwitchRequested()
        {
            return PreciseSwitch || ScrollGunDown || ScrollGunUp;
        }

        private void UpdatePreciseSwitchIndex()
        {
            PreciseSwitch = false;
            //Update index by current number keys input
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    PreciseSwitchIndex = i;
                    PreciseSwitch = true;
                }
            }
        }

        public void Update()
        {
            ScrollGunUp = Input.mouseScrollDelta.y > 0;
            ScrollGunDown = Input.mouseScrollDelta.y < 0;
            UpdatePreciseSwitchIndex();
        }
    }

    public class GunController : GamePlayBehaviour
    {
        [SerializeField] private Transform gunRoot;
        [SerializeField] private TimeStopAbility ability;
        [SerializeField] private CloseRange leftHand;
        [SerializeField] private CloseRange leg;

        private readonly GunSwitchInput _gunSwitchInput = new();
        private Player.Player _owner;
        private readonly List<Gun> _guns = new();
        private Gun _activeGun;
        private Gun _lastGun;
        
        private List<Gun> HiddenGuns => _guns.FindAll(g => !g.IsActive);

        [Inject]
        private void Construct(TimeController timeController, ITimeNotifier timeNotifier, Player.Player player)
        {
            ability.Init(timeController, timeNotifier);
            _owner = player;
            InitHand();
            InitLeg();
        }

        private void InitHand()
        {
            leftHand.Equip(_owner);
            leftHand.Activate();
            leftHand.transform.parent.SetParent(gunRoot);
            leftHand.transform.parent.localPosition = Vector3.zero;
            leftHand.transform.parent.forward = gunRoot.forward;
            leftHand.transform.forward = gunRoot.forward;
        }

        private void InitLeg()
        {
            leg.Equip(_owner);
            leg.Activate();
            leg.transform.parent.SetParent(gunRoot);
            leg.transform.parent.localPosition = Vector3.zero;
            leg.transform.parent.forward = gunRoot.forward;
            leg.transform.forward = gunRoot.forward;
        }

        private void Update()
        {
            _gunSwitchInput.Update();

            if (_gunSwitchInput.SwitchRequested())
            {
                if (_gunSwitchInput.PreciseSwitch) SwitchGun(_gunSwitchInput.PreciseSwitchIndex);
                else if (_gunSwitchInput.ScrollGunUp) ScrollGun(true);
                else if (_gunSwitchInput.ScrollGunDown) ScrollGun(false);
            }

            var input = new GunInput
            {
                AbilityRequest = Input.GetKeyDown(KeyCode.Q),
                ShootRequest = Input.GetMouseButton(0),
                ReloadRequest = Input.GetKeyDown(KeyCode.R),
                HandPunchRequest = Input.GetMouseButtonDown(4),
                LegPunchRequest = Input.GetKeyDown(KeyCode.F),
                StartSpinRequest =  Input.GetMouseButtonDown(1),
                EndSpinRequest = !Input.GetMouseButton(1)
            };

            if (input.AbilityRequest) ability.SwitchActive();
            if (_activeGun && input.ShootRequest) _activeGun.Shoot();
            if (_activeGun && input.ReloadRequest) _activeGun.Reload();
            if (input.HandPunchRequest) leftHand.Shoot();
            if (input.LegPunchRequest) leg.Shoot();
            if (_activeGun is ISpinnableGun spinnableGun)
            {
                if (input.StartSpinRequest  ) spinnableGun.StartSpin();
                if (input.EndSpinRequest) spinnableGun.EndSpin();
            }
            
        }

        private void ScrollGun(bool up)
        {
            if (_guns.Count <= 0) return;
            int nextIndex;
            if (_activeGun) nextIndex = (_guns.IndexOf(_activeGun));
            else if (_lastGun) nextIndex = (_guns.IndexOf(_lastGun));
            else nextIndex = 0;
            nextIndex = (nextIndex + _guns.Count + (up ? 1 : -1)) % _guns.Count;
            if (nextIndex < 0) nextIndex = _guns.Count - 1;
            SwitchGun(nextIndex);
        }

        private void SwitchGun(int slotIndex)
        {
            if (_guns.Count <= 0) return;
            if (slotIndex < 0 || slotIndex >= _guns.Count) return;
            var wantedGun = _guns[slotIndex];
            if (wantedGun is null) return;
            if (wantedGun == _activeGun)
            {
                return;
            }

            if (HiddenGuns.Contains(wantedGun)) SwitchToGun(_guns[slotIndex]);
        }
        
        private void SwitchToGun(Gun gun)
        {
            if (gun is null) return;
            if (_activeGun)
            {
                if (_activeGun == gun) return;
                _activeGun.Deactivate();
                _lastGun = _activeGun;
            }

            _activeGun = gun;
            gun.Activate();
        }

        public void EquipGun(Gun gun)
        {
            _guns.Add(gun);
            if (_activeGun != null)
            {
                _activeGun.Deactivate();
            }

            _activeGun = gun;
            gun.Equip(_owner);
            _activeGun.Activate();

            gun.transform.parent.SetParent(gunRoot);
            gun.transform.parent.localPosition = Vector3.zero;
            gun.transform.localPosition = Vector3.zero;
            gun.transform.parent.forward = gunRoot.forward;
            gun.transform.forward = gunRoot.forward;
            
            gun.Data.ViewTransform.SetParent(gunRoot);
            gun.Data.ViewTransform.localPosition = Vector3.zero;
            
        }

        
    }
}