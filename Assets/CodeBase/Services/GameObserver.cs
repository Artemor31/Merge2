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

        private readonly GridDataService _gridService;
        private readonly PlayerProgressService _playerService;

        private IEnumerable<Actor> _actors;
        //private List<Actor> _enemies;
        //private List<Actor> _allies;

        public GameObserver(GridDataService gridService, PlayerProgressService playerService)
        {
            _gridService = gridService;
            _playerService = playerService;
        }

        public void StartWatch()
        {
            Profit = _playerService.Money;

            //_enemies = _dataProvider.EnemyUnits.Select(e => e).ToList();
            //_allies = _dataProvider.GetPlayerUnits().Select(a => a).ToList();
            _gridService.EnemyUnits.ForEach(e => e.OnDied += OnEnemyDied);
            _gridService.GetPlayerUnits().ForEach(a => a.OnDied += OnAllyDied);
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