using System;
using Infrastructure;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TutorWindow
{
    public class TutorView : Presenter
    {
        public event Action Clicked;

        [field: SerializeField] public string Id2 { get; set; }
        [field: SerializeField] public bool Is2D { get; set; }
        public Transform Transform => transform;
        public RectTransform RectTransform { get; private set; }
        private TutorialService _tutorialService;
        private Action _action;

        public override void Init()
        {
            if (Is2D)
            {
                RectTransform = GetComponent<RectTransform>();
                GetComponent<Button>().onClick.AddListener(() => Clicked?.Invoke());
            }

            ServiceLocator.Resolve<TutorialService>().AddItem(this);
        }

        public void AddOneShotHandler(Action action)
        {
            _action = action;
            Clicked += InvokeAction;
        }

        private void InvokeAction()
        {
            Clicked -= InvokeAction;
            _action?.Invoke();
        }

        private void OnMouseDown() => Clicked?.Invoke();
    }
}