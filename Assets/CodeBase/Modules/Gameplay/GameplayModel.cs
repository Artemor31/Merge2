using System.Collections.Generic;
using CodeBase.Units;
using System;

namespace CodeBase.Modules.Gameplay
{
    public class GameplayModel : IModel
    {
        public event Action<GameState> StateChanged; 
        
        public GameState State
        {
            set
            {
                if (_state != value)
                {
                    _state = value;
                    StateChanged?.Invoke(_state);
                }
            }
        }

        private GameState _state;
        public List<Unit> EnemyUnits { get; set; }
        public List<Unit> PlayerUnits { get; set; }
        public int CurrentWave { get; set; }
    }
}