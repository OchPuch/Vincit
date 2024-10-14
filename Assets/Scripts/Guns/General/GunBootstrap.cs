using Guns.Data;
using UnityEngine;
using Zenject;

namespace Guns.General
{
    public class GunBootstrap : MonoInstaller
    {
        [Header("Bootstrap Settings")] 
        [SerializeField] private bool _disableAfterAwake;
        [SerializeField] private BoxCollider _pickUpCollider;

        [Header("General components")] 
        [SerializeField] private GunData _data;
        [SerializeField] protected Gun Gun;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GunConfig>().FromInstance(_data.Config);
            Container.BindInterfacesAndSelfTo<GunData>().FromInstance(_data);
            BindGun();
        }

        protected virtual void BindGun()
        {
            Container.BindInterfacesAndSelfTo<Gun>().FromInstance(Gun);
        }

        private void Awake()
        {
            if (_disableAfterAwake)
            {
                enabled = false;
                _pickUpCollider.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<GunController>(out var gunController))
            {
                gunController.EquipGun(Gun);
                enabled = false;
                _pickUpCollider.enabled = false;
            }
        }
    }
}