using System;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Gameplay
{
    public class UnitCell : MonoBehaviour
    {
        public bool Free => _actor != null;
        
        private Actor _actor;


        public void SetUnit(Actor actor)
        {
            if (_actor != null)
                throw new Exception("Unit already exist");
            _actor = actor;
            _actor.transform.position = transform.position;
        }
    }
}