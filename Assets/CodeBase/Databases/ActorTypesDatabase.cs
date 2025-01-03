using System.Collections.Generic;
using UnityEngine;
using System;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/ActorTypesDatabase", fileName = "ActorTypesDatabase", order = 0)]
    public class ActorTypesDatabase : Database
    {
        public List<ActorTypeData> Datas;
    }
    
    [Serializable]
    public class ActorTypeData
    {
        public string Name => Mastery == Mastery.None ? Race.ToString() : Mastery.ToString();
        public Sprite Icon;
        public Mastery Mastery;
        public Race Race;
    }
}