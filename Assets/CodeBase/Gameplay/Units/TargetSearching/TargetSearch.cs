using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Units.TargetSearching
{
    public abstract class TargetSearch : MonoBehaviour
    {
        public Actor Target { get; protected set; }
        public abstract void SearchTarget(ICollection<Actor> candidates);
        public virtual void Dispose() => Target = null;
        public virtual bool NeedNewTarget() => !Target || Target.IsDead;
        protected float DistanceTo(Actor target) => Vector3.Distance(target.transform.position, transform.position);
    }
}