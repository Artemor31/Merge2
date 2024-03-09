using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Services
{
    public class GameFactory : IService
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly UnitsDatabase _database;

        public GameFactory(DatabaseProvider database, AssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
            _database = database.GetDatabase<UnitsDatabase>();
        }

        public Unit CreateUnit(UnitId id)
        {
            var unitData = _database.Units.First(u => u.Id == id);
            var unit = Object.Instantiate(unitData.Prefab);
            unit.gameObject.name += Random.Range(0, 10000);
            return unit;
        }
    }
}