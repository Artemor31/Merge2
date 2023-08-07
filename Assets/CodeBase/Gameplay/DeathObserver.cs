using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.Models;

namespace CodeBase.Gameplay
{
    public class DeathObserver
    {
        private readonly GameplayModel _gameplayModel;
        private readonly UnitsDatabase _unitsDatabase;

        public DeathObserver(GameplayModel gameplayModel, UnitsDatabase unitsDatabase)
        {
            _gameplayModel = gameplayModel;
            _unitsDatabase = unitsDatabase;
        }

        public void StartWatch()
        {
            foreach (var unit in _gameplayModel.EnemyUnits)
            {
                unit.Died += OnEnemyDies;
            }
        }

        private void OnEnemyDies(IUnit unit)
        {
            foreach (var unitData in _unitsDatabase.Units)
            {
                if (unitData._id == unit.Id)
                {
                
                    return;
                }
            }
        }
    }
}