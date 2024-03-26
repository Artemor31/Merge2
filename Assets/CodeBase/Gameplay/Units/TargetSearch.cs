using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class TargetSearch : MonoBehaviour
    {
        public abstract Actor Target { get; protected set; }
        public abstract void SearchTarget(List<Actor> candidates);
        public abstract void Disable();
        public abstract bool NeedNewTarget();
    }
}