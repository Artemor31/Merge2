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
        public UnitId[,] UnitIds;
        public GridData(UnitId[,] unitId) => UnitIds = unitId;
        public GridData() => UnitIds = null;
    }
}