using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    [Serializable]
    public class UnitData
    {
        public UnitId _id;
        public Sprite Icon;
        public GameObject Prefab;
    }

    public enum UnitId
    {
        Eblan = 0,
        Hyesos = 1,
        Rusich = 2,
    }
}