using Gameplay.Units.Healths;
using UI.GameplayWindow;
using UnityEngine;

namespace Gameplay.Units
{
    public class ActorSkin : MonoBehaviour
    {
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Atk = Animator.StringToHash("Attack");
        
        [SerializeField] private Animator _animator;
        private CanvasHealthbar _healthbar;
        private ActorRank _rank;
        private ParticleSystem _bloodVfx;

        public void Initialize(CanvasHealthbar healthbar, ActorRank rank, ParticleSystem bloodVfx)
        {
            _bloodVfx = bloodVfx;
            _rank = rank;
            _healthbar = healthbar;
        }

        public void ChangeHealth(float currentRatio, HealthContext context)
        {
            if (_healthbar == null) return;
            _healthbar.ChangeHealth(currentRatio);
            if (context == HealthContext.Crit)
            {
                _bloodVfx.Play();
            }
        }

        public void Die() => _animator.SetTrigger(Died);
        public void PerformAct() => _animator.SetTrigger(Atk);
        public void Move(float speed) => _animator.SetFloat(Speed, speed);
        public void GoIdle() => _animator.SetFloat(Speed, 0);
    }
}