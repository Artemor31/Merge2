using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create UnitsDatabase", fileName = "UnitsDatabase", order = 0)]
    public class UnitsDatabase : Database
    {
        [SerializeField] private string _assetsPath;
        [SerializeField] public List<ActorConfig> Units;

        public ActorConfig ConfigFor(ActorData actorData) => Units.FirstOrDefault(data => data.Data == actorData);
        public ActorConfig ConfigFor(int level) => Units.Random(u => u.Data.Level == level);

        public List<ActorConfig> ConfigsFor(int maxPower, Race[] races, Mastery[] masteries) => Units
            .Where(u => u.Power == maxPower)
            .Where(u => races.Contains(u.Data.Race))
            .Where(u => masteries.Contains(u.Data.Mastery))
            .ToList();

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