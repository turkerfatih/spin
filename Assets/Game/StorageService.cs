using System.IO;
using UnityEngine;

namespace Game
{
    public class StorageService
    {
        private static readonly string savePath = Application.persistentDataPath + "/game.rd";
        
        public static void SaveRun(RunData state)
        {
            string json = JsonUtility.ToJson(state, prettyPrint: true);
            File.WriteAllText(savePath, json);
            Debug.Log("Game saved to: " + savePath);
        }

        public static bool HasSavedGame() => File.Exists(savePath);
        
        public static RunData LoadRun()
        {
            if (HasSavedGame())
            {
                string json = File.ReadAllText(savePath);
                return JsonUtility.FromJson<RunData>(json);
            }
            Debug.LogWarning("Save file not found. Returning new game state.");
            return new RunData(); // Return default state if no save exists
        }
    }
}