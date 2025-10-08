using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class PlayerEventModule : ABaseModule
{
    private IPlayerHolderService _playerHolderService;
    private bool _collisionSafe;
    private Sequence _fadeTween;
    public PlayerEventModule(IRuntimeUserData runtimeUserData, IPlayerHolderService playerHolderService) : base(runtimeUserData)
    {
        _playerHolderService = playerHolderService;
    }

    public override void OnStart(IMainGameService gameCycleService)
    {
        base.OnStart(gameCycleService);
        PlayCollisionSafe();
        _playerHolderService.View.OnTriggered.Subscribe(TriggerHandle).AddTo(_compositeDisposable);
    }

    private void TriggerHandle(Collider2D d)
    {

        if (d.TryGetComponent(out Obstacle obs))
        {

            switch (obs.ObstacleType)
            {
                case EObstacleType.Eat:
                    _runtimeData.CurrentEat.Value += (uint)obs.Points;
                    obs.gameObject.SetActive(false);
                    break;
                case EObstacleType.FatalObstacle:
                    if (!_collisionSafe)
                    {
                        _runtimeData.CurrentHealth.Value = 0;
                    }
                    break;
                case EObstacleType.DefaultObstacle:
                    if (!_collisionSafe)
                    {
                        _runtimeData.CurrentHealth.Value -= obs.Points;
                        PlayCollisionSafe();
                    }
                    break;
            }

        }
    }

    private void PlayCollisionSafe()
    {
        _fadeTween?.Kill(true);
        _collisionSafe = true;
        _fadeTween = DOTween.Sequence();
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(0.1f, 0.2f));
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(1f, 0.2f));
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(0.1f, 0.2f));
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(1f, 0.2f));
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(0.1f, 0.2f));
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(1f, 0.2f));
        _fadeTween.OnComplete(() => _collisionSafe = false);
    }
}
