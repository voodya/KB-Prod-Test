using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScene : ABaseScene
{
    [SerializeField] private Button _up;
    [SerializeField] private Button _down;
    [SerializeField] private Button _onPause;
    [SerializeField] private TextMeshProUGUI _currentDistance;
    [SerializeField] private TextMeshProUGUI _currentEat;
    [SerializeField] private List<Image> _hearts;
    [SerializeField] private Slider _slider;

    public IObservable<Unit> OnUp => _up.OnClickAsObservable();
    public IObservable<Unit> OnDown => _down.OnClickAsObservable();
    public IObservable<Unit> OnPause => _onPause.OnClickAsObservable();

    public void SetDistance(float current, uint record)
    {
        _currentDistance.text = Mathf.RoundToInt(current).ToString();
        if(current < record)
        {
            _slider.gameObject.SetActive(true);
            _slider.maxValue = record;
            _slider.value = current;
        }
        else
        {
            _slider.gameObject.SetActive(false);
        }

    }

    public void SetEat(uint current)
    {
        _currentEat.text = current.ToString();
    }

    public void SetHearts(uint current)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].gameObject.SetActive(i < current);
        }
    }
}
