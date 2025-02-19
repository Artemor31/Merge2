using System;
using System.Collections.Generic;
using Databases.Data;

namespace Services.SaveProgress
{
    [Serializable]
    public class GridProgress : SaveData
    {
        public List<ActorData> UnitIds;
        public GridProgress(List<ActorData> actorDatas) => UnitIds = actorDatas;
        public GridProgress() => UnitIds = null;
    }
}