using System;
using Guns.Data;
using UnityEngine;
using Utils;

namespace Guns.General
{
    public abstract class Gun : GamePlayBehaviour
    {
        [SerializeField] private BoxCollider pickupCollider;
        protected GunData Data;
        public event Action Shot;
        public event Action Equip;
        public event Action Drop;
        public event Action Activate;
        public event Action Deactivate;
        
        public void Init(GunData data)
        {
            Data = data;
        }
        
        public void HandleInput(GunInput input)
        {
            if (input.ShootRequest)
            {
                Shot?.Invoke();
            }
        }
        protected abstract void Shoot();

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}