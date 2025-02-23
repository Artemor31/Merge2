using System;
using System.Collections;
using System.Collections.Generic;
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
        private ParticleSystem _death;

        public void Initialize(CanvasHealthbar healthbar, ActorRank rank, ParticleSystem bloodVfx, ParticleSystem death)
        {
            _death = death;
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

        public void PerformAct() => _animator.SetTrigger(Atk);
        public void Move(float speed) => _animator.SetFloat(Speed, speed);
        public void GoIdle() => _animator.SetFloat(Speed, 0);
        
        public void Dispose()
        {
            if (_healthbar != null)
            {
                Destroy(_healthbar.gameObject);
                _healthbar = null;
            }            
            
            if (_rank != null)
            {
                Destroy(_rank.gameObject);
                _rank = null;
            }
        }

        public void Die()
        {
            _animator.SetTrigger(Died);
            StartCoroutine(Death());
            Dispose();
        }

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(0.7f);
            _death.Play();
        }
    }
}