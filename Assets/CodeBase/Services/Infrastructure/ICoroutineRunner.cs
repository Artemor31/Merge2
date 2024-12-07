using System.Collections;
using UnityEngine;

namespace Services
{
    public interface ICoroutineRunner : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}