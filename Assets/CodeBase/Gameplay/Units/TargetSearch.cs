using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class TargetSearch : MonoBehaviour
    {
        public abstract Unit Target { get; protected set; }
        public abstract void SearchTarget(List<Unit> candidates);
        public abstract void Reset();
    }
}