using System;
using Newtonsoft.Json;
using Services.Infrastructure;
using UnityEngine;

namespace Services
{

    [Serializable]
    public class SaveData
    {
        public virtual void Serialize(){}
        public virtual void Deserialize(){}
    }
    
    public class SaveService : IService
    {
        public void Save<TData>(string path, TData data) where TData : SaveData
        {
            data.Serialize();
            PlayerPrefs.SetString(path, JsonConvert.SerializeObject(data));
        }

        public TData Restore<TData>(string path) where TData : SaveData, new()
        {
            if (!PlayerPrefs.HasKey(path))
            {
                return new TData();
            }
            
            string json = PlayerPrefs.GetString(path);
            TData data = JsonConvert.DeserializeObject<TData>(json);
            data.Deserialize();
            return data;
        }
    }
}