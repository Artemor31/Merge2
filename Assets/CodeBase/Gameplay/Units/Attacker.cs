using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class Attacker : MonoBehaviour
    {
        public abstract bool CanAttack(Unit unit);
        public abstract void Attack(Unit unit);
        public abstract void Reset();
        public abstract bool InRange(Vector3 transformPosition);
    }
}