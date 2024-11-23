using System;
using Databases;
using System.Linq;
using Gameplay.Units;
using System.Collections.Generic;

namespace Services.BuffService
{
    public class BuffService : IService
    {
        private readonly List<BuffConfig> _configs;
        private List<BuffConfig> _activeConfigs;
        private Dictionary<Race, int> _races = new();
        private Dictionary<Mastery, int> _masteries = new();

        public BuffService(DatabaseProvider databaseProvider)
        {
            _configs = databaseProvider.GetDatabase<BuffsDatabase>().BuffConfigs;
            FillDictionary(_races);
            FillDictionary(_masteries);
        }

        public List<BuffConfig> CalculateBuffs(ICollection<Actor> actors)
        {
            Clear();

            foreach (var actor in actors)
            {
                _races[actor.Data.Race]++;
                _masteries[actor.Data.Mastery]++;
            }

            var active = new List<BuffConfig>();
            foreach (var config in _configs)
            {
                if (_races[config.Race] > 1)
                {
                    active.Add(config);
                }

                if (_masteries[config.Mastery] > 1)
                {
                    active.Add(config);
                }
            }

            _activeConfigs = active;
            return _activeConfigs;
        }

        // change this to ecs like thing
        // create pool of active systems, that itereate through actors and do smth
        // anyway, move away grom using monobehaviours and addComponent things
        public void ApplyBuffs(ICollection<Actor> actors)
        {
            foreach (BuffConfig buffConfig in _activeConfigs)
            {
                foreach (var actor in actors)
                {
                    actor.gameObject.AddComponent(buffConfig.Behaviour.Type);
                }
            }
        }

        private void Clear()
        {
            _races = _races.ToDictionary(p => p.Key, _ => 0);
            _masteries = _masteries.ToDictionary(p => p.Key, _ => 0);
        }

        private void FillDictionary<T>(IDictionary<T, int> dict) where T : Enum
        {
            foreach (T value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                dict.Add(value, 0);
            }
        }
    }
}