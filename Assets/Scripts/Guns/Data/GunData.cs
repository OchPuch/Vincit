using System;
using System.Collections.Generic;
using Guns.General;
using Guns.Projectiles;
using Guns.Types.SpinThrowGun.Revolver;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Guns.Data
{
    [Serializable]
    public class GunData
    {
        [field: SerializeField] public GunConfig Config { get; private set; }
        [field: SerializeField] public Transform ViewTransform { get; private set; }
        [field: SerializeField] public SphereCollider GunPunchCollider {get; private set; }

        [field: SerializeField] public List<ProjectileConfig> AvailableProjectiles { get; private set; }
        public List<CapsuleHolder> CapsuleHolders { get; private set; } = new();
        
        public float FireTimer;
        public float CurrentSpinSpeed;
        
    }
}