﻿using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;

namespace CodeBase.Services
{
    public class MergeService : IService
    {
        private readonly GameFactory _gameFactory;
        private readonly UnitsDatabase _unitsDatabase;

        public MergeService(GameFactory gameFactory, DatabaseProvider databaseProvider)
        {
            _gameFactory = gameFactory;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
        }

        public bool TryMerge(Actor actor, Actor actor2, out Actor result)
        {
            result = null;
            if (actor.Level != actor2.Level) return false;

            var unit = _unitsDatabase.Units.FirstOrDefault(u => u.Level == actor.Level + 1);
            if (unit == null) return false;
            
            result = _gameFactory.CreateActor(unit.Race, unit.Mastery, unit.Level);
            return true;
        }
    }
}