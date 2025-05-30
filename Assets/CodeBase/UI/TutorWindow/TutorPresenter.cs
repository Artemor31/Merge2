using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TutorWindow
{
    public class TutorPresenter : Presenter
    {
        [SerializeField] private FingerPresenter _finger;
        [SerializeField] private TutorText _text;

        private readonly WaitForSeconds _waitHalfSecond = new(0.5f);
        private WindowsService _windowService;
        private GridService _logicService;
        private TutorialService _tutorialService;
        private GameStateMachine _gameStateMachine;

        public override void Init()
        {
            _windowService = ServiceLocator.Resolve<WindowsService>();
            _tutorialService = ServiceLocator.Resolve<TutorialService>();
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _logicService = ServiceLocator.Resolve<GridService>();
            _tutorialService.OnTutorRequested += StartTutorQueue;
        }
        
        private void StartTutorQueue()
        {
            // fix
            if (ServiceLocator.Resolve<GameplayDataService>().Coins.Value < 100)
            {
                ShowBuffButton();
            }
            else
            {
                TutorView shopButton = _tutorialService.GetItem("GameplayShop");
            
                _finger.Disable();
                _finger.PointTo(shopButton);
                _logicService.OnPlayerFieldChanged += ShowBuffButton;

                foreach (Button button in _windowService.Buttons)
                    button.interactable = shopButton.gameObject == button.gameObject;
            }
        }

        private void ShowBuffButton()
        {
            UnlockButtons();
            if (_logicService.PlayerUnits.Count < 2) return;
            
            _logicService.OnPlayerFieldChanged -= ShowBuffButton;
            AwaitClickedAndHighlight("BuffButton", HideBuffButton);
        }

        private void HideBuffButton()
        {
            AwaitClickedAndHighlight("BuffButton", Step4_2WarriorsBought);
        }

        private void Step4_2WarriorsBought()
        {
            List<Actor> units = _logicService.PlayerUnits;
            
            if (units.Count == 1)
            {
                Step5_WarriorsMerged();
            }
            else
            {
                TutorView currentView = units[0].gameObject.AddComponent<TutorView>();
                TutorView currentView2 = units[1].gameObject.AddComponent<TutorView>();
                _finger.MoveBetween(currentView, currentView2);
                _logicService.OnPlayerFieldChanged += Step5_WarriorsMerged;   
            }
        }

        private void Step5_WarriorsMerged()
        {
            UnlockButtons();
            if (_logicService.PlayerUnits.Any(u => u.Data.Level > 1))
            {
                _logicService.OnPlayerFieldChanged -= Step5_WarriorsMerged;
                AwaitClickedAndHighlight("GameplayFight", StartWaitForGameStart);
            }
        }
        
        private void StartWaitForGameStart() => StartCoroutine(WaitForGameStart());

        private IEnumerator WaitForGameStart()
        {
            _tutorialService.EndTutor();
            Type memberInfo = typeof(GameLoopState);
            while (_gameStateMachine.Current.GetType() != memberInfo)
            {
                yield return _waitHalfSecond;
            }
            
            _finger.Disable();
            UnlockButtons();
        }
        
        private void AwaitClickedAndHighlight(string id, Action action)
        {
            TutorView currentView1 = _tutorialService.GetItem(id);
            _finger.PointTo(currentView1);
            
            foreach (Button button in _windowService.Buttons)
                button.interactable = currentView1.gameObject == button.gameObject;
            
            TutorView currentView = _tutorialService.GetItem(id);
            currentView.AddOneShotHandler(action);
        }

        public void UnlockButtons()
        {
            foreach (Button button in _windowService.Buttons)
                button.interactable = true;
        }
    }
}