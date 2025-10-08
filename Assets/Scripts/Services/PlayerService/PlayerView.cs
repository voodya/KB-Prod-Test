using System;
using UniRx.Triggers;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private CanvasGroup _canvasGroup;

    public IObservable<Collider2D> OnTriggered => _collider.OnTriggerEnter2DAsObservable();
    public CanvasGroup CanvasGroup => _canvasGroup;
    public RectTransform Rect => _rect;
}
