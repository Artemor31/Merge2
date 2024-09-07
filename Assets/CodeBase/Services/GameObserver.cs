using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Services.SaveService;

namespace Services
{
    public class GameObserver : IService
    {
        public event Action OnGameplaySrated;
        public event Action<bool> OnGameplayEnded;

        public bool IsWin { get; private set; }
        public int Profit { get; private set; }

        private readonly GridDataService _gridService;
        private readonly PlayerProgressService _playerService;
        private IEnumerable<Actor> _actors;
        
        public GameObserver(GridDataService gridService, PlayerProgressService playerService)
        {
            _gridService = gridService;
            _playerService = playerService;
        }

        public void StartWatch()
        {
            Profit = _playerService.Money;
            _gridService.EnemyUnits.ForEach(e => e.OnDied += OnEnemyDied);
            _gridService.GetPlayerUnits().ForEach(a => a.OnDied += OnAllyDied);
            OnGameplaySrated?.Invoke();
        }

        private void OnAllyDied(Actor actor)
        {
            if (_gridService.GetPlayerUnits().All(a => a.IsDead))
            {
                IsWin = false;
                EndGameplayLoop();
            }
        }

        private void OnEnemyDied(Actor actor)
        {
            if (_gridService.EnemyUnits.All(e => e.IsDead))
            {
                IsWin = true;
                EndGameplayLoop();
            }
        }

        private void EndGameplayLoop()
        {
            _gridService.EnemyUnits.ForEach(e => e.OnDied -= OnEnemyDied);
            _gridService.GetPlayerUnits().ForEach(a => a.OnDied -= OnAllyDied);
            Profit = _playerService.Money - Profit;
            OnGameplayEnded?.Invoke(IsWin);
        }
    }
}