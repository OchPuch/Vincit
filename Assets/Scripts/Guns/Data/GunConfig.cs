﻿using UnityEngine;

namespace Guns.Data
{
    [CreateAssetMenu(fileName = "New Gun Config", menuName = "Guns/Config")]
    public class GunConfig : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: Min(1)][field: SerializeField] public int MagSize { get; private set; } = 6;
        [field: SerializeField] public float FireRate { get; private set; }
        [field: SerializeField] public float ReloadTIme { get; private set; } = 3f;
        
        [field: SerializeField] public float SpinFireSpeedAdd { get; private set; }
        [field: SerializeField] public float AddScaleAtStart { get; private set; }
        [field: SerializeField] public float HelicopterForce { get; private set; }
        [field: SerializeField] public float SpinMaxSpeed { get; private set; }
        [field: SerializeField] public float SpinAcceleration { get; private set; }
        [field: SerializeField] public float SpinDeacceleration { get; private set; }

    }
}