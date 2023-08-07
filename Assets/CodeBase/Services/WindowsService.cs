using System.Collections.Generic;
using System.Linq;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services
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