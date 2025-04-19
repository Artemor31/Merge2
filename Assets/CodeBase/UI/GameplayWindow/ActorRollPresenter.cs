using System.Collections.Generic;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ActorRollPresenter : Presenter
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private List<ActorRollView> _actorRollView;
        [SerializeField] private ActorRollView _adsUnit;
        [SerializeField] private Canvas _canvas;
        
        private ActorRollService _service;
        private GameplayDataService _gameplayDataService;
        private WindowsService _windowService;
        private CameraService _cameraService;

        public override void Init()
        {
            _service = ServiceLocator.Resolve<ActorRollService>();
            _gameplayDataService = ServiceLocator.Resolve<GameplayDataService>();
            _windowService = ServiceLocator.Resolve<WindowsService>();
            _cameraService = ServiceLocator.Resolve<CameraService>();
            _closeButton.onClick.AddListener(Close);
            DontDestroyOnLoad(this);

            foreach (ActorRollView view in _actorRollView)
            {
                view.Init();
            }
            _adsUnit.Init();
        }

        public override void OnShow()
        {
            _canvas.worldCamera = _cameraService.CurrentCamera();
            
            RollData rollData = _service.GetRoll();
            for (int i = 0; i < _actorRollView.Count; i++)
            {
                _actorRollView[i].SetData(rollData.Actors[i]);
            }

            if (_gameplayDataService.Wave % 3 == 0)
            {
                _adsUnit.SetData(rollData.AdsActor);
            }
            else
            {
                _adsUnit.Hide();
            }
        }

        private void Close()
        {
            _windowService.Show<GameplayPresenter>();
            _windowService.Close<ActorRollPresenter>();
        }
    }
}