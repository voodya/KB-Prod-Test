using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABaseSceneAnimation : ScriptableObject
{
    protected Dictionary<int, AnimationData> _cachedTweens = new();
    [SerializeField] protected float _time = 0.5f;

    public abstract UniTask AnimateShow(int hash, ABaseScene scene);

    public abstract UniTask AnimateHide(int hash, ABaseScene scene);

    public abstract void BaseState(int hash, ABaseScene scene);

    protected void Validate(int hash)
    {
        if (_cachedTweens.ContainsKey(hash))
        {
            _cachedTweens[hash].Tween?.Kill();
        }
        else
            _cachedTweens[hash] = new AnimationData();
    }

}

public class AnimationData
{
    public Tween Tween;
}

