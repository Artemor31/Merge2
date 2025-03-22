using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Services.Infrastructure
{
    public class SceneLoader : IService
    {
        public event Action OnSceneChanged;
        private readonly ICoroutineRunner _coroutineRunner;
        
        public SceneLoader(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        public void Load(string name, LoadSceneMode mode = LoadSceneMode.Single, Action then = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, mode, then));

        public void Unload(string name, Action onUnloaded = null) =>
            _coroutineRunner.StartCoroutine(UnloadScene(name, onUnloaded));

        private IEnumerator LoadScene(string nextScene, LoadSceneMode mode, Action onLoaded = null)
        {
            var operation = SceneManager.LoadSceneAsync(nextScene, mode);
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