using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

public interface IPlayerHolderService : IBootable
{
    PlayerView View { get; }

    void SetPlayerToRect(RectTransform parent, Vector2 position);
}


public class PlayerHolderService : IPlayerHolderService
{
    private PlayerView _playerPfb;
    private PlayerView _playerView;

    public PlayerView View => _playerView;

    [Inject]
    public PlayerHolderService(PlayerView playerPfb)
    {
        _playerPfb = playerPfb;
    }

    public int Priority => 10;

    public async UniTask Boot()
    {
        _playerView = MonoBehaviour.Instantiate(_playerPfb);
        _playerView.Rect.localScale = Vector2.zero;
        _playerView.Rect.position = Vector3.zero;
        await UniTask.Yield();
    }

    public void SetPlayerToRect(RectTransform parent, Vector2 position)
    {
        _playerView.Rect.SetParent(parent);
        _playerView.Rect.DOAnchorPos(position, 0.5f);
        _playerView.Rect.DOScale(1, 0.5f);
    }
}
