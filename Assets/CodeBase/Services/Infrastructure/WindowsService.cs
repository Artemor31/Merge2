using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UI;
using UI.ResultWindow;
using UnityEngine;

namespace Services.Infrastructure
{
    public class WindowsService : MonoBehaviour, IService
    {
        [SerializeField] private List<Presenter> _windows;
        private readonly Dictionary<Type, Presenter> _cache = new();

        public void InitWindows()
        {
            foreach (Presenter presenter in _windows)
            {
                presenter.Init();
            }
        }

        [Button]
        public void FindPresenters()
        {
            _windows.Clear();

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = transform.GetChild(i);
                List<Presenter> presenters = child.GetComponentsInChildren<Presenter>(true).ToList();
                _windows.AddRange(presenters);
            }
        }

        public void Show<T>() where T : Presenter
        {
            Presenter presenter = _windows.First(w => w.GetType() == typeof(T));
            presenter.OnShow();
            presenter.gameObject.SetActive(true);
        }

        public void Show<TWindow, TData>(TData data) where TWindow : Presenter where TData : WindowData
        {
            Presenter presenter = _windows.First(w => w.GetType() == typeof(TWindow));
            presenter.OnShow();
            presenter.SetData<ResultData>(data);
            presenter.gameObject.SetActive(true);
        }

        public void Close<T>()
        {
            Presenter presenter = _windows.First(w => w.GetType() == typeof(T));
            presenter.OnHide();
            presenter.gameObject.SetActive(false);
        }
        
        public T Get<T>() where T : Presenter
        {
            Type type = typeof(T);
            if (_cache.TryGetValue(type, out Presenter value))
            {
                return (T)value;
            }
            
            T presenter = (T)_windows.First(w => w is T);
            _cache.Add(type, presenter);
            return presenter;
        }
    }
}