using Guns.Projectiles;
using UnityEngine;

namespace Guns.View
{
    public class GunStateUI : GeneralGunView
    {
        [SerializeField] private RectTransform rootPanel;

        private void Awake()
        {
            rootPanel.gameObject.SetActive(false);
        }

        public override void OnGunEquip(Transform root)
        {
            rootPanel.gameObject.SetActive(true);
            rootPanel.SetParent(root);
            rootPanel.localRotation = Quaternion.identity;
            rootPanel.localPosition = Vector3.zero;
            rootPanel.localScale = Vector3.one;
            var max = rootPanel.offsetMax;
            max.x = 0;
            max.y = 0;
            rootPanel.offsetMax = max;
            var min = rootPanel.offsetMin;
            min.x = 0;
            min.y = 0;
            rootPanel.offsetMin = min;
        }

        public override void OnGunDeactivated()
        {
            enabled = false;
            rootPanel.gameObject.SetActive(false);
        }

        public override void OnGunActivated()
        {
            enabled = true;
            rootPanel.gameObject.SetActive(true);
        }

        public override void OnGunShot(ProjectileConfig projectileConfig)
        {
            
        }
    }
}