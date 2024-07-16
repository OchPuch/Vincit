using Guns.Data;
using Guns.View;
using UnityEngine;

namespace Guns.General
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