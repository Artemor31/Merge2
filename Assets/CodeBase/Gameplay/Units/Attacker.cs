using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class Attacker : MonoBehaviour
    {
        public abstract bool CanAttack(Actor actor);
        public abstract void Attack(Actor actor);
        public abstract bool InRange(Actor actor);
        public abstract void Tick();
        public abstract void Init(AnimatorScheduler animator);
    }
}