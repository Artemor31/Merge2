using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class Attacker : MonoBehaviour
    {
        public abstract bool CanAttack(Actor actor);
        public abstract void Attack(Actor actor);
        public abstract void Reset();
        public abstract bool InRange(Vector3 transformPosition);
    }
}