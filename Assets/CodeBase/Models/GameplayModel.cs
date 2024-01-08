using System;
using System.Collections.Generic;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.UI.GameplayWindow;

namespace CodeBase.Models
{
    public class GameplayModel
    {
        public event Action<GameState> StateChanged;
        public int CurrentWave { get; set; }
        public int Money { get; set; }
        
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

        public List<UnitCell> PlayerCells { get; set; } = new();
        public List<Unit> EnemyUnits { get; set; } = new();
        public List<Unit> PlayerUnits { get; set; } = new();
        public List<UnitCard> PlayerCards { get; set; } = new();
    }
}