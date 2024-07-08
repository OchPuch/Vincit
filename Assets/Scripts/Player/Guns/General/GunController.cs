using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Guns.General
{
    public struct GunInput
    {
        public bool ShootRequest;
    }
    
    public class GunController : MonoBehaviour
    {
        private List<Gun> _guns;
        private Gun _activeGun;
        private void Update()
        {
            var input = new GunInput
            {
                ShootRequest = Input.GetMouseButton(0)
            };
            
            if (!_activeGun) return;
            _activeGun.HandleInput(input);
        }
    }
}
