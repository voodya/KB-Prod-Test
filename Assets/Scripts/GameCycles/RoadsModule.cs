using DG.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RoadsModule : ABaseModule
{
    private uint _roadCount;
    private RectTransform _roadRectPfb;

    public RoadsModule(IRuntimeUserData runtimeUserData, RectTransform roadRect) : base(runtimeUserData)
    {
        _roadRectPfb = roadRect;
    }
    public override void OnStart(IMainGameService gameCycleService)
    {
        base.OnStart(gameCycleService);
        _gameCycleService = gameCycleService;
        _roadCount = _gameCycleService.GameConfig.StartRowsCount;
        CreateRoads();
        _runtimeData.PlayerPosition.Value = 1;
        Observable.Timer(TimeSpan.FromSeconds(_gameCycleService.GameConfig.AddRowsRate))
            .Repeat()
            .Take((int)_gameCycleService.GameConfig.MaxRowsCount - (int)_gameCycleService.GameConfig.StartRowsCount)
            .Subscribe(_ => AddRoad())
            .AddTo(_compositeDisposable);
    }

    private void CreateRoads()
    {
        RectTransform gameZone = _gameCycleService.GamePlayScene.GameField;
        uint targetRoadCount = _roadCount;
        int currentRoadCount = _runtimeData.RoadPositions.Count;
        var height = gameZone.rect.height;
        var roadHeight = height / targetRoadCount;
        var roadPoseOffset = roadHeight / 2f;

        var roadPositions = new List<Vector2>();


        for (int i = 0; i < _roadCount; i++)
        {
            var pose = new Vector2(50, roadPoseOffset + (roadHeight * i));

            roadPositions.Add(pose);
        }
        _runtimeData.RoadPositions = roadPositions;

        for (int i = 0; i < _runtimeData.RoadPositions.Count; i++)
        {
            if (_runtimeData.RoadRects.Count > i)
                _runtimeData.RoadRects[i].DOAnchorPos(_runtimeData.RoadPositions[i], 0.5f);
            else
            {
                var obj = MonoBehaviour.Instantiate(_roadRectPfb, _gameCycleService.GamePlayScene.GameField);
                obj.anchoredPosition = _runtimeData.RoadPositions[i] + Vector2.up * 100;
                _runtimeData.RoadRects.Add(obj);
                _runtimeData.RoadRects[i].DOAnchorPos(_runtimeData.RoadPositions[i], 0.5f);
            }
        }
    }

    private void AddRoad()
    {
        _roadCount += 1;
        CreateRoads();
    }
}
