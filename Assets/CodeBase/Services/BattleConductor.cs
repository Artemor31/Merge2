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

        public BattleConductor(ProgressService progressService, IUpdateable updateable, WaveBuilder waveBuilder)
        {
            _gameplayModel = progressService.GameplayModel;
            _staticData = progressService.StaticData;
            _updateable = updateable;
            _waveBuilder = waveBuilder;
            _updateable.Tick += Tick;
            _gameplayModel.StateChanged += GameplayModelOnStateChanged;
            GameplayModelOnStateChanged(_gameplayModel.State);
        }

        private void Tick()
        {
            if (_gameplayModel.State != GameState.Processing) return;

            var enemies = _gameplayModel.EnemyUnits;
            var allies = _gameplayModel.PlayerUnits;

            ProcessUnits(enemies);
            ProcessUnits(allies);
        }

        private static void ProcessUnits(List<Unit> enemies)
        {
            foreach (var unit in enemies)
            {
                if (unit.Health.Current <= 0) continue;
                ProcessUnitBehaviour(unit);
            }
        }

        private static void ProcessUnitBehaviour(Unit unit)
        {
            var target = unit.TargetSearch.Target;
            if (unit.Attacker.CanAttack(target))
                unit.Attacker.Attack(target);
            else
                unit.Mover.MoveTo(target);
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