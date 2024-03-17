using System;
using System.Collections.Generic;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.Services.StateMachine;

namespace CodeBase.Models
{
    public class GameplayModel
    {
        public event Action<GameState> StateChanged;
        
        private GameState _state;
        
        public List<Actor> EnemyUnits { get; private set; } = new();
        public List<Actor> PlayerUnits { get; private set; } = new();
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

        public void AddEnemy(Actor actor)
        {
            EnemyUnits.Add(actor);
        }
        
        public void AddAlly(Actor actor)
        {
            PlayerUnits.Add(actor);
        }
    }
}