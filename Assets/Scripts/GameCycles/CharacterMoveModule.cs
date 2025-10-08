using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

/// <summary>
/// Move character view
/// </summary>
public class CharacterMoveModule : ABaseModule
{
    private Tween _moveTween;

    public CharacterMoveModule(IRuntimeUserData runtimeUserData) : base(runtimeUserData)
    {
    }


    public override void OnStart(IMainGameService gameCycleService)
    {
        base.OnStart(gameCycleService);
        _runtimeData.PlayerPosition.Subscribe(MovePlayer).AddTo(_compositeDisposable);
    }

    private void MovePlayer(int obj)
    {
        if (_runtimeData.RoadPositions.Count == 0) return;
        var targetRect = _runtimeData.RoadRects[obj];
        _gameCycleService.GamePlayScene.PlayerHolder.SetParent(targetRect);
        _moveTween?.Kill();
        _moveTween = _gameCycleService.GamePlayScene.PlayerHolder.DOAnchorPos(new Vector2(50, 25), 0.5f);
    }
}
