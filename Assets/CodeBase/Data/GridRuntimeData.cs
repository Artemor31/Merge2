using Gameplay.LevelItems;
using Gameplay.Units;
using UnityEngine;

namespace Data
{
    public class GridRuntimeData
    {
        public bool Busy => Actor != null;
        public bool Free => Actor == null;
        public Vector2Int Index => Platform.Index;

        public Actor Actor;
        public Platform Platform;
    }
}