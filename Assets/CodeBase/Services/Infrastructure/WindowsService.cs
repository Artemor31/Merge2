using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Infrastructure
{
    public class WindowsService : MonoBehaviour, IService
    {
        [SerializeField] private List<Presenter> _windows;
        public List<Button> Buttons;
        private readonly Dictionary<Type, Presenter> _cache = new();

        public void InitWindows()
        {
            foreach (Presenter presenter in _windows)
            {
                presenter.Init();
            }
        }

        [Sirenix.OdinInspector.Button]
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
        
        [Sirenix.OdinInspector.Button]
        public void FindButtons()
        {
            Buttons.Clear();

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = transform.GetChild(i);
                List<Button> presenters = child.GetComponentsInChildren<Button>(true).ToList();
                Buttons.AddRange(presenters);
            }
        }

        public T Show<T>() where T : Presenter
        {
            Presenter presenter = _windows.First(w => w.GetType() == typeof(T));
            presenter.OnShow();
            presenter.gameObject.SetActive(true);
            return (T)presenter;
        }

        public void Show<TWindow, TData>(TData data) where TWindow : Presenter where TData : WindowData
        {
            Presenter presenter = _windows.First(w => w.GetType() == typeof(TWindow));
            presenter.OnShow();

            if (presenter is IWindowDataReceiver<TData> receiver)
            {
                receiver.SetData(data);
            }
            
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