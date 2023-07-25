using System.Collections.Generic;
using CodeBase.Units;

namespace CodeBase.Modules.Gameplay
{
    public interface IGameplayModel : IModel
    {
        List<Unit> EnemyUnits { get; set; }
        List<Unit> PlayerUnits { get; set; }
    }
}