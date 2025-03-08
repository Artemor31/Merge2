using System;
using Infrastructure;
using Services;
using UnityEngine;

namespace Gameplay
{
    public class Confetti : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private void Start()
        {
            _particleSystem.gameObject.SetActive(false);
            ServiceLocator.Resolve<GameplayContainer>().Add(this);
        }

        public void Play() => _particleSystem.gameObject.SetActive(true);
    }
}
