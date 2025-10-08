using DG.Tweening;
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
    [SerializeField] private RectTransform _sliderRect;
    [SerializeField] private GameObject _animatedRecord;

    private bool _isSliderActive;

    public IObservable<Unit> OnUp => _up.OnClickAsObservable();
    public IObservable<Unit> OnDown => _down.OnClickAsObservable();
    public IObservable<Unit> OnPause => _onPause.OnClickAsObservable();

    public void SetDistance(float current, uint record)
    {
        _currentDistance.text = Mathf.RoundToInt(current).ToString();
        if (_isSliderActive)
        {
            _slider.value = current;
            if (current >= record)
            {
                _isSliderActive = false;
                HideSlider();
            }

        }
        else if (current < record)
        {
            _slider.maxValue = record;
            _slider.value = 0;
            _isSliderActive = true;
            ShowSlider();
        }
    }

    private void HideSlider()
    {
        _sliderRect.DOAnchorPos(new Vector2(0, 300), 0.5f).SetEase(Ease.InBack);
        _animatedRecord.gameObject.SetActive(true);
    }
    private void ShowSlider()
    {
        _sliderRect.DOAnchorPos(new Vector2(0, -125), 0.5f).SetEase(Ease.OutBack);
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
