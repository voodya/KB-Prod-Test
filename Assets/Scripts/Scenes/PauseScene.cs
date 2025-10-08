using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PauseScene : ABaseScene
{
    [SerializeField] private Button _onReturn;
    [SerializeField] private Button _onMenu;
    [SerializeField] private Button _onBG;
    [SerializeField] private Button _onContinue;

    public IObservable<Unit> OnReturn => _onReturn.OnClickAsObservable()
        .Merge(_onBG.OnClickAsObservable())
        .Merge(_onContinue.OnClickAsObservable());
    public IObservable<Unit> OnMenu => _onMenu.OnClickAsObservable();
}
