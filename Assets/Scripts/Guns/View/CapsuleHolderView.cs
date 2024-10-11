using General;
using Guns.General;
using Guns.Projectiles;
using UnityEngine;
using UnityEngine.UI;

namespace Guns.View
{
    public class CapsuleHolderView : GamePlayBehaviour
    {
        [SerializeField] private Image _image;
        private CapsuleHolder _capsuleHolder;
        
        public void Init(CapsuleHolder capsuleHolder)
        {
            _capsuleHolder = capsuleHolder;
            _image.color = _capsuleHolder.IsLoaded ? _capsuleHolder.ProjectileConfig.DisplayColor : Color.black;
            capsuleHolder.Reloaded += OnReload;
            capsuleHolder.Shot += OnShot;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _capsuleHolder.Reloaded -= OnReload;
            _capsuleHolder.Shot -= OnShot;
        }
        
        private void OnShot()
        {
            _image.color = Color.black;
        }
        

        private void OnReload(ProjectileConfig obj)
        {
            _image.color = obj.DisplayColor;
        }
    }
}