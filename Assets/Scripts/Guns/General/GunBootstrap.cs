using Guns.Data;
using Guns.View;
using UnityEngine;

namespace Guns.General
{
    public class GunBootstrap : MonoBehaviour
    {
        [Header("Bootstrap Settings")] [SerializeField]
        private bool disableAfterAwake;
        [SerializeField] private BoxCollider pickUpCollider;

        [Header("General components")] 
        [SerializeField] private GunData data;
        [SerializeField] private Gun gun;
        [SerializeField] private Transform viewTransform;

        [Header("View components")]
        [SerializeField] private GunAudio gunAudio;
        [SerializeField] private GunView view;

        private void Awake()
        {
            gun.Init(data);
            view.Init(gun, data);
            if (gunAudio) gunAudio.Init(gun, data);
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