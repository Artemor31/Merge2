using System.Linq;
using CodeBase.Databases;
using CodeBase.Databases.Data;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Services
{
    public class GameplayFactory
    {
        private readonly UnitsDatabase _database;
        private readonly UnitCell _cellPrefab;

        public GameplayFactory(UnitsDatabase database, AssetsProvider assetsProvider)
        {
            _database = database;
            _cellPrefab = assetsProvider.Load<UnitCell>(AssetsPath.UnitCellPrefab);
        }

        public UnitCell CreateCell(Vector3 position) => 
            Object.Instantiate(_cellPrefab, position, Quaternion.identity);

        public Unit CreateUnit(UnitId id)
        {
            var unitData = _database.Units.First(u => u.Id == id);
            return Object.Instantiate(unitData.Prefab);
        }
    }
}