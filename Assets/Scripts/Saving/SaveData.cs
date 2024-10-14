using System;


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
        }
    }
}