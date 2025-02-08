using Infrastructure;
using Services;
using Services.GridService;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class SellButton : Presenter, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Image _view;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _secondColor;
        private GridViewService _gridViewService;
        private TutorialService _tutorialService;

        public override void Init()
        {
            _gridViewService = ServiceLocator.Resolve<GridViewService>();
            _tutorialService = ServiceLocator.Resolve<TutorialService>();
        }

        public void Show(int cost)
        {
            gameObject.SetActive(true);
            _cost.text = "Продать за " + cost;
            _gridViewService.Selling = false;
            _view.color = _startColor;
        }

        private void Update()
        {
            float t = Mathf.PingPong(Time.time / 1, 2);
            _view.color = Color.Lerp(_startColor, _secondColor, t);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _gridViewService.Selling = false;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tutorialService.InTutor) return;
            _gridViewService.Selling = true;
        }

        public void OnPointerExit(PointerEventData eventData) => _gridViewService.Selling = false;
    }
}