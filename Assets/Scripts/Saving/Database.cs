using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Saving
{
    public class Database
    {
        private static string CheckPoint => Application.persistentDataPath + "/progress.data";
        
        public static void SaveCheckpoint(SaveData saveData)
        {
            var formatter = new BinaryFormatter();
            var file = System.IO.File.Create(CheckPoint);
            formatter.Serialize(file, saveData);
            file.Close();
        }
        
        public static SaveData? LoadCheckpoint()
        {
            if (System.IO.File.Exists(CheckPoint))
            {
                var formatter = new BinaryFormatter();
                var file = System.IO.File.Open(CheckPoint, System.IO.FileMode.Open);
                var data = (SaveData) formatter.Deserialize(file);
                file.Close();
                return data;
            }

            return null;
        }
        
        public static void DeleteCheckpoint()
        {
            if (System.IO.File.Exists(CheckPoint))
            {
                System.IO.File.Delete(CheckPoint);
            }
        }
    }
}