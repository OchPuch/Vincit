using System;
using Guns.Data;
using UnityEngine;
using Utils;

namespace Guns.General
{
    public abstract class Gun : GamePlayBehaviour
    {
        protected GunData Data;
        public event Action Shot;
        public event Action Equipped;
        public event Action Activated;
        public event Action Deactivated;
        
        public void Init(GunData data)
        {
            Data = data;
        }

        private void Update()
        {
            Data.fireTimer += Time.deltaTime;
        }

        public void HandleInput(GunInput input)
        {
            if (input.ShootRequest && Data.fireTimer > Data.Config.FireRate)
            {
                Shoot();
                Shot?.Invoke();
                Data.fireTimer = 0;
            }
        }

        public void Equip()
        {
            Equipped?.Invoke();
        }

        public void Deactivate()
        {
            Deactivated?.Invoke();
        }
        
        public void Activate()
        {
            Activated?.Invoke();
        }
        
        protected abstract void Shoot();
    }
}