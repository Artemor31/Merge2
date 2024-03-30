using CodeBase.Gameplay.Units;
using CodeBase.LevelData;

namespace CodeBase.Services.SaveService
{
    public class GridRuntimeData
    {
        public bool Busy => Actor != null;

        public Actor Actor;
        public Platform Platform;
    }
}