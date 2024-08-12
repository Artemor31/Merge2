using Gameplay.Units;
using LevelData;
using UnityEngine;

namespace Services.SaveService
{
    public class GridRuntimeData
    {
        public bool Busy => Actor != null;
        public Vector2Int Index => Platform.Index;

        public Actor Actor;
        public Platform Platform;
    }
}