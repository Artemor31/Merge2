using System;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Services.SaveService
{
    public class PlayerDataRepository : IRepository<PlayerData>
    {
        private readonly PlayerData _data;

        public int Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }

        public int Wave => _data.Wave;

        public PlayerDataRepository() => _data = Restore();
        public void NextWave() => _data.Wave++;

        public void Save(PlayerData data)
        {
            string serializeObject = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString("PlayerData", serializeObject);
        }

        public PlayerData Restore()
        {
            string json = PlayerPrefs.GetString("PlayerData", string.Empty);
            return json == string.Empty ? new PlayerData() : JsonConvert.DeserializeObject<PlayerData>(json);
        }
    }
    
    [Serializable]
    public class PlayerData
    {
        public int Money = 10;
        public int Wave;
        public int Coins;
    }
}