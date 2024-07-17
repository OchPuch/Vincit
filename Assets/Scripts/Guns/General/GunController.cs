using System.Collections.Generic;
using TimeStop;
using UnityEngine;
using Utils;

namespace Guns.General
{
    public struct GunInput
    {
        public bool AbilityRequest;
        public bool ShootRequest;
    }
    
    public class GunController : GamePlayBehaviour
    {
        [SerializeField] private Transform gunRoot;
        [SerializeField] private TimeStopAbility ability;
        private readonly List<Gun> _guns = new();
        private Gun _activeGun;
        private void Update()
        {
            var input = new GunInput
            {
                AbilityRequest = Input.GetKeyDown(KeyCode.Q),
                ShootRequest = Input.GetMouseButton(0)
            };
            
            if (input.AbilityRequest) ability.Activate();
            if (!_activeGun) return;
            _activeGun.HandleInput(input);
        }

        public void EquipGun(Gun gun)
        {
            _guns.Add(gun);
            if (_activeGun != null)
            {
                _activeGun.Deactivate();
            }
            _activeGun = gun;
            gun.Equip();
            _activeGun.Activate();
            gun.transform.parent.SetParent(gunRoot);
        }
    }
}
