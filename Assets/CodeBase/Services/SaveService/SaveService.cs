using Newtonsoft.Json;
using UnityEngine;

namespace Services.SaveService
{
    public class SaveService : IService
    {
        public void Save<TData>(string path, TData data) => 
            PlayerPrefs.SetString(path, JsonConvert.SerializeObject(data));

        public TData Restore<TData>(string path) where TData : class, new()
        {
            if (!PlayerPrefs.HasKey(path))
            {
                return new TData();
            }
            
            string json = PlayerPrefs.GetString(path);
            return JsonConvert.DeserializeObject<TData>(json);
        }
    }
}