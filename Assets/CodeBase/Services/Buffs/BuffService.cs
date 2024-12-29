using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly GridDataService _gridDataService;

        public BuffService(DatabaseProvider databaseProvider, GridDataService gridDataService)
        {
            _configs = databaseProvider.GetDatabase<BuffsDatabase>().BuffConfigs;
            _gridDataService = gridDataService;

            FillDictionary(_races);
            FillDictionary(_masteries);
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

        public string CreteDescription()
        {
            StringBuilder stringBuilder = new();
            List<BuffConfig> buffs = CalculateBuffs(_gridDataService.PlayerUnits);
            foreach (BuffConfig buff in buffs)
            {
                stringBuilder.Append(buff.Description);
                stringBuilder.Append("\r\n");
            }
            
            return stringBuilder.ToString();
        }

        private List<BuffConfig> CalculateBuffs(ICollection<Actor> actors)
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