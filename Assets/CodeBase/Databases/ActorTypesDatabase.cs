using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using UnityEngine;

namespace Databases
{
    
    [CreateAssetMenu(menuName = "Database/ActorTypesDatabase", fileName = "ActorTypesDatabase", order = 0)]
    public class ActorTypesDatabase : ScriptableObject
    {
        [SerializeField] private List<ActorTypeConfig> _configs;

        public IEnumerable<Race> GetAllRaces() => Extensions.AsCollection<Race>().Where(race => race != Race.None);
        public IEnumerable<Mastery> GetAllMasteries() => Extensions.AsCollection<Mastery>().Where(race => race != Mastery.None);
    }

    [Serializable]
    public class ActorTypeConfig
    {
        
    }
}