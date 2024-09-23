using UnityEngine;

namespace Guns.Data
{
    [CreateAssetMenu(fileName = "New Gun Config", menuName = "Guns/Config")]
    public class GunConfig : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
        
        [field: SerializeField] public float HelicopterForce { get; private set; }
        
        [field: SerializeField] public float AddScaleAtStart { get; private set; }
        
        [field: SerializeField] public float TimeBeforeAbleToThrow { get; private set; }
    }
}