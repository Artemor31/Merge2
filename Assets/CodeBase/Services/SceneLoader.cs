using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CodeBase.Services
{
    public class SceneLoader : IService
    {
        public event Action OnSceneChanged; 
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action then = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, then));

        public void Unload(string name, Action onUnloaded = null) =>
            _coroutineRunner.StartCoroutine(UnloadScene(name, onUnloaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            var operation = SceneManager.LoadSceneAsync(nextScene);
            while (!operation.isDone)
            {
                yield return null;
            }
            
            OnSceneChanged?.Invoke();
            onLoaded?.Invoke();
        }

        private IEnumerator UnloadScene(string name, Action onUnloaded)
        {
            var unloadSceneAsync = SceneManager.UnloadSceneAsync(name);

            while (unloadSceneAsync.isDone)
            {
                onUnloaded?.Invoke();
                yield break;
            }

            unloadSceneAsync.completed += x => onUnloaded?.Invoke();
        }
    }
}