using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.SaveService
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
        public ActorData[,] UnitIds;
        public GridData(ActorData[,] unitId) => UnitIds = unitId;
    }
}