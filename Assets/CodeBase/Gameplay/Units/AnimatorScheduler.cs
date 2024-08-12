using UnityEngine;

namespace Gameplay.Units
{
    public class AnimatorScheduler : MonoBehaviour
    {
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Atk = Animator.StringToHash("Attack"); 
        private Animator _animator;

        public void Init(Animator animator) => _animator = animator;
        public void PerformAct() => _animator.SetTrigger(Atk);
        public void Move(float speed) => _animator.SetFloat(Speed, speed);
        public void GoIdle() => _animator.SetFloat(Speed, 0);
        public void Die() => _animator.SetTrigger(Died);
    }
}