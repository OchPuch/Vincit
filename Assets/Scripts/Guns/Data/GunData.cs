using System;
using UnityEngine;

namespace Guns.Data
{
    [Serializable]
    public class GunData
    {
        [field: SerializeField] public GunConfig Config { get; private set; }
        [field: SerializeField] public SphereCollider GunPunchCollider {get; private set; }
        public float fireTimer;
        public float spinTimer;
    }
}