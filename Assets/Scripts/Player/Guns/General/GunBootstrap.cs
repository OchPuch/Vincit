using Player.Guns.Data;
using Player.Guns.View;
using UnityEngine;

namespace Player.Guns.General
{
    public class GunBootstrap : MonoBehaviour
    {
        [SerializeField] private GunData data;
        [SerializeField] private Gun gun;
        [SerializeField] private GunView view;

        private void Awake()
        {
            gun.Init(data);
            view.Init(gun, data);
        }
    }
}