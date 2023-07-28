using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI
{
    public class WindowsService : MonoBehaviour, IService
    {
        [SerializeField] private List<Window> _windows;

        public void Show<T>() where T : Window
        {
            _windows.First(w => w.GetType() == typeof(T))
                    .gameObject
                    .SetActive(true);
        }
    }
}