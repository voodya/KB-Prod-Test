using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SceneAnimations/Fade", fileName = "FadeAnimation")]
public class FadeSceneAnim : ABaseSceneAnimation
{
    public override async UniTask AnimateShow(int hash, ABaseScene scene)
    {
        BaseState(hash, scene);
        _cachedTweens[hash].Tween = scene.CanvasGroup.DOFade(1f, _time).OnComplete(() => scene.CanvasGroup.blocksRaycasts = true);
        await _cachedTweens[hash].Tween.AsyncWaitForCompletion();
    }

    public override async UniTask AnimateHide(int hash, ABaseScene scene)
    {
        Validate(hash);
        _cachedTweens[hash].Tween = scene.CanvasGroup.DOFade(0f, _time).OnComplete(() => scene.CanvasGroup.blocksRaycasts = false);
        await _cachedTweens[hash].Tween.AsyncWaitForCompletion();
    }

    public override void BaseState(int hash, ABaseScene scene)
    {
        Validate(hash);
        scene.CanvasGroup.alpha = 0f;
        scene.CanvasGroup.blocksRaycasts = false;
    }
}
