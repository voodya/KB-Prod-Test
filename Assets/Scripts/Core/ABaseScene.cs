using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CanvasGroup))]
public class ABaseScene : MonoBehaviour, IDisposable
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private ABaseSceneAnimation _sceneAnimation;

    private bool _isShowed;
    private CompositeDisposable _compositeDisposable;
    private int _hash;

    public CanvasGroup CanvasGroup => _canvasGroup;
    public RectTransform CanvasRect => _canvasRect;
    public CompositeDisposable OnceSub => _compositeDisposable;
    public bool IsShowed => _isShowed;

    private void OnValidate()
    {
        if(_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasRect == null)
            _canvasRect = GetComponent<RectTransform>();
    }

    public virtual async UniTask Show()
    {
        
        if (_hash == 0)
            GenerateHash();
        await _sceneAnimation.AnimateShow(_hash, this);
        _isShowed = true;
        OnShowed();
    }

    public virtual async UniTask Hide()
    {
        //_compositeDisposable?.Dispose();
        if (_hash == 0)
            GenerateHash();
        await _sceneAnimation.AnimateHide(_hash, this);
        _isShowed = false;
    }

    public virtual void SetDefaultState()
    {
        _sceneAnimation.BaseState(_hash, this);
    }

    private void GenerateHash()
    {
        _hash = this.GetHashCode();
    }

    public virtual void OnShowed()
    {

    }

    public virtual void Dispose()
    {
        _compositeDisposable?.Dispose();
    }

    public void ReloadDispose()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
    }
}
