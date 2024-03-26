using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using CodeBase.Services.StateMachine;

namespace CodeBase.Models
{
    public class GameplayModel
    {
        public event Action<GameState> StateChanged;
        
        public IReadOnlyList<Actor> EnemyUnits => _enemyUnits;
        public IReadOnlyList<Actor> PlayerUnits => _playerUnits;

        private readonly List<Actor> _playerUnits = new();
        private readonly List<Actor> _enemyUnits = new();
        private GameState _state;

        public GameState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                
                _state = value;
                StateChanged?.Invoke(_state);
            }
        }


        public void AddAlly(Actor actor)
        {
            _playerUnits.Add(actor);
        }

        public void AddEnemy(Actor actor)
        {
            _enemyUnits.Add(actor);
        }

        public void RemoveEnemies()
        {
            _enemyUnits.ForEach(UnityEngine.Object.Destroy);
            _enemyUnits.Clear();
        }
    }
}