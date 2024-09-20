using System.Collections.Generic;
using General;
using General.GlobalManagers;
using Guns.Types.CloseRange;
using TimeStop;
using UnityEngine;
using Utils;
using Zenject;

namespace Guns.General
{
    public struct GunInput
    {
        public bool AbilityRequest;
        public bool ShootRequest;
        public bool HandPunchRequest;
        public bool LegPunchRequest;
    }
    
    public class GunController : GamePlayBehaviour
    {
        [SerializeField] private Transform gunRoot;
        [SerializeField] private TimeStopAbility ability;
        [SerializeField] private CloseRange leftHand;
        [SerializeField] private CloseRange leg;

        private Player.Player _owner;
        private readonly List<Gun> _guns = new();
        private Gun _activeGun;

        [Inject]
        private void Construct(TimeController timeController, ITimeNotifier timeNotifier)
        {
            ability.Init(timeController,timeNotifier);
        }
        
        protected override void Start()
        {
            base.Start();
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
            var input = new GunInput
            {
                AbilityRequest = Input.GetKeyDown(KeyCode.Q),
                ShootRequest = Input.GetMouseButton(0),
                HandPunchRequest = Input.GetMouseButtonDown(4),
                LegPunchRequest = Input.GetKeyDown(KeyCode.F)
            };
            
            if (input.AbilityRequest) ability.SwitchActive();
            if (_activeGun && input.ShootRequest) _activeGun.Shoot();
            if (input.HandPunchRequest) leftHand.Shoot();
            if (input.LegPunchRequest) leg.Shoot();
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
        }

        public void Init(Player.Player player)
        {
            _owner = player;
        }
    }
}
