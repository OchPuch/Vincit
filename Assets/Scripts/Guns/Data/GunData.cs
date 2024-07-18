using System;
using UnityEngine;

namespace Guns.Data
{
    [Serializable]
    public class GunData
    {
        [field: SerializeField] public GunConfig Config { get; private set; }
        
        public float fireTimer;
    }
}