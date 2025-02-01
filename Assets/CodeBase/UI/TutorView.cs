using System;
using Infrastructure;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorView : Presenter
    {
        public event Action Clicked;
        public Action PreviousAction;

        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public string Id2 { get; private set; }
        [field: SerializeField] public bool Is2D { get; private set; }
        public Transform Transform => transform;
        public RectTransform RectTransform { get; private set; }
        private TutorialService _tutorialService;


        public override void Init()
        {
            if (Is2D)
            {
                RectTransform = GetComponent<RectTransform>();
                GetComponent<Button>().onClick.AddListener(() => Clicked?.Invoke());
            }

            ServiceLocator.Resolve<TutorialService>().AddItem(this);
        }

        private void OnMouseDown() => Clicked?.Invoke();
    }
}