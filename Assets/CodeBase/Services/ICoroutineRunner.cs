using System.Collections;
using UnityEngine;

namespace CodeBase.Services
{
    public interface ICoroutineRunner : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}