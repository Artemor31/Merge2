using System.Linq;
using Databases;
using Gameplay.Units;
using Services.SaveService;
using UnityEngine;

namespace Services
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

        public void Merge(GridRuntimeData started, GridRuntimeData ended)
        {
            if (TryMerge(started.Actor, ended.Actor, out var newActor))
            {
                Object.Destroy(started.Actor.gameObject);
                started.Actor.Dispose();
                started.Actor = null;

                Object.Destroy(ended.Actor.gameObject);
                ended.Actor.Dispose();
                ended.Actor = null;

                newActor.transform.position = ended.Platform.transform.position;
                ended.Actor = newActor;
            }
        }

        public bool TryMerge(Actor actor, Actor actor2, out Actor result)
        {
            result = null;
            if (actor.Data.Level != actor2.Data.Level) return false;

            var unit = _unitsDatabase.Units.FirstOrDefault(u => u.Level == actor.Data.Level + 1);
            if (unit == null) return false;
            
            result = _gameFactory.CreateActor(unit.Race, unit.Mastery, unit.Level);
            
            
            
            return true;
        }
    }
}