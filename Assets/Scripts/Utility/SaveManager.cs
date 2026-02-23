using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Threading.Tasks;

namespace Eduzo.Games.Utility {
    /// <summary>
    /// Generic Save Manager
    /// </summary>
    /// <typeparam name="T"></typeparam> Save Data Class
    public class SaveManager<T> {
        public virtual void ClearSave(GameName gameType) {
            string path = GetPath(gameType.ToString());
            try {
                File.Delete(path);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }
        public bool HasSaveData(GameName gameType) {
            string path = GetPath(gameType.ToString());
            try {
                return File.Exists(path);
            }
            catch (Exception e) {
                Debug.LogError(e);
                return false;
            }
        }
        public async Task SaveGameAsync(GameName gameType, T saveData = default) {
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            string path = GetPath(gameType.ToString());
            try {
                await Task.Run(() => {
                    File.WriteAllText(path, json);
                });
                Debug.Log("Result saved to: " + path);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }
        public async Task<T> LoadGameAsync(GameName gameType) {
            string path = GetPath(gameType.ToString());
            try {
                string json = "";
                await Task.Run(() => {
                    if (File.Exists(path)) {
                        json = File.ReadAllText(path);
                    }
                });
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
            return default;
        }
        private string GetPath(string gameType) {
            string fileName = $"{gameType}_Result_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }

    public enum GameName {
        BoxingMayhem
        // Add other game types here if needed
    }
}