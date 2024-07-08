using System;
using UnityEngine;

namespace Player.Guns.Data
{
    [Serializable]
    public class GunData
    {
        [field: SerializeField] public GunConfig Config { get; private set; }
    }
}