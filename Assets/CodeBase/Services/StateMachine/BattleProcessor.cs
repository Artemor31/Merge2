using System;
using System.Linq;
using CodeBase.Gameplay;
using CodeBase.Models;

namespace CodeBase.Services.StateMachine
{
    public class BattleProcessor
    {
        private readonly GameplayModel _gameplayModel;
        private readonly BattleBrainConductor _conductor;

        public BattleProcessor(GameplayModel gameplayModel, BattleBrainConductor conductor)
        {
            _gameplayModel = gameplayModel;
            _conductor = conductor;
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
                    _conductor.StartBattle();
                    break;

                case GameState.Shopping:
                    SetIdle();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void SetIdle()
        {
            var allUnits = _gameplayModel.EnemyUnits.Concat(_gameplayModel.PlayerUnits.Select(u => u.Unit).ToList());
            foreach (var unit in allUnits)
            {
                unit.SetIdle();
            }

            return;
        }
    }
}