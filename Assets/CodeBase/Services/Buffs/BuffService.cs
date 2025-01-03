using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Gameplay.Units;
using Services.GridService;
using Services.Infrastructure;
using Services.Resources;

namespace Services.Buffs
{
    public class BuffService : IService
    {
        private readonly List<BuffConfig> _configs;
        private List<BuffConfig> _activeConfigs;
        private Dictionary<Race, int> _races = new();
        private Dictionary<Mastery, int> _masteries = new();
        private readonly GridDataService _gridService;
        private readonly GameplayDataService _gameplayService;
        private readonly WaveBuilder _waveBuilder;

        public BuffService(DatabaseProvider databaseProvider)
        {
            _configs = databaseProvider.GetDatabase<BuffsDatabase>().BuffConfigs;
            FillDictionary(_races);
            FillDictionary(_masteries);
        }
        
        public void ApplyBuffs(ICollection<Actor> actors)
        {
            if (_activeConfigs == null)
            {
                _activeConfigs = new List<BuffConfig>();
                CalculateBuffs(actors);
            }
            
            foreach (BuffConfig buffConfig in _activeConfigs)
            {
                foreach (Actor actor in actors)
                {
                    actor.gameObject.AddComponent(buffConfig.Behaviour.Type);
                }
            }
        }
        
        public List<BuffConfig> CalculateBuffs(ICollection<Actor> actors)
        {
            _races = _races.ToDictionary(p => p.Key, _ => 0);
            _masteries = _masteries.ToDictionary(p => p.Key, _ => 0);
            _activeConfigs.Clear();

            foreach (var actor in actors)
            {
                _races[actor.Data.Race]++;
                _masteries[actor.Data.Mastery]++;
            }

            foreach (var config in _configs)
            {
                if (_races[config.Race] > 1)
                {
                    _activeConfigs.Add(config);
                }

                if (_masteries[config.Mastery] > 1)
                {
                    _activeConfigs.Add(config);
                }
            }

            return _activeConfigs;
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