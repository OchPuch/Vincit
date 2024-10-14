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

        public bool IsLoaded => _capsuleHolder.IsLoaded;
        
        public void Init(CapsuleHolder capsuleHolder)
        {
            _capsuleHolder = capsuleHolder;
            transform.up = transform.localPosition;
            _image.color = _capsuleHolder.IsLoaded ? _capsuleHolder.ProjectileConfig.DisplayColor : Color.black;
            capsuleHolder.Reloaded += OnReload;
            capsuleHolder.Shot += OnShot;
            capsuleHolder.Unloaded += OnUnload;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            _capsuleHolder.Reloaded -= OnReload;
            _capsuleHolder.Shot -= OnShot;
            _capsuleHolder.Unloaded -= OnUnload;
        }

        private void OnUnload()
        {
            _image.color = Color.black;
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