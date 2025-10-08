using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SceneAnimations/Scale", fileName = "ScaleAnimation")]
public class ScaleSceneAnim : ABaseSceneAnimation
{
    [SerializeField] private float _startScale = 1.5f;
    [SerializeField] private float _targetScale = 1f;

    public override async UniTask AnimateHide(int hash, ABaseScene scene)
    {
        Validate(hash);
        _cachedTweens[hash].Tween = scene.CanvasRect.DOScale(_startScale, _time);
        await _cachedTweens[hash].Tween.AsyncWaitForCompletion();
    }

    public override async UniTask AnimateShow(int hash, ABaseScene scene)
    {
        BaseState(hash, scene);
        _cachedTweens[hash].Tween = scene.CanvasRect.DOScale(_targetScale, _time);
        await _cachedTweens[hash].Tween.AsyncWaitForCompletion();
    }

    public override void BaseState(int hash, ABaseScene scene)
    {
        Validate(hash);
        scene.CanvasGroup.blocksRaycasts = false;
        scene.CanvasRect.localScale = new Vector3(_startScale, _startScale, _startScale);

    }
}
