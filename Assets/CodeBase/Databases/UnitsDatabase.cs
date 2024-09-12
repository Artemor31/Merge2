using System.Collections.Generic;
using System.Linq;
using Data;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create UnitsDatabase", fileName = "UnitsDatabase", order = 0)]
    public class UnitsDatabase : Database
    {
        [SerializeField] private string _assetsPath;
        public List<ActorConfig> Units;

        public ActorConfig ConfigFor(ActorData actorData) => Units.First(data => data.Data == actorData);

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