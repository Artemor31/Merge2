using System;
using System.Collections.Generic;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;

namespace CodeBase.Models
{
    public class GameplayModel
    {
        public event Action<GameState> StateChanged;
        
        private GameState _state;
        
        public List<Unit> EnemyUnits { get; private set; } = new();
        public List<Unit> PlayerUnits { get; private set; } = new();
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

        public void AddEnemy(Unit unit)
        {
            EnemyUnits.Add(unit);
        }
        
        public void AddAlly(Unit unit)
        {
            PlayerUnits.Add(unit);
        }
    }
}