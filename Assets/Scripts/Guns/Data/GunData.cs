using System;
using System.Collections.Generic;
using Guns.Types.Revolver;
using Guns.Types.SpinThrowGun.Revolver;
using UnityEngine;

namespace Guns.Data
{
    [Serializable]
    public class GunData
    {
        [field: SerializeField] public Transform ViewTransform { get; private set; }
        [field: SerializeField] public GunConfig Config { get; private set; }
        [field: SerializeField] public SphereCollider GunPunchCollider {get; private set; }
        public List<CapsuleHolder> CapsuleHolders { get; private set; } = new();
        
        public float fireTimer;
        public float currentSpinSpeed;
    }
}