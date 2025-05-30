using System.Collections.Generic;
using System.Linq;
using Databases;
using Gameplay.Units;
using Infrastructure;
using Services.DataServices;
using Services.Infrastructure;
using Services.StateMachine;

namespace Services
{
    public class RollData
    {
        public readonly List<Actor> Actors;
        public readonly Actor AdsActor;
        public RollData(List<Actor> actors, Actor adsActor)
        {
            Actors = actors;
            AdsActor = adsActor;
        }
    }
    
    public class ActorRollService : IService
    {
        private const uint RollCount = 3;
        
        private readonly UnitsDatabase _unitsDatabase;
        private readonly PersistantDataService _persistantData;
        private readonly GameFactory _gameFactory;
        private readonly GameStateMachine _gameStateMachine;
        private readonly GameplayDataService _gameplayDataService;
        private RollData _roll;

        public ActorRollService(UnitsDatabase unitsDatabase, PersistantDataService persistantData, 
                                GameFactory gameFactory, GameStateMachine gameStateMachine, GameplayDataService gameplayDataService)
        {
            _unitsDatabase = unitsDatabase;
            _persistantData = persistantData;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
            _gameplayDataService = gameplayDataService;
            _gameStateMachine.OnStateChanged += StateChanged;
        }

        public RollData GetRoll() => _roll ??= CreateRoll();

        public bool TryReRoll(int cost)
        {
            if (_gameplayDataService.Coins.Value >= cost)
            {
                _gameplayDataService.Coins.Value -= cost;
                _roll = CreateRoll();
                return true;
            }

            return false;
        }

        private RollData CreateRoll()
        {
            List<Actor> data = new();
            for (int i = 0; i < RollCount; i++)
            {
                ActorData randomOpened = RandomOpened();
                Actor actor = _gameFactory.CreateActorView(randomOpened);
                data.Add(actor);
            }

            Actor adsActor = _gameplayDataService.Wave % 3 == 0 ? _gameFactory.CreateActorView(RandomOpened()) : null;
            
            return new RollData(data, adsActor);
        }

        private void StateChanged(IState state)
        {
            if (state.GetType() == typeof(LoadLevelState))
            {
                _roll = null;
            }
        }

        private ActorData RandomOpened() => _unitsDatabase.ConfigsFor(1)
                                                          .Where(c => _persistantData.IsOpened(c.Data))
                                                          .Random().Data;
    }
}