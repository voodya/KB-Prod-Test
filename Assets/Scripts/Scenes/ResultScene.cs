using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultScene : ABaseScene
{
    [SerializeField] private Button _onReplay;
    [SerializeField] private Button _onMenu;
    [SerializeField] private TextMeshProUGUI _eatResult;
    [SerializeField] private GameObject _eatRecord;
    [SerializeField] private TextMeshProUGUI _distanceResult;
    [SerializeField] private GameObject _distanceRecord;

    public IObservable<Unit> OnReplay => _onReplay.OnClickAsObservable();
    public IObservable<Unit> OnMenu => _onMenu.OnClickAsObservable();

    public void SetData(float currentDistance, uint recordDistance, uint currentEat, uint recordEat)
    {
        _eatResult.text = currentEat.ToString();
        _eatRecord.SetActive(currentEat > recordEat);
        _distanceResult.text = ((int)currentDistance).ToString();
        _distanceRecord.SetActive(currentDistance > recordDistance);
    }
}
