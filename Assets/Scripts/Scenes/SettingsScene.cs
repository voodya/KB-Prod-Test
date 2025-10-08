using NUnit.Framework;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScene : ABaseScene
{
    [SerializeField] private Button _onMusic;
    [SerializeField] private Button _onSound;
    [SerializeField] private Button _onClose;
    [SerializeField] private Button _onBG;
    [SerializeField] private GameObject _musicCheck;
    [SerializeField] private GameObject _soundCheck;

    public IObservable<Unit> OnReturn => _onClose.OnClickAsObservable().Merge(_onBG.OnClickAsObservable());
    public IObservable<Unit> OnMusic => _onMusic.OnClickAsObservable();
    public IObservable<Unit> OnSound => _onSound.OnClickAsObservable();

    public void SetData(SettingsData data)
    {
        _musicCheck.SetActive(data.IsMusic);
        _soundCheck.SetActive(data.IsSound);
    }
}
