using Databases;
using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public abstract class Mover : MonoBehaviour
    {
        public abstract void Init(AnimatorScheduler animator, ActorStats stats);
        public abstract void MoveTo(Actor target);
        public abstract void Stop();
        public abstract void Dispose();
    }
}