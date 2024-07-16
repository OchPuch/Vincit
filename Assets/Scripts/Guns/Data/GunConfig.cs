using UnityEngine;

namespace Guns.Data
{
    [CreateAssetMenu(fileName = "New Gun Config", menuName = "Guns/Config")]
    public class GunConfig : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
        [field: SerializeField] public float PushPower { get; private set; }
    }
}