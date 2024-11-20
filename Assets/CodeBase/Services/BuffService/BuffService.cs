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
        private Dictionary<Race, int> _races = new();
        private readonly Dictionary<Mastery, int> _masteries = new();

        public BuffService(DatabaseProvider databaseProvider)
        {
            _configs = databaseProvider.GetDatabase<BuffsDatabase>().BuffConfigs;
            
            foreach (var race in Enum.GetValues(typeof(Race)).Cast<Race>().Skip(1))
            {
                _races.Add(race, 0);
            }
            foreach (var mastery in Enum.GetValues(typeof(Mastery)).Cast<Mastery>().Skip(1))
            {
                _masteries.Add(mastery, 0);
            }
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

            return active;
        }

        private void Clear()
        {
            foreach (var key in _races.Keys)
            {
                _races[key] = 0;
            }    
            
            foreach (var key in _masteries.Keys)
            {
                _masteries[key] = 0;
            }
        }

        public void ApplyBuffs(List<Actor> actors)
        {
        }
    }
}