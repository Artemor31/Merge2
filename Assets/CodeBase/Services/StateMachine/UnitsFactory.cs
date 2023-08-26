using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Services.StateMachine
{
    public class UnitsFactory
    {
        private readonly UnitsDatabase _database;

        public UnitsFactory(UnitsDatabase database)
        {
            _database = database;
        }

        public Damageable CreateUnit(UnitId id)
        {
            var unitData = _database.Units.First(u => u.Id == id);
            return Object.Instantiate(unitData.Prefab);
        }
    }
}