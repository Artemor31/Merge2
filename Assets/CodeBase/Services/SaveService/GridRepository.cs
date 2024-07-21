using System;
using CodeBase.Databases;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Services.SaveService
{
    public class GridRepository : IRepository<GridData>
    {
        private readonly GridData _data;

        public GridRepository() => _data = Restore();
        public GridData GetData() => _data;

        public void Save(GridData saveData)
        {
            string serializeObject = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString("GridData", serializeObject);
        }

        public GridData Restore()
        {
            string json = PlayerPrefs.GetString("GridData", string.Empty);
            return json == string.Empty ? null : JsonConvert.DeserializeObject<GridData>(json);
        }
    }
    
    [Serializable]
    public class GridData
    {
        public UnitData[,] UnitIds;
        public GridData(UnitData[,] unitId) => UnitIds = unitId;
    }
    
    [Serializable]
    public struct UnitData
    {
        public static UnitData None => new(Race.None, Mastery.None, 0);
        
        public Race Race;
        public Mastery Mastery;
        public int Level;

        public UnitData(Race race, Mastery mastery, int level)
        {
            Race = race;
            Mastery = mastery;
            Level = level;
        }

        public override bool Equals(object obj) => obj is UnitData data && Equals(data);
        private bool Equals(UnitData other) => Race == other.Race && Mastery == other.Mastery;
        public override int GetHashCode() => HashCode.Combine((int)Race, (int)Mastery);
    }
}