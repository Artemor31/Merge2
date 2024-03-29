using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class AnimatorScheduler : MonoBehaviour
    {
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Atk = Animator.StringToHash("Attack");
        
        [SerializeField] private Animator _animator;

        public void Attack()
        {
            _animator.SetTrigger(Atk);
        }

        public void Move(float speed)
        {
            _animator.SetFloat(Speed, speed);
        }

        public void GoIdle()
        {
            _animator.SetFloat(Speed, 0);
        }

        public void Die()
        {
            _animator.SetTrigger(Died);
        }
    }
}