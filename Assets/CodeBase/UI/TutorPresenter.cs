using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.GridService;
using Services.Infrastructure;
using Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorPresenter : Presenter
    {
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private FingerPresenter _finger;
        [SerializeField] private Button _startTutor;
        [SerializeField] private Button _endTutor;
        [SerializeField] private TutorText _text;

        private readonly WaitForSeconds _waitHalfSecond = new(0.5f);
        private WindowsService _windowService;
        private Coroutine _current;
        private GridLogicService _logicService;
        private TutorialService _tutorialService;
        private GameStateMachine _gameStateMachine;

        public override void Init()
        {
            _windowService = ServiceLocator.Resolve<WindowsService>();
            _tutorialService = ServiceLocator.Resolve<TutorialService>();
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _logicService = ServiceLocator.Resolve<GridLogicService>();
            
            _startTutor.onClick.AddListener(Step1_OnAcceptClicked);
            _endTutor.onClick.AddListener(Cancel);
        }

        public override void OnShow() => _startPanel.SetActive(true);

        private void Cancel()
        {
            _startPanel.SetActive(false);
            _tutorialService.EndedTutor = true;
            _tutorialService.NeedTutor = false;
        }

        private void BlockAllButtonsBut(TutorView view)
        {
            foreach (Button button in _windowService.Buttons)
                button.interactable = view.gameObject == button.gameObject;
        }

        private void HighlightObject(string id)
        {
            TutorView currentView = _tutorialService.GetItem(id);
            _finger.PointTo(currentView);
            BlockAllButtonsBut(currentView);
        }

        private void DoThenStateIs(Type state, Action action) => _current = StartCoroutine(AwaitState(state, action));

        private IEnumerator AwaitState(Type state, Action action)
        {
            while (_gameStateMachine.Current.GetType() != state)
            {
                yield return _waitHalfSecond;
            }

            if (_current != null)
                StopCoroutine(_current);

            action.Invoke();
        }

        private void AwaitClicked(string id, Action action)
        {
            TutorView currentView = _tutorialService.GetItem(id);
            currentView.Clicked += action;
            currentView.Clicked -= currentView.PreviousAction;
        }

        private void AwaitClickedAndHighlight(string id, Action action)
        {
            HighlightObject(id);
            AwaitClicked(id, action);
        }

        private void Step1_OnAcceptClicked()
        {
            _tutorialService.NeedTutor = false;
            _startPanel.SetActive(false);
            AwaitClickedAndHighlight("MenuFight", Step2_StartLevelClicked);
            _text.ShowText(0);
        }

        private void Step2_StartLevelClicked() =>
            _current = StartCoroutine(Step3_LevelLoaded());

        private IEnumerator Step3_LevelLoaded()
        {
            _finger.Disable();
            _text.Hide();
            TutorView shopButton = _tutorialService.GetItem("GameplayShop");
            BlockAllButtonsBut(shopButton);
            yield return _waitHalfSecond;
            _finger.PointTo(shopButton);
            _text.ShowText(1);
            _logicService.OnPlayerFieldChanged += Step4_2WarriorsBought;
        }

        private void Step4_2WarriorsBought()
        {
            StopCoroutine(_current);
            List<Actor> units = _logicService.PlayerUnits;
            if (units.Count == 2)
            {
                _text.ShowText(2);
                _logicService.OnPlayerFieldChanged -= Step4_2WarriorsBought;
                TutorView currentView = units[0].gameObject.AddComponent<TutorView>();
                TutorView currentView2 = units[1].gameObject.AddComponent<TutorView>();
                _finger.MoveBetween(currentView, currentView2);
                _logicService.OnPlayerFieldChanged += Step5_WarriorsMerged;
            }
        }

        private void Step5_WarriorsMerged()
        {
            if (_logicService.PlayerUnits.Any(u => u.Data.Level > 1))
            {
                _logicService.OnPlayerFieldChanged -= Step5_WarriorsMerged;
                HighlightObject("GameplayFight");
                _text.ShowText(3);

                DoThenStateIs(typeof(GameLoopState), () =>
                {
                    _text.Hide();
                    _finger.Disable();
                    Step7_FightEnded();
                });
            }
        }

        private void Step7_FightEnded()
        {
            DoThenStateIs(typeof(ResultScreenState), () =>
            {
                _text.ShowText(4);
                AwaitClickedAndHighlight("WinNext", Step8_Level2StartLoad);
            });
        }

        private void Step8_Level2StartLoad()
        {
            _text.Hide();
            _finger.Disable();
            DoThenStateIs(typeof(SetupLevelState), () =>
            {
                _text.ShowText(5);
                AwaitClickedAndHighlight("GameplayFight", Step10_Level2Started);
            });
        }

        private void Step10_Level2Started()
        {
            _finger.Disable();
            _text.Hide();
            DoThenStateIs(typeof(GameLoopState), Step12_WaitLose);
        }

        private void Step12_WaitLose()
        {
            DoThenStateIs(typeof(ResultScreenState), () =>
            {
                _text.ShowText(6);
                AwaitClickedAndHighlight("LoseNext", Step13_MenuLoaded);
            });
        }

        private void Step13_MenuLoaded()
        {
            _text.ShowText(7);
            AwaitClickedAndHighlight("BottmInfo", Step14);
        }

        private void Step14()
        {
            _text.ShowText(8);
            AwaitClickedAndHighlight("BuyChest", Step15_ShopTabOpened);
        }

        private void Step15_ShopTabOpened()
        {
            _text.ShowText(9);
            AwaitClickedAndHighlight("ConfirmChest", Step15_2_ShopChestTabOpened);
        }

        private void Step15_2_ShopChestTabOpened()
        {
            _text.ShowText(10);
            AwaitClickedAndHighlight("BottomUpgrade", Step16_UnitUnlocked);
        }

        private void Step16_UnitUnlocked()
        {
            // beg for 5 start review
            
            _text.ShowText(11);
            _finger.Disable();

            foreach (Button button in _windowService.Buttons)
                button.interactable = true;
        }
    }
}