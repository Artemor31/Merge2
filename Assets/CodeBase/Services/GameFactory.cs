using System.Linq;
using CodeBase.Databases;
using CodeBase.Databases.Data;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Services
{
    public class GameFactory : IService
    {
        private readonly UnitsDatabase _database;
        private readonly UnitCell _cellPrefab;

        public GameFactory(DatabaseProvider database, AssetsProvider assetsProvider)
        {
            _database = database.GetDatabase<UnitsDatabase>();
            _cellPrefab = assetsProvider.Load<UnitCell>(AssetsPath.UnitCellPrefab);
        }

        public UnitCell CreateCell(Vector3 position) => 
            Object.Instantiate(_cellPrefab, position, Quaternion.identity);

        public Unit CreateUnit(UnitId id)
        {
            var unitData = _database.Units.First(u => u.Id == id);
            var unit = Object.Instantiate(unitData.Prefab);
            unit.gameObject.name += Random.Range(0, 10000);
            return unit;
        }
    }
}