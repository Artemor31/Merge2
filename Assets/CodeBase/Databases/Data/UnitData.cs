using System;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases.Data
{
    [Serializable]
    public class UnitData
    {
        public UnitId Id;
        public Sprite Icon;
        public Unit Prefab;
        public string Name;
    }

    public enum UnitId
    {
        None = 0,
        Blue = 1,
        Red = 2,
        Orange = 3,
    }
}