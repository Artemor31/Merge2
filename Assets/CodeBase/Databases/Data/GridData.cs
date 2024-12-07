using System;

namespace Databases.Data
{
    [Serializable]
    public class GridData
    {
        public ActorData[,] UnitIds;
        public GridData(ActorData[,] unitId) => UnitIds = unitId;
        public GridData() => UnitIds = null;
    }
}