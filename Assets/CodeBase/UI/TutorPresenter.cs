using System.Collections;
using System.Collections.Generic;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.GridService;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorPresenter : Presenter
    {
        private const string SavePath = "TutorData";

        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _blackPanel;
        [SerializeField] private FingerPresenter _finger;
        [SerializeField] private Button _startTutor;
        [SerializeField] private Button _endTutor;

        private WindowsService _windowService;
        private Coroutine _current;
        
        public override void Init() => _windowService = ServiceLocator.Resolve<WindowsService>();
        public override void OnShow() => StartTutor();
        private void Cancel() => _startPanel.SetActive(false);

        private void StartTutor()
        {
            _startPanel.SetActive(true);
            _startTutor.onClick.AddListener(Step_ClickFight);
            _endTutor.onClick.AddListener(Cancel);
        }

        private void PointFinger(TutorView currentView)
        {
            _finger.gameObject.SetActive(true);
            _finger.PointTo(currentView);
            BlockAllButtonsBut(currentView);
        }
        
        private void BlockAllButtonsBut(TutorView view)
        {
            foreach (Button button in _windowService.Buttons) 
                button.interactable = view.gameObject == button.gameObject;
        }
        
        private void ViewClicked(TutorView view)
        {
            view.Clicked -= ViewClicked;

            if (_current != null)
            {
                StopCoroutine(_current);
            }

            _current = StartCoroutine(StartStep(view.Id + 1));
        }

        private IEnumerator StartStep(int id)
        {
            switch (id)
            {
                case 1:
                    Step_ClickFight();
                    break;
                case 2:
                    _finger.Disable();
                    TutorView currentView = TutorialService.Instance.GetItem(2);
                    BlockAllButtonsBut(currentView);
                    yield return new WaitForSeconds(2);
                    Step_PointOnBuy(currentView);
                    break;
                case 3:
                {
                    _finger.Disable();
                    StartStep3();
                    break;
                }
                case 4:
                    StartStep4();
                    break;
                case 5:
                    StartStep5();
                    break;
                case 6:
                    StartStep6();
                    break;
            }
        }

        private void Step_ClickFight()
        {
            _startPanel.SetActive(false);
            TutorView currentView = TutorialService.Instance.GetItem(1);
            PointFinger(currentView);
            currentView.Clicked += ViewClicked;
        }

        private void Step_PointOnBuy(TutorView currentView)
        {
            _finger.Active();
            _finger.PointTo(currentView);
            ServiceLocator.Resolve<GridLogicService>().OnPlayerFieldChanged += WaitForTwoUnits;
        }

        private void WaitForTwoUnits()
        {
            GridLogicService logicService = ServiceLocator.Resolve<GridLogicService>();
            List<Actor> units = logicService.PlayerUnits;
            if (units.Count == 2)
            {
                logicService.OnPlayerFieldChanged -= WaitForTwoUnits;
                TutorView currentView = units[0].gameObject.AddComponent<TutorView>();
                TutorView currentView2 = units[1].gameObject.AddComponent<TutorView>();
                _finger.MoveBetween(currentView, currentView2);
            }
        }

        private void StartStep3()
        {
        }

        private void StartStep4()
        {
        }

        private void StartStep5()
        {
        }

        private void StartStep6()
        {
        }
    }
}