using System;
using UnityEngine;

namespace GlobalManagers
{
    [Serializable]
    public class GameManager
    {
        [field: SerializeField] public GameSettings GameSettings { get; private set; }
    }
}