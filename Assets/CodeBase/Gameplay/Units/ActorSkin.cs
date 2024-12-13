using Gameplay.Units.Healths;
using UnityEngine;

namespace Gameplay.Units
{
    public class ActorSkin : MonoBehaviour
    {
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Atk = Animator.StringToHash("Attack");
        
        [SerializeField] private Animator _animator;
        private Healthbar _healthbar;

        public void Initialize(Healthbar healthbar) => _healthbar = healthbar;
        public void SetHealth(float ratio) => _healthbar.ChangeHealth(ratio);
        public void PerformAct() => _animator.SetTrigger(Atk);
        public void Move(float speed) => _animator.SetFloat(Speed, speed);
        public void GoIdle() => _animator.SetFloat(Speed, 0);
        public void Die() => _animator.SetTrigger(Died);
    }
}