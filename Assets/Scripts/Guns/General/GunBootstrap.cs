using Guns.Data;
using Guns.View;
using UnityEngine;

namespace Guns.General
{
    public class GunBootstrap : MonoBehaviour
    {
        [SerializeField] private BoxCollider pickUpCollider;
        [SerializeField] private GunData data;
        [SerializeField] private Gun gun;
        [SerializeField] private GunView view;
        [SerializeField] private bool disableAfterAwake;

        private void Awake()
        {
            gun.Init(data);
            view.Init(gun, data);
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