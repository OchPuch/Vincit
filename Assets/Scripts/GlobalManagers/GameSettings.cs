using UnityEngine;

namespace GlobalManagers
{
    [CreateAssetMenu(menuName = "Game Settings", fileName = "New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [field: Header("Bullets")]
        [field: SerializeField] public LayerMask BulletHitMask  { get; private set; }
        [field: SerializeField] public LayerMask BulletStopMask { get; private set; }
        [field: SerializeField] public float MaxShootingDistance { get; private set; }
    }
}