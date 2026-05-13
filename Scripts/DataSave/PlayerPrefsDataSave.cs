using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Default backend: PlayerPrefs + JsonUtility.
    /// Phù hợp cho prototype / mobile. Không cần thêm package ngoài.
    /// </summary>
    public class PlayerPrefsDataSave : IDataSave
    {
        public void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            if (!PlayerPrefs.HasKey(key)) return defaultValue;

            var json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json)) return defaultValue;

            return JsonUtility.FromJson<T>(json);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}
