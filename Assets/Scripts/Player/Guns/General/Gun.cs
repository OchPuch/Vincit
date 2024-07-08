using System;
using Player.Guns.Data;
using UnityEngine;

namespace Player.Guns.General
{
    public abstract class Gun : MonoBehaviour
    {
        protected GunData Data;
        public event Action Shot;
        
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
    }
}