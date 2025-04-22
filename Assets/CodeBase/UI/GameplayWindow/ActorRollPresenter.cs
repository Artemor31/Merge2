using System.Collections.Generic;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _crowns;
        [SerializeField] private TextMeshProUGUI _reRollCostText;
        [SerializeField] private Button _reRoll;
        
        private ActorRollService _service;
        private GameplayDataService _gameplayDataService;
        private WindowsService _windowService;
        private CameraService _cameraService;
        private int _reRollCost;

        public override void Init()
        {
            _service = ServiceLocator.Resolve<ActorRollService>();
            _gameplayDataService = ServiceLocator.Resolve<GameplayDataService>();
            _windowService = ServiceLocator.Resolve<WindowsService>();
            _cameraService = ServiceLocator.Resolve<CameraService>();
            _closeButton.onClick.AddListener(Close);
            _reRoll.onClick.AddListener(ReRoll);
            _gameplayDataService.Crowns.AddListener(_crowns);
            DontDestroyOnLoad(this);

            foreach (ActorRollView view in _actorRollView)
            {
                view.Init();
            }
            _adsUnit.Init();
        }

        public void Setup()
        {
            _canvas.worldCamera = _cameraService.CurrentCamera();
            _reRollCost = 1;
            _reRollCostText.text = _reRollCost.ToString();
            
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

        private void ReRoll()
        {
            if (_service.TryReRoll(_reRollCost))
            {
                _reRollCost += 1;
                _reRollCostText.text = _reRollCost.ToString();
            }
        }

        private void Close()
        {
            _windowService.Show<GameplayPresenter>();
            _windowService.Close<ActorRollPresenter>();
        }
    }
}