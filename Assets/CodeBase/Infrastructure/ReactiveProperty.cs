using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure
{
    public class ReactiveProperty<T>
    {
        public event Action<T> Changed;
        private readonly List<Object> _listeners = new();
        private T _value;

        public ReactiveProperty(T value) => _value = value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                foreach (Object listener in _listeners)
                {
                    Update(listener);
                }

                Changed?.Invoke(_value);
            }
        }

        public void AddListener<TListener>(TListener listener, bool update = true) where TListener : Object
        {
            _listeners.Add(listener);
            if (update)
            {
                Update(listener);
            }
        }

        public void RemoveListener<TListener>(TListener listener) where TListener : Object
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        private void Update<TListener>(TListener listener) where TListener : Object
        {
            switch (listener)
            {
                case TextMeshProUGUI textMeshPro:
                    textMeshPro.text = Value.ToString();
                    break;
                case TMP_Text tmp:
                    tmp.text = Value.ToString();
                    break;
                default:
                    Debug.LogError("Cannot notify type = " + listener.GetType());
                    break;
            }
        }
    }
}