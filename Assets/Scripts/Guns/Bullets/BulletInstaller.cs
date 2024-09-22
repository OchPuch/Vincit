using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Guns.Bullets
{
    public class BulletInstaller : MonoInstaller
    {
        [SerializeField] private List<Projectile> bulletPrefabs;
        
        public override void InstallBindings()
        {
            foreach (var bulletPrefab in bulletPrefabs)
            {
                // Bind the factory and pass the bulletPrefab as an argument
                Container.BindFactory<Projectile, ProjectileFactory>()
                    .WithId(bulletPrefab.Config.FactoryId) // Use the unique ID for resolution later
                    .WithFactoryArguments(bulletPrefab); // Pass the prefab as an argument to the factory
                
            }
        }
    }
}