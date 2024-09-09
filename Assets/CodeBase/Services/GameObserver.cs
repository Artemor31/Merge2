using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Services.SaveService;

namespace Services
{
    public class GameObserver : IService
    {
        public event Action<bool> OnGameplayEnded;

        public bool IsWin { get; private set; }
        public int Profit { get; private set; }

        private readonly GridDataService _gridService;
        private readonly PlayerProgressService _playerService;
        private readonly WaveBuilder _waveBuilder;
        private IEnumerable<Actor> _actors;

        public GameObserver(GridDataService gridService, 
                            PlayerProgressService playerService,
                            WaveBuilder waveBuilder)
        {
            _gridService = gridService;
            _playerService = playerService;
            _waveBuilder = waveBuilder;
        }

        public void StartGameplayLoop()
        {
            foreach (Actor actor in _waveBuilder.EnemyUnits)
                actor.Died += OnEnemyDied;

            foreach (Actor actor in _gridService.PlayerUnits)
                actor.Died += OnAllyDied;

            Profit = _playerService.Money;
        }

        private void EndGameplayLoop()
        {
            foreach (Actor actor in _waveBuilder.EnemyUnits)
                actor.Died -= OnEnemyDied;

            foreach (Actor actor in _gridService.PlayerUnits)
                actor.Died -= OnAllyDied;

            Profit = _playerService.Money - Profit;
            OnGameplayEnded?.Invoke(IsWin);
        }

        private void OnAllyDied()
        {
            if (_gridService.PlayerUnits.Any(a => !a.IsDead)) return;
            IsWin = false;
            EndGameplayLoop();
        }

        private void OnEnemyDied()
        {
            if (_waveBuilder.EnemyUnits.Any(a => !a.IsDead)) return;
            IsWin = true;
            EndGameplayLoop();
        }
    }
}