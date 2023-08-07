using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.Models;

namespace CodeBase.Services.StateMachine
{
    public class BattleProcessor
    {
        private readonly GameplayModel _gameplayModel;
        private readonly DatabaseProvider _databaseProvider;

        public BattleProcessor(GameplayModel gameplayModel, DatabaseProvider databaseProvider)
        {
            _gameplayModel = gameplayModel;
            _databaseProvider = databaseProvider;
            _gameplayModel.StateChanged += GameplayModelOnStateChanged;
        }

        private void GameplayModelOnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Waiting:
                    SetIdle();
                    break;  
                
                case GameState.Processing:
                    StartBattle();
                    break;
                
                case GameState.Shopping:
                    SetIdle();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void StartBattle()
        {
            FindBestTargets(_gameplayModel.PlayerUnits, _gameplayModel.EnemyUnits);
            FindBestTargets(_gameplayModel.EnemyUnits, _gameplayModel.PlayerUnits);
        }

        private void FindBestTargets(List<Unit> seekers, List<Unit> candidates)
        {
            foreach (var unit in seekers)
            {
                var brainsDatabase = _databaseProvider.GetDatabase<BrainsDatabase>();
                var brain = brainsDatabase.Brains.First(x => x.UnitId == unit.Id);
                var bestTarget = brain.Brain.BestTarget(candidates);
                unit.SetTarget(bestTarget);
            }
        }

        private void SetIdle()
        {
            var allUnits = _gameplayModel.EnemyUnits.Concat(_gameplayModel.PlayerUnits);
            foreach (var unit in allUnits)
            {
                unit.SetIdle();
            }

            return;
        }
    }
}