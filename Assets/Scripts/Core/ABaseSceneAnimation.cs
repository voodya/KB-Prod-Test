using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SceneAnimation", fileName = "BaseAnimation")]
public class ABaseSceneAnimation : ScriptableObject
{
    private Dictionary<int, AnimationData> _cachedTweens = new();
    [SerializeField] private float _time = 0.5f;

    public async UniTask AnimateShow(int hash, ABaseScene scene)
    {
        BaseState(hash, scene);
        if (_cachedTweens.ContainsKey(hash))
        {
            _cachedTweens[hash].Tween?.Kill();
        }
        else
            _cachedTweens[hash] = new AnimationData();

            _cachedTweens[hash].Tween = scene.CanvasGroup.DOFade(1f, _time).OnComplete(() => scene.CanvasGroup.blocksRaycasts = true);
        _cachedTweens[hash].Tween.timeScale = 1f;
        await _cachedTweens[hash].Tween.AsyncWaitForCompletion();
    }

    public async UniTask AnimateHide(int hash, ABaseScene scene)
    {
        if (_cachedTweens.ContainsKey(hash))
        {
            _cachedTweens[hash].Tween?.Kill();
        }
        else
            _cachedTweens[hash] = new AnimationData();

        _cachedTweens[hash].Tween = scene.CanvasGroup.DOFade(0f, _time).OnComplete(() => scene.CanvasGroup.blocksRaycasts = false);
        await _cachedTweens[hash].Tween.AsyncWaitForCompletion();
        
    }

    public void BaseState(int hash, ABaseScene scene)
    {
        if (_cachedTweens.ContainsKey(hash))
        {
            _cachedTweens[hash].Tween?.Kill();
        }
        else
            _cachedTweens[hash] = new AnimationData();

        scene.CanvasGroup.alpha = 0f;
        scene.CanvasGroup.blocksRaycasts = false;

    }

}

public class AnimationData
{
    public Tween Tween;
    public CancellationToken CancellationToken;
}

