using System.Collections.Generic;
using System.Linq;
using Databases.Data;
using Infrastructure;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/UnitsDatabase", fileName = "UnitsDatabase", order = 0)]
    public class UnitsDatabase : Database
    {
        [SerializeField] private string _assetsPath;
        [SerializeField] public List<ActorConfig> Units;
        Dictionary<Race, Dictionary<Mastery, ActorConfig[]>> _cache;

        public override void Cache()
        {
            const int arraySize = 3;
            _cache = new Dictionary<Race, Dictionary<Mastery, ActorConfig[]>>();

            foreach (ActorConfig unit in Units)
            {
                if (_cache.TryGetValue(unit.Data.Race, out Dictionary<Mastery, ActorConfig[]> masteries))
                {
                    if (!masteries.ContainsKey(unit.Data.Mastery))
                    {
                        masteries.Add(unit.Data.Mastery, new ActorConfig[arraySize]);
                    }

                    masteries[unit.Data.Mastery][unit.Data.Level - 1] = unit;
                }
                else
                {
                    masteries = new Dictionary<Mastery, ActorConfig[]>();
                    masteries.Add(unit.Data.Mastery, new ActorConfig[arraySize]);
                    masteries[unit.Data.Mastery][unit.Data.Level - 1] = unit;
                    _cache.Add(unit.Data.Race, masteries);
                }
            }

            Debug.LogError("VAR");
        }

        public ActorConfig ConfigFor(int level) => Units.Random(u => u.Data.Level == level);
        public ActorConfig ConfigFor(ActorData actorData) => Units.FirstOrDefault(data => data.Data == actorData);

        public List<ActorConfig> ConfigsFor(int level, Race[] races, Mastery[] masteries) => Units
                                                                                             .Where(u => u.Data.Level == level)
                                                                                             .Where(u => races.Contains(u.Data.Race))
                                                                                             .Where(u => masteries.Contains(u.Data.Mastery))
                                                                                             .ToList();
        
        public List<ActorConfig> ConfigsFor(int level) => Units.Where(u => u.Data.Level == level).ToList();     

        [Button]
        public void CollectData()
        {
            Units = new();
            ActorConfig[] configs = Resources.LoadAll<ActorConfig>(_assetsPath);
            foreach (ActorConfig config in configs)
            {
                Units.Add(config);
            }
        }
    }
}