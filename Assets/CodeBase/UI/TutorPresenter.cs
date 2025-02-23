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
using UI.Tutor;
using UI.UpgradeWindow;
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

        public override void OnShow()
        {
            foreach (Button button in _windowService.Buttons)
            {
                button.interactable = _startTutor.gameObject == button.gameObject || 
                                      _endTutor.gameObject == button.gameObject;
            }
            
            _startPanel.SetActive(true);
        }

        private void Cancel()
        {
            _startPanel.SetActive(false);
            FreeAllButtons();
        }

        private void BlockAllButtonsBut(TutorView view)
        {
            foreach (Button button in _windowService.Buttons)
                button.interactable = view.gameObject == button.gameObject;
        }
        
        private void FreeAllButtons()
        {
            foreach (Button button in _windowService.Buttons)
                button.interactable = true;
        }

        private void HighlightObject(string id)
        {
            TutorView currentView = _tutorialService.GetItem(id);
            _finger.PointTo(currentView);
            BlockAllButtonsBut(currentView);
        }

        private void DoThenStateIs(Type state, Action action) => StartCoroutine(AwaitState(state, action));

        private IEnumerator AwaitState(Type state, Action action)
        {
            while (_gameStateMachine.Current.GetType() != state)
            {
                yield return _waitHalfSecond;
            }

            action.Invoke();
        }

        private void AwaitClicked(string id, Action action)
        {
            TutorView currentView = _tutorialService.GetItem(id);
            currentView.AddOneShotHandler(action);
        }

        private void AwaitClickedAndHighlight(string id, Action action)
        {
            HighlightObject(id);
            AwaitClicked(id, action);
        }

        private void Step1_OnAcceptClicked()
        {
            _tutorialService.InTutor = true;
            _startPanel.SetActive(false);
            FreeAllButtons();
            AwaitClickedAndHighlight("MenuFight", Step2_StartLevelClicked);
            _text.ShowText(0);
        }

        private void Step2_StartLevelClicked()
        {
            StartCoroutine(Step3_LevelLoaded());
        }

        private IEnumerator Step3_LevelLoaded()
        {
            _finger.Disable();
            _text.Hide();
            TutorView shopButton = _tutorialService.GetItem("GameplayShop");
            BlockAllButtonsBut(shopButton);
            yield return _waitHalfSecond;
            _finger.PointTo(shopButton);
            _text.ShowText(1);
            _logicService.OnPlayerFieldChanged += ShowBuffButton;
        }

        private void ShowBuffButton()
        {
            if (_logicService.PlayerUnits.Count < 2) return;
            
            _logicService.OnPlayerFieldChanged -= ShowBuffButton;
            AwaitClickedAndHighlight("BuffButton", HideBuffButton);
            _text.ShowText(12);
        }

        private void HideBuffButton()
        {
            AwaitClickedAndHighlight("BuffButton", Step4_2WarriorsBought);
            _text.ShowText(13);
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
                _text.ShowText(2);
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
            DoThenStateIs(typeof(ResultScreenState), () => StartCoroutine(AwaitViewLoad()));
        }

        private IEnumerator AwaitViewLoad()
        {
            yield return _waitHalfSecond;
            yield return _waitHalfSecond;
            yield return _waitHalfSecond;
            _text.ShowText(4, TextAllignment.Bottom);
            AwaitClickedAndHighlight("WinNext", Step8_Level2StartLoad);
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
            _tutorialService.InTutor = false;
            DoThenStateIs(typeof(ResultScreenState), () => StartCoroutine(AwaitViewLoad2()));
        }

        private IEnumerator AwaitViewLoad2()
        {
            yield return _waitHalfSecond;
            yield return _waitHalfSecond;
            yield return _waitHalfSecond;
            _text.ShowText(6, TextAllignment.Bottom);
            AwaitClickedAndHighlight("LoseNext", Step13_MenuLoaded);
        }

        private void Step13_MenuLoaded()
        {
            _text.ShowText(7);
            AwaitClickedAndHighlight("BottmShop", Step14);
        }

        private void Step14()
        {
            _text.ShowText(8, TextAllignment.Bottom);
            AwaitClickedAndHighlight("BuyChest", Step15_ShopTabOpened);
        }

        private void Step15_ShopTabOpened()
        {
            _text.ShowText(9, TextAllignment.Bottom);
            AwaitClickedAndHighlight("ConfirmChest", Step15_2);
        }
        
        private void Step15_2()
        {
            _text.ShowText(12, TextAllignment.Bottom);
            AwaitClickedAndHighlight("BuyGrid", Step15_2_ShopChestTabOpened);
        }

        private void Step15_2_ShopChestTabOpened()
        {
            _text.ShowText(10);
            AwaitClickedAndHighlight("BottomUpgrade", Step16_UnitUnlocked);
        }

        private void Step16_UnitUnlocked()
        {
            StartCoroutine(PointToButton());
        }

        private IEnumerator PointToButton()
        {
            _finger.Disable();
            _text.ShowText(11);
            yield return _waitHalfSecond;

            UpgradeShopPresenter upgradeShopPresenter = _windowService.Get<UpgradeShopPresenter>();
            UpgradeItemPresenter[] upgradeItemPresenters = upgradeShopPresenter.GetComponentsInChildren<UpgradeItemPresenter>();
            Button selected = upgradeItemPresenters[0].GetComponentInChildren<Button>();
            RectTransform rectTransform = selected.GetComponent<RectTransform>();
            
            _finger.PointTo(rectTransform);
            
            foreach (Button button in _windowService.Buttons)
                button.interactable = selected.gameObject == button.gameObject;
            
            selected.onClick.AddListener(CloseAll);
        }

        private void CloseAll()
        {
            _text.Hide();
            _finger.Disable();
            FreeAllButtons();
            
            _windowService.Get<UpgradeShopPresenter>()
                          .GetComponentsInChildren<UpgradeItemPresenter>()[0]
                          .GetComponentInChildren<Button>()
                          .onClick.RemoveListener(CloseAll);
        }
    }
}