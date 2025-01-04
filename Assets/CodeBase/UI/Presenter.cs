using UnityEngine;

namespace UI
{
    public abstract class Presenter : MonoBehaviour
    {
        public virtual void Init(){}
        public virtual void OnShow(){}
        public virtual void OnHide(){}
        public virtual void SetData<TData>(WindowData data) where TData : WindowData { } 
    }

    public class WindowData { }
}