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

        public void Initialize(CanvasHealthbar healthbar) => _healthbar = healthbar;
        public void ChangeHealth(float currentRatio) => _healthbar.ChangeHealth(currentRatio);
        public void PerformAct() => _animator.SetTrigger(Atk);
        public void Move(float speed) => _animator.SetFloat(Speed, speed);
        public void GoIdle() => _animator.SetFloat(Speed, 0);

        public void Die()
        {
            _animator.SetTrigger(Died);
            Dispose();
        }

        public void Dispose()
        {
            if (_healthbar == null) return;
            _healthbar.UnInit();
            Destroy(_healthbar.gameObject);
            _healthbar = null;
        }
    }
}