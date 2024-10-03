using Guns.Data;
using UnityEngine;
using Zenject;

namespace Guns.General
{
    public class GunBootstrap : MonoInstaller
    {
        [Header("Bootstrap Settings")] 
        [SerializeField] private bool disableAfterAwake;
        [SerializeField] private BoxCollider pickUpCollider;

        [Header("General components")] 
        [SerializeField] private GunData data;
        [SerializeField] private Gun gun;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Gun>().FromInstance(gun);
            Container.BindInterfacesAndSelfTo<GunData>().FromInstance(data);
        }

        private void Awake()
        {
            if (disableAfterAwake)
            {
                enabled = false;
                pickUpCollider.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<GunController>(out var gunController))
            {
                gunController.EquipGun(gun);
                enabled = false;
                pickUpCollider.enabled = false;
            }
        }
    }
}