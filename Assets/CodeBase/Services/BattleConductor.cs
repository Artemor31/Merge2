using System.Collections.Generic;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.LevelData;
using CodeBase.Models;

namespace CodeBase.Services
{
    public class BattleConductor
    {
        private readonly GameplayModel _gameplayModel;
        private readonly LevelStaticData _staticData;
        private readonly WaveBuilder _waveBuilder;

        private bool _battling;

        public BattleConductor(ProgressService progressService, WaveBuilder waveBuilder)
        {
            _gameplayModel = progressService.GameplayModel;
            _staticData = progressService.StaticData;
            _waveBuilder = waveBuilder;
            
            _gameplayModel.StateChanged += GameplayModelOnStateChanged;
            GameplayModelOnStateChanged(_gameplayModel.State);
        }

        private void GameplayModelOnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Waiting:
                    _waveBuilder.BuildWave(_staticData, 0);
                    return;
                case GameState.Processing:
                    StartBattle();
                    return;
            }
        }

        private void StartBattle()
        {
            SetTargets(_gameplayModel.EnemyUnits, _gameplayModel.PlayerUnits);
            SetTargets(_gameplayModel.PlayerUnits, _gameplayModel.EnemyUnits);
        }

        private void SetTargets(IEnumerable<Unit> units, List<Unit> candidates)
        {
            foreach (Unit unit in units)
                unit.SetFighting(candidates);
        }
    }
}