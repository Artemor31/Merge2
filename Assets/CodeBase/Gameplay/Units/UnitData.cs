using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    [Serializable]
    public class UnitData
    {
        public UnitId Id;
        public Sprite Icon;
        public Unit Prefab;
    }

    public enum UnitId
    {
        Eblan = 0,
        Hyesos = 1,
        Rusich = 2,
    }
}