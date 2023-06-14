using UnityEngine;

namespace MergeIdle.Scripts.Storage.Controller
{
    public class StorageController:MonoBehaviour
    {
        public string GetValue(string key)
        {
            return PlayerPrefs.GetString(key, "");
        }
        
        public void SetValue(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        
        public int GetIntValue(string key)
        {
            return PlayerPrefs.GetInt(key, 0);
        }
        
        public void SetIntValue(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
        
        public float GetFloatValue(string key)
        {
            return PlayerPrefs.GetFloat(key, 0f);
        }
        
        public void SetFloatValue(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}