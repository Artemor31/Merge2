using System;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorView : Presenter
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public string Id2 { get; private set; }
        [field: SerializeField] public bool Is2D { get; private set; }

        public Transform Transform => transform;
        public RectTransform RectTransform { get; private set; }

        public event Action<TutorView> Clicked;

        public override void Init()
        {
            if (Is2D)
            {
                RectTransform = GetComponent<RectTransform>();
                GetComponent<Button>().onClick.AddListener(() => Clicked?.Invoke(this));
            }
            
            TutorialService.Instance.AddItem(this);
        }

        private void OnMouseDown() => Clicked?.Invoke(this);
    }
}