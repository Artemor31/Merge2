using Gameplay.LevelItems;
using Gameplay.Units;

namespace Data
{
    public class GridRuntimeData
    {
        public bool Busy => Actor != null;
        public bool Free => Actor == null;
        public Actor Actor;
        public Platform Platform;
    }
}