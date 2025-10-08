using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

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
        _playerHolderService.View.OnTriggered.Subscribe(TriggerHandle).AddTo(_compositeDisposable);
    }

    private void TriggerHandle(Collider2D d)
    {
        if(d.CompareTag("Obstacle"))
        {
            if (!_collisionSafe)
            {
                _runtimeData.CurrentHealth.Value -= 1;
                PlayCollisionSafe();
            }
        }
        if(d.CompareTag("Eat"))
        {
            _runtimeData.CurrentEat.Value += 1;
            d.GetComponent<Obstacle>().gameObject.SetActive(false);
        }
        if(d.CompareTag("largeObstacle"))
        {
            if (!_collisionSafe)
            {
                _runtimeData.CurrentHealth.Value = 0;
            }
        }
    }

    private void PlayCollisionSafe()
    {
        _fadeTween?.Kill(true);
        _collisionSafe = true;
        _fadeTween = DOTween.Sequence();
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(0.1f, 0.25f));
        _fadeTween.AppendInterval(2f);
        _fadeTween.Append(_playerHolderService.View.CanvasGroup.DOFade(1f, 0.25f));
        _fadeTween.OnComplete(() => _collisionSafe = false);
    }
}
