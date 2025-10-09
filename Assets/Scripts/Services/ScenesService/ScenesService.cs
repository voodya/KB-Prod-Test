using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public interface IScenesService
{
    UniTask<T> GetScene<T>(bool hide = false, Action<T> OnShowed = null, Action<T> beforeShow = null) where T : ABaseScene;
    void Register<T>(AssetReference reference) where T : ABaseScene;
    UniTask ReleaseScene<T>(Action<T> BeforeRelease = null) where T : ABaseScene;
}


public class ScenesService : IScenesService
{
    private Dictionary<Type, AssetReference> _references = new();
    private readonly Dictionary<Type, (ABaseScene, AsyncOperationHandle<SceneInstance>)> _loadedScenes = new();

    public void Register<T>(AssetReference reference) where T : ABaseScene
    {
        Type type = typeof(T);
        if (reference.AssetGUID == "")
        {
            Debug.LogError($"{type} AssetReference is null!");
            return;
        }
        if (!_references.ContainsKey(type))
            _references.Add(type, reference);
        else
        {
            Debug.LogError($"{type} already registred");
        }
    }

    public async UniTask<T> GetScene<T>(bool hide = false, Action<T> OnShowed = null, Action<T> beforeShow = null)
            where T : ABaseScene
    {
        Type screenType = typeof(T);
        Debug.Log($"ALERT_LOAD_SCENE => {screenType}");
        if (_loadedScenes.ContainsKey(screenType))
        {
            T scene = _loadedScenes[screenType].Item1 as T;

            beforeShow?.Invoke(scene);
            if (!hide)
            {
                if (!scene.IsShowed)
                {
                    scene.ReloadDispose();
                    await scene.Show();
                }
                OnShowed?.Invoke(scene);
            }
            else
                scene.SetDefaultState();
            return scene;
        }
        else
        {
            AsyncOperationHandle<SceneInstance> loadingOperation = Addressables.LoadSceneAsync(_references[screenType], LoadSceneMode.Additive);

            T scene = (await loadingOperation).Scene.GetRoot<T>();
            scene.ReloadDispose();
            beforeShow?.Invoke(scene);
            _loadedScenes[screenType] = (scene, loadingOperation);
            if (!hide)
            {
                if (!scene.IsShowed)
                {
                    scene.ReloadDispose();
                    await scene.Show();
                }
                OnShowed?.Invoke(scene);
            }
            else
                scene.SetDefaultState();
            return scene;
        }
    }

    public async UniTask ReleaseScene<T>(Action<T> BeforeRelease = null)
            where T : ABaseScene
    {
        Type screenType = typeof(T);
        Debug.Log($"ALERT_RELEASE_SCENE => {screenType}");
        if (_loadedScenes.ContainsKey(screenType))
        {
            BeforeRelease?.Invoke(_loadedScenes[screenType].Item1 as T);
            await _loadedScenes[screenType].Item1.Hide();
            _loadedScenes[screenType].Item1.Dispose();
            await Addressables.UnloadSceneAsync(_loadedScenes[screenType].Item2);
            _loadedScenes.Remove(screenType);
        }
        else
            Debug.LogError($"No instance scene {screenType}");

    }
}
