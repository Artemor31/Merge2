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
        private RollData _roll;

        public ActorRollService(UnitsDatabase unitsDatabase, PersistantDataService persistantData, 
                                GameFactory gameFactory, GameStateMachine gameStateMachine)
        {
            _unitsDatabase = unitsDatabase;
            _persistantData = persistantData;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
            _gameStateMachine.OnStateChanged += StateChanged;
        }

        public RollData GetRoll() => _roll ??= CreateRoll();

        private RollData CreateRoll()
        {
            List<Actor> data = new();
            for (int i = 0; i < RollCount; i++)
            {
                ActorData randomOpened = RandomOpened();
                Actor actor = _gameFactory.CreateActorView(randomOpened);
                data.Add(actor);
            }

            return new RollData(data, _gameFactory.CreateActorView(RandomOpened()));
        }

        private void StateChanged(IState state)
        {
            if (state.GetType() == typeof(SetupLevelState))
            {
                _roll = null;
            }
        }

        private ActorData RandomOpened() => _unitsDatabase.ConfigsFor(1)
                                                          .Where(c => _persistantData.IsOpened(c.Data))
                                                          .Random().Data;
    }
}