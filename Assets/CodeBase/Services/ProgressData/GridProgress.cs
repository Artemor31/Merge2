using System;
using System.Collections.Generic;
using Databases;

namespace Services.ProgressData
{
    [Serializable]
    public class GridProgress : SaveData
    {
        public List<ActorData> UnitIds;
        public GridProgress(List<ActorData> actorDatas) => UnitIds = actorDatas;
        public GridProgress() => UnitIds = null;
    }
}