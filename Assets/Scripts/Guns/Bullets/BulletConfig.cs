using UnityEngine;

namespace Guns.Bullets
{
    [CreateAssetMenu(menuName = "Guns/Bullet Config", fileName = "New Bullet Config")]
    public class BulletConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask BulletHitMask  { get; private set; }
        [field: SerializeField] public LayerMask BulletStopMask { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float DestroyTime { get; private set; }
        [field: SerializeField] public float StartRadius { get; private set; }
        [field: SerializeField] public float PushPower { get; private set; } = 10f;
        [field: SerializeField] public float MaxDistance { get; private set; } = 100f;
        [field: SerializeField] public AnimationCurve DisappearAnimation { get; private set; }
    }
}