using System.Collections.Generic;
using System.Linq;
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
        private readonly IUpdateable _updateable;
        private readonly WaveBuilder _waveBuilder;

        private bool _battling;

        public BattleConductor(GameplayModel gameplayModel, LevelStaticData staticData,
                               IUpdateable updateable, WaveBuilder waveBuilder)
        {
            _gameplayModel = gameplayModel;
            _staticData = staticData;
            _updateable = updateable;
            _waveBuilder = waveBuilder;
            _updateable.Tick += Tick;
            _gameplayModel.StateChanged += GameplayModelOnStateChanged;
            GameplayModelOnStateChanged(_gameplayModel.State);
        }

        private void Tick()
        {
            if (_gameplayModel.State != GameState.Processing) return;

            foreach (var unit in _gameplayModel.EnemyUnits)
            {
                if (unit.Health.Current <= 0) continue;
                
                unit.Mover.MoveTo(unit.TargetSearch.Target);
                unit.Attacker.Attack(unit.TargetSearch.Target);
            }

            foreach (var unit in _gameplayModel.PlayerUnits)
            {
                if (unit.Health.Current <= 0) continue;
                
                unit.Mover.MoveTo(unit.TargetSearch.Target);
                unit.Attacker.Attack(unit.TargetSearch.Target);
            }
        }

        private void GameplayModelOnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Waiting:
                    SetUnitsIdle();
                    break;

                case GameState.Processing:
                    StartBattle();
                    break;
            }
        }

        private void SetUnitsIdle()
        {
            _waveBuilder.BuildWave(_staticData, 0);
            
        }

        private void StartBattle()
        {
            var enemies = _gameplayModel.EnemyUnits;
            var allies = _gameplayModel.PlayerUnits;

            SetTargets(enemies.Select(e => e.TargetSearch), allies);
            SetTargets(allies.Select(e => e.TargetSearch), enemies);
        }

        private void SetTargets(IEnumerable<TargetSearch> searchers, List<Unit> candidates)
        {
            foreach (var search in searchers)
                search.SetTargets(candidates);
        }
    }
}