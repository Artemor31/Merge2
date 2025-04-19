using System.Collections.Generic;
using System.Linq;
using Databases;
using Gameplay.Units;
using Infrastructure;
using Services.DataServices;
using Services.Infrastructure;
using UnityEngine;

namespace Services
{
    public class ActorRollService : IService
    {
        private const uint RollCount = 3;
        
        private WindowsService _windowsService;
        private UnitsDatabase _unitsDatabase;
        private PersistantDataService _persistantData;
        private GameFactory _gameFactory;
        
        public (List<ActorData> datas, List<Actor> actors) GenerateRoll()
        {
            var datas = CreateRoll();
            List<Actor> actors = CreateActors(datas);
            return (datas, actors);
        }

        public (ActorData, Actor) GenerateSingleUnit()
        {
            ActorData data = RandomOpened();
            return (data, _gameFactory.CreateEnemyActor(data, Vector3.zero));
        }

        private List<Actor> CreateActors(List<ActorData> datas) => 
            datas.Select(data => _gameFactory.CreateEnemyActor(data, Vector3.zero)).ToList();

        private List<ActorData> CreateRoll()
        {
            List<ActorData> data = new();
            for (int i = 0; i < RollCount; i++)
            {
                data.Add(RandomOpened());
            }

            return data;
        }

        private ActorData RandomOpened() => _unitsDatabase.ConfigsFor(1)
                                                            .Where(c => _persistantData.IsOpened(c.Data))
                                                            .Random().Data;
    }
}