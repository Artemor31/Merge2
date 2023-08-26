using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    [Serializable]
    public class UnitData
    {
        public UnitId Id;
        public Sprite Icon;
        public Damageable Prefab;
        public string Name;
    }

    public enum UnitId
    {
        None = 0,
        Eblan = 1,
        Hyesos = 2,
        Rusich = 3,
    }
}