using System.Collections.Generic;
using System.Linq;
using Databases.Data;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/UnitsDatabase", fileName = "UnitsDatabase", order = 0)]
    public class UnitsDatabase : Database
    {
        [SerializeField] private string _assetsPath;
        [SerializeField] public List<ActorConfig> Units;
        private Dictionary<Race, Dictionary<Mastery, ActorConfig[]>> _cache;

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
        }

        public ActorConfig ConfigFor(ActorData data) => _cache[data.Race][data.Mastery][data.Level - 1];

        public Dictionary<Race, IEnumerable<Mastery>> AllActorTypes() =>
            _cache.ToDictionary<KeyValuePair<Race, Dictionary<Mastery, ActorConfig[]>>, Race, IEnumerable<Mastery>>(
                races => races.Key, races => races.Value.Keys);

        public IEnumerable<ActorConfig> ConfigsFor(int level, List<Race> races, List<Mastery> masteries)
        {
            foreach (Race race in races)
            {
                if (_cache.TryGetValue(race, out var masteryDict))
                {
                    foreach (Mastery mastery in masteries)
                    {
                        if (masteryDict.TryGetValue(mastery, out ActorConfig[] data))
                        {
                            yield return data[level - 1];
                        }
                    }
                }
            }
        }

        public IEnumerable<ActorConfig> ConfigsFor(int level) =>
            _cache.SelectMany(races => races.Value, (_, masteries) => masteries.Value[level - 1]);

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