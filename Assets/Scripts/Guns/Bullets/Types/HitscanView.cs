using General;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Guns.Bullets.Types
{
    public class HitscanView : GamePlayBehaviour
    {
        [FormerlySerializedAs("bullet")] [SerializeField] private HitscanProjectile projectile;
        [SerializeField] private SpriteRenderer[] lineRenderers;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color overloadedColor;


        protected override void Start()
        {
            base.Start();
            projectile.Overloaded += OnOverloaded;
            projectile.BulletDestroyed += OnProjectileDestroyed;
            if (projectile.IsOverloaded)
            {
                OnOverloaded();
            }
        }

        private void OnProjectileDestroyed()
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.color = normalColor;
            }
        }

        private void OnOverloaded()
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.color = overloadedColor;
            }
        }
    }
}