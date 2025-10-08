using UnityEngine;

public class GamePlayScene : ABaseScene
{
    [SerializeField] private RectTransform _gameField;
    [SerializeField] private RectTransform _playerHolder;
    [SerializeField] private RectTransform _objectHolder;

    public RectTransform PlayerHolder => _playerHolder;
    public RectTransform GameField => _gameField;
    public RectTransform ObjectsHolder => _objectHolder;
}
