using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create UnitsDatabase", fileName = "UnitsDatabase", order = 0)]
    public class UnitsDatabase : Database
    {
        public List<UnitData> Units;
        
        [Serializable]
        public class UnitData
        {
            public UnitId Id;
            public Sprite Icon;
            public Actor Prefab;
            public string Name;
        }
    }
    
    public enum UnitId
    {
        None = 0,
        Skeleton = 1,
        Orc = 2
    }
}