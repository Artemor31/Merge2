using CodeBase.Gameplay.Units;
using CodeBase.LevelData;
using UnityEngine;

namespace CodeBase.Services.SaveService
{
    public class GridRuntimeData
    {
        public bool Busy => Actor != null;
        public Vector2Int Index => Platform.Index;

        public Actor Actor;
        public Platform Platform;
    }
}