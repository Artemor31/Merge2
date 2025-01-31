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
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _text;

        private readonly WaitForSeconds _waitHalfSecond = new(0.5f);
        private WindowsService _windowService;
        private Coroutine _current;
        private GridLogicService _logicService;

        public override void Init() => _windowService = ServiceLocator.Resolve<WindowsService>();
        public override void OnShow() => StartTutor();
        private void Cancel()
        {
            _startPanel.SetActive(false);
            TutorialService.Instance.EndedTutor = true;
            TutorialService.Instance.NeedTutor = false;
        }

        private void BlockAllButtonsBut(TutorView view)
        {
            foreach (Button button in _windowService.Buttons)
                button.interactable = view.gameObject == button.gameObject;
        }

        private void StartTutor()
        {
            _startPanel.SetActive(true);
            _startTutor.onClick.AddListener(Step1_OnAcceptClicked);
            _endTutor.onClick.AddListener(Cancel);
        }

        private void Step1_OnAcceptClicked()
        {
            TutorialService.Instance.NeedTutor = false;
            _startPanel.SetActive(false);
            TutorView currentView = TutorialService.Instance.GetItem("MenuFight");
            _finger.PointTo(currentView);
            BlockAllButtonsBut(currentView);
            currentView.Clicked += Step2_StartLevelClicked;
        }

        private void Step2_StartLevelClicked(TutorView view)
        {
            view.Clicked -= Step2_StartLevelClicked;

            if (view.Id == 1)
            {
                _current = StartCoroutine(Step3_LevelLoaded());
            }
            else
            {
                throw new Exception();
            }
        }

        private IEnumerator Step3_LevelLoaded()
        {
            _finger.Disable();
            TutorView shopButton = TutorialService.Instance.GetItem("GameplayShop");
            BlockAllButtonsBut(shopButton);
            yield return _waitHalfSecond;
            yield return _waitHalfSecond;
            _finger.PointTo(shopButton);
            ServiceLocator.Resolve<GridLogicService>().OnPlayerFieldChanged += Step4_2WarriorsBought;
        }

        private void Step4_2WarriorsBought()
        {
            StopCoroutine(_current);
            _logicService = ServiceLocator.Resolve<GridLogicService>();
            List<Actor> units = _logicService.PlayerUnits;
            if (units.Count == 2)
            {
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
                var view = TutorialService.Instance.GetItem("GameplayFight");
                BlockAllButtonsBut(view);
                _finger.PointTo(view);
                StartCoroutine(Step6_FightClicked());
            }
        }

        private IEnumerator Step6_FightClicked()
        {
            GameStateMachine resolve = ServiceLocator.Resolve<GameStateMachine>();
            Type type = typeof(GameLoopState);

            while (resolve.Current.GetType() != type)
            {
                yield return _waitHalfSecond;
            }

            _finger.Disable();
            StopCoroutine(_current);
            StartCoroutine(Step7_FightEnded());
        }

        private IEnumerator Step7_FightEnded()
        {
            GameStateMachine resolve = ServiceLocator.Resolve<GameStateMachine>();
            Type type = typeof(ResultScreenState);
            TutorView nextLeevelButton = TutorialService.Instance.GetItem("WinNext");
            BlockAllButtonsBut(nextLeevelButton);

            while (resolve.Current.GetType() != type)
            {
                yield return _waitHalfSecond;
            }

            _finger.PointTo(nextLeevelButton);
            StopCoroutine(_current);
            nextLeevelButton.Clicked += Step8_Level2StartLoad;
        }

        private void Step8_Level2StartLoad(TutorView nextLeevelButton)
        {
            nextLeevelButton.Clicked -= Step8_Level2StartLoad;
            StartCoroutine(Step9_Level2Loaded());
        }

        private IEnumerator Step9_Level2Loaded()
        {
            _finger.Disable();
            GameStateMachine resolve = ServiceLocator.Resolve<GameStateMachine>();
            Type type = typeof(SetupLevelState);

            while (resolve.Current.GetType() != type)
            {
                yield return _waitHalfSecond;
            }

            TutorView fightButton = TutorialService.Instance.GetItem("GameplayFight");
            _finger.PointTo(fightButton);
            BlockAllButtonsBut(fightButton);
            StopCoroutine(_current);
            fightButton.Clicked += Step10_Level2Started;
        }

        private void Step10_Level2Started(TutorView fightButton)
        {
            fightButton.Clicked -= Step10_Level2Started;
            StartCoroutine(Step11_FightClickedTwo());
        }
        
        private IEnumerator Step11_FightClickedTwo()
        {
            GameStateMachine resolve = ServiceLocator.Resolve<GameStateMachine>();
            Type type = typeof(GameLoopState);

            while (resolve.Current.GetType() != type)
            {
                yield return _waitHalfSecond;
            }

            _finger.Disable();
            StopCoroutine(_current);
            StartCoroutine(Step12_WaitLose());
        }

        private IEnumerator Step12_WaitLose()
        {
            GameStateMachine resolve = ServiceLocator.Resolve<GameStateMachine>();
            Type type = typeof(ResultScreenState);
            TutorView nextLevelButton = TutorialService.Instance.GetItem("LoseNext");
            BlockAllButtonsBut(nextLevelButton);

            while (resolve.Current.GetType() != type)
            {
                yield return _waitHalfSecond;
            }

            _finger.PointTo(nextLevelButton);
            StopCoroutine(_current);
            nextLevelButton.Clicked += Step13_MenuLoaded;
        }

        private void Step13_MenuLoaded(TutorView nextLevelButton)
        {
            nextLevelButton.Clicked -= Step13_MenuLoaded;
            TutorView shopButton = TutorialService.Instance.GetItem("BottmInfo");
            BlockAllButtonsBut(shopButton);
            _finger.PointTo(shopButton);
            shopButton.Clicked += Step14;
        }

        private void Step14(TutorView obj)
        {
            obj.Clicked -= Step14;
            TutorView shopButton = TutorialService.Instance.GetItem("BuyChest");
            BlockAllButtonsBut(shopButton);
            _finger.PointTo(shopButton);
            shopButton.Clicked += Step15_ShopTabOpened;
        }

        private void Step15_ShopTabOpened(TutorView shopTab)
        {
            shopTab.Clicked -= Step15_ShopTabOpened;
            TutorView shopButton = TutorialService.Instance.GetItem("ConfirmChest");
            BlockAllButtonsBut(shopButton);
            _finger.PointTo(shopButton);
            shopButton.Clicked += Step15_2_ShopChestTabOpened;
        }
        
        private void Step15_2_ShopChestTabOpened(TutorView shopTab)
        {
            shopTab.Clicked -= Step15_2_ShopChestTabOpened;
            TutorView shopButton = TutorialService.Instance.GetItem("BottomUpgrade");
            BlockAllButtonsBut(shopButton);
            _finger.PointTo(shopButton);
            shopButton.Clicked += Step16_UnitUnlocked;
        }
        
        private void Step16_UnitUnlocked(TutorView shopButton)
        {
            _finger.Disable();
            return;
            shopButton.Clicked -= Step16_UnitUnlocked;
            TutorView upgradeTab = TutorialService.Instance.GetItem("");
            BlockAllButtonsBut(upgradeTab);
            _finger.PointTo(upgradeTab);
            upgradeTab.Clicked += Step17_UnitUnlocked;
        }
        
        // beg for 5 start review
        private void Step17_UnitUnlocked(TutorView upgradeTab)
        {
            upgradeTab.Clicked -= Step17_UnitUnlocked;
        }
    }
}