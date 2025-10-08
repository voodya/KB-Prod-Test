using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : ABaseScene
{
    [SerializeField] private Button _play;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _testNotifi;
    [SerializeField] private RectTransform _playerHolder;

    [SerializeField] private TextMeshProUGUI _recordDistanceText;
    [SerializeField] private TextMeshProUGUI _recordEatText;

    public IObservable<Unit> OnPlay => _play.OnClickAsObservable();
    public IObservable<Unit> OnSettings => _settings.OnClickAsObservable();
    public IObservable<Unit> OnTestNoitifi => _testNotifi.OnClickAsObservable();
    public RectTransform PlayerHolder => _playerHolder;

    public override void OnShowed()
    {
        base.OnShowed();
    }

    public void ShowUserData(UserData data)
    {
        _recordDistanceText.text = $"Distance record: {data.DistanceRecord}";
        _recordEatText.text = $"Eat record: {data.EatRecord}";
    }
}
