using System.Linq;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.Models;

namespace CodeBase.Services.StateMachine
{
    public class BattleConductor
    {
        private readonly GameplayModel _gameplayModel;
        private readonly IUpdateable _updateable;

        private bool _battling;

        public BattleConductor(GameplayModel gameplayModel, IUpdateable updateable)
        {
            _gameplayModel = gameplayModel;
            _gameplayModel.StateChanged += GameplayModelOnStateChanged;
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

        private void StartBattle()
        {
            var enemies = _gameplayModel.EnemyUnits;
            foreach (var unit in enemies)
            {
                var targetSearch = unit.GetComponent<ITargetSearch>();
                targetSearch.Construct(_gameplayModel.PlayerUnits);
                targetSearch.SearchTarget();
                
                
            }
        }

        private void SetUnitsIdle()
        {
        }
    }
}