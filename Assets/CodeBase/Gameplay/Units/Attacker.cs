using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class Attacker : MonoBehaviour
    {
        public abstract bool CanAttack(Unit unit);
        public abstract void Attack(Unit unit);
    }
}