using System;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Gameplay
{
    public class UnitCell : MonoBehaviour
    {
        public bool Free => _unit != null;
        
        private Unit _unit;


        public void SetUnit(Unit unit)
        {
            if (_unit != null)
                throw new Exception("Unit already exist");
            _unit = unit;
            _unit.transform.position = transform.position;
        }
    }
}