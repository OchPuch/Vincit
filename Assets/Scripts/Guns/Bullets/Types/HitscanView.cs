using UnityEngine;
using Utils;

namespace Guns.Bullets.Types
{
    public class HitscanView : GamePlayBehaviour
    {
        [SerializeField] private HitscanBullet bullet;
        [SerializeField] private SpriteRenderer[] lineRenderers;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color overloadedColor;


        protected override void Start()
        {
            base.Start();
            bullet.Overload += OnOverload;
            bullet.BulletDestroyed += OnBulletDestroyed;
        }

        private void OnBulletDestroyed()
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.color = normalColor;
            }
        }

        private void OnOverload()
        {
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.color = overloadedColor;
            }
        }
    }
}