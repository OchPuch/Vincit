using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace Guns.Bullets
{
    public class BulletInstaller : MonoInstaller
    {
        [SerializeField] private List<Bullet> bulletPrefabs;
        
        public override void InstallBindings()
        {
            foreach (var bulletPrefab in bulletPrefabs)
            {
                //TODO: KYS
                // var bulletType = bulletPrefab.GetType();
                // var factoryType = typeof(BulletFactory<>).MakeGenericType(bulletType);
                //
                // // Bind the bullet type to the prefab instance
                // Container.Bind(bulletType).FromComponentInNewPrefab(bulletPrefab).AsTransient();
                //
                // // Bind the factory type using reflection
                // var factoryBindingMethod = typeof(DiContainer).GetMethod("BindFactory")?.MakeGenericMethod(bulletType, factoryType);
                // if (factoryBindingMethod != null) factoryBindingMethod.Invoke(Container, null);
            }
        }
    }
}