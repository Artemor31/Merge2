using System.Collections.Generic;
using CodeBase.Units;

namespace CodeBase.Modules.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        public List<Unit> EnemyUnits { get; set; }
        public List<Unit> PlayerUnits { get; set; }
    }
}