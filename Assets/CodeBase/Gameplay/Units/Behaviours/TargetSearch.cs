using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Units.Behaviours
{
    public abstract class TargetSearch : MonoBehaviour
    {
        public abstract Vector3 TargetPoint { get; }
        public abstract Actor Target { get; protected set; }
        public abstract void SearchTarget(IReadOnlyList<Actor> candidates);
        public abstract bool NeedNewTarget();
    }
}