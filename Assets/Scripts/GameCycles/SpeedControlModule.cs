using System;
using UniRx;
using UnityEngine;

public class SpeedControlModule : ABaseModule
{
    public SpeedControlModule(IRuntimeUserData runtimeUserData) : base(runtimeUserData)
    {
    }

    public override void OnStart(IMainGameService gameCycleService)
    {
        base.OnStart(gameCycleService);
        Observable
            .Timer(TimeSpan.FromSeconds(_gameCycleService.GameConfig.SpeedChangeRate))
            .Repeat()
            .Subscribe(_ =>
            {
                float newSpeed = _runtimeData.CurrentSpeed * _gameCycleService.GameConfig.CoeffSpeed;
                _runtimeData.CurrentSpeed = Mathf.Clamp(newSpeed, 0, _gameCycleService.GameConfig.MaxSpeed);
            })
            .AddTo(_compositeDisposable);
    }
}
