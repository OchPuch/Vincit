using System;
using UnityEngine;

namespace Saving
{
    [Serializable]
    public struct SaveData
    {
        public string SceneName;
        public CharacterSaveData PlayerSaveData;
        
        [Serializable]
        public struct CharacterSaveData
        {
            public float PositionX;
            public float PositionY;
            public float PositionZ;

            public float GravityX;
            public float GravityY;
            public float GravityZ;
        }
    }
}