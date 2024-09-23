using Sirenix.OdinInspector;
using UnityEngine;

namespace Guns.Projectiles
{
    [CreateAssetMenu(menuName = "Guns/Bullet Config", fileName = "New Bullet Config")]
    public class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask BulletHitMask  { get; private set; }
        [field: SerializeField] public LayerMask BulletStopMask { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float DestroyTime { get; private set; }
        [field: SerializeField] public float StartRadius { get; private set; }
        [field: SerializeField] public float PushPower { get; private set; } = 10f;
        [field: SerializeField] public float MaxDistance { get; private set; } = 100f;
        [field: SerializeField] public AnimationCurve DisappearAnimation { get; private set; }
        [field: SerializeField] public string FactoryId { get; private set; }

        [Button("Generate Factory Id")]
        private void GenerateFactoryId()
        {
            var id = $"{name}_Factory_#{Random.Range(0, 9999):0000}";
            id = id.Replace("Config", "");
            id = id.Replace(" ", "_");
            FactoryId = id;
        }
    }
}