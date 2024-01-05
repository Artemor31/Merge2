using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class Mover : MonoBehaviour
    {
        public abstract void MoveTo(Vector3 target);
        public abstract void MoveTo(Unit target);
        public abstract void Reset();
    }
}