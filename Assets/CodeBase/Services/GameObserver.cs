using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Units;
using CodeBase.Services.SaveService;

namespace CodeBase.Services
{
    public class GameObserver : IService
    {
        public event Action<bool> OnGameplayEnded;
        
        public bool IsWin { get; private set; }
        public int Profit { get; private set; }

        private readonly RuntimeDataRepository _dataRepository;

        private IEnumerable<Actor> _actors;
        //private List<Actor> _enemies;
        //private List<Actor> _allies;

        public GameObserver(RuntimeDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void StartWatch()
        {
            Profit = _dataRepository.Money;

            //_enemies = _dataProvider.EnemyUnits.Select(e => e).ToList();
            //_allies = _dataProvider.GetPlayerUnits().Select(a => a).ToList();
            _dataRepository.EnemyUnits.ForEach(e => e.OnDied += OnEnemyDied);
            _dataRepository.GetPlayerUnits().ForEach(a => a.OnDied += OnAllyDied);
        }

        private void OnAllyDied(Actor actor)
        {
            if (_dataRepository.GetPlayerUnits().All(a => a.IsDead))
            {
                IsWin = false;
                EndGameplayLoop();
            }
        }

        private void OnEnemyDied(Actor actor)
        {
            if (_dataRepository.EnemyUnits.All(e => e.IsDead))
            {
                IsWin = true;
                EndGameplayLoop();
            }
        }

        private void EndGameplayLoop()
        {
            _dataRepository.EnemyUnits.ForEach(e => e.OnDied -= OnEnemyDied);
            _dataRepository.GetPlayerUnits().ForEach(a => a.OnDied -= OnAllyDied);
            Profit = _dataRepository.Money - Profit;
            OnGameplayEnded?.Invoke(IsWin);
        }
    }
}