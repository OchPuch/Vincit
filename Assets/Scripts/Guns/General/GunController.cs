using System.Collections.Generic;
using Guns.Types.Hand;
using TimeStop;
using UnityEngine;
using Utils;

namespace Guns.General
{
    public struct GunInput
    {
        public bool AbilityRequest;
        public bool ShootRequest;
        public bool HandPunchRequest;
    }
    
    public class GunController : GamePlayBehaviour
    {
        [SerializeField] private Transform gunRoot;
        [SerializeField] private TimeStopAbility ability;
        [SerializeField] private CloseRange leftHand;

        private Player.Player _owner;
        private readonly List<Gun> _guns = new();
        private Gun _activeGun;

        protected override void Start()
        {
            base.Start();
            InitHand();
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

        private void Update()
        {
            var input = new GunInput
            {
                AbilityRequest = Input.GetKeyDown(KeyCode.Q),
                ShootRequest = Input.GetMouseButton(0),
                HandPunchRequest = Input.GetMouseButtonDown(4),
            };
            
            if (input.AbilityRequest) ability.SwitchActive();
            if (_activeGun) _activeGun.HandleInput(input);
            leftHand.HandleInput(input);
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
