using Guns.Data;
using Guns.View;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Guns.General
{
    public class GunBootstrap : SerializedMonoBehaviour
    {
        [Header("Bootstrap Settings")] [SerializeField]
        private bool disableAfterAwake;
        [SerializeField] private BoxCollider pickUpCollider;

        [Header("General components")] 
        [SerializeField] private GunData data;
        [SerializeField] private Gun gun;

        [Header("View components")]
        [SerializeField] private GunAudio gunAudio;
        [SerializeField] private GunView view;
        [SerializeField] private GunStateUI gunStateUI;
        
        private void Awake()
        {
            gun.Init(data);
            view.Init(gun, data);
            if (gunStateUI) gunStateUI.Init(gun, data);
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