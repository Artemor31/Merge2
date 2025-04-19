using System.Collections.Generic;
using Databases;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.DataServices;
using UnityEngine;

namespace UI.GameplayWindow
{
    public class ActorRollPresenter : Presenter
    {
        [SerializeField] private List<ActorRollView> _actorRollView;
        [SerializeField] private ActorRollView _adsUnit;
        private ActorRollService _service;
        private GameplayDataService _gameplayDataService;

        public override void Init()
        {
            _service = ServiceLocator.Resolve<ActorRollService>();
            _gameplayDataService = ServiceLocator.Resolve<GameplayDataService>();
        }

        public override void OnShow()
        {
            (List<ActorData> datas, List<Actor> actors) = _service.GenerateRoll();
            for (int i = 0; i < _actorRollView.Count; i++)
            {
                _actorRollView[i].SetData(datas[i], actors[i]);
            }

            if (_gameplayDataService.Wave % 3 == 0)
            {
                var adsUnit = _service.GenerateSingleUnit();
                _adsUnit.SetData(adsUnit.Item1, adsUnit.Item2);
            }
        }
    }
}