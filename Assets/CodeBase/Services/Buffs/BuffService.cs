using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Databases.BuffConfigs;
using Gameplay.Units;
using Services.Infrastructure;
using Services.Resources;

namespace Services.Buffs
{
    public class BuffService : IService
    {
        private readonly BuffsDatabase _buffsDatabase;
        private readonly Dictionary<BuffConfig, int> _activeConfigs = new();
        private Dictionary<Mastery, int> _masteries = new();
        private Dictionary<Race, int> _races = new();

        public BuffService(DatabaseProvider databaseProvider)
        {
            _buffsDatabase = databaseProvider.GetDatabase<BuffsDatabase>();
            FillDictionary(_races);
            FillDictionary(_masteries);
        }

        public void ApplyBuffs(ICollection<Actor> allies, List<Actor> enemies)
        {
            CalculateBuffs(allies);

            foreach (var buffConfig in _activeConfigs)
            {
                if (buffConfig.Key.ForAllies)
                {
                    foreach (Actor actor in allies)
                    {
                        buffConfig.Key.ApplyTo(actor, buffConfig.Value);
                    }
                }
                else
                {
                    foreach (Actor actor in enemies)
                    {
                        buffConfig.Key.ApplyTo(actor, buffConfig.Value);
                    }
                }
            }
        }

        public IEnumerable<string> CalculateBuffs(ICollection<Actor> actors)
        {
            _races = _races.ToDictionary(p => p.Key, _ => 0);
            _masteries = _masteries.ToDictionary(p => p.Key, _ => 0);
            _activeConfigs.Clear();

            foreach (var actor in actors)
            {
                _races[actor.Data.Race]++;
                _masteries[actor.Data.Mastery]++;
            }

            foreach (var config in _buffsDatabase.BuffConfigs)
            {
                if (_races[config.Race] > 1)
                {
                    _activeConfigs.Add(config, _races[config.Race]);
                }

                if (_masteries[config.Mastery] > 1)
                {
                    _activeConfigs.Add(config, _masteries[config.Mastery]);
                }
            }

            return _activeConfigs.Keys.Select(k => k.Description);
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