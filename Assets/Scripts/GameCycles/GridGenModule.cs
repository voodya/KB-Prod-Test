using System.Linq;
using UniRx;
using UnityEngine;

/// <summary>
/// Step by step generate game grid
/// </summary>
public class GridGenModule : ABaseModule
{
    private bool[] _reachable;
    private float _currentDistanceStep;

    public GridGenModule(IRuntimeUserData runtimeUserData) : base(runtimeUserData)
    {
    }

    public override void OnStart(IMainGameService gameCycleService)
    {
        base.OnStart(gameCycleService);
        _currentDistanceStep = 0;
        _gameCycleService = gameCycleService;
        _reachable = Enumerable.Repeat(false, 3).ToArray();
    }

    private void GenerateNewColumn()
    {
        bool[] bools = new bool[_runtimeData.RoadPositions.Count];
        if (_reachable.Length < _runtimeData.RoadPositions.Count)
        {

            for (int i = 0; i < _runtimeData.RoadPositions.Count; i++)
            {
                if (_reachable.Length > i)
                    bools[i] = _reachable[i];
                else
                    bools[i] = true;
            }

        }
        else
        {
            bools = _reachable;
        }

        bool isGood = false;
        bool[] newColumn = new bool[_runtimeData.RoadPositions.Count];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < _runtimeData.RoadPositions.Count; j++)
            {
                newColumn[j] = UnityEngine.Random.Range(0, 2) == 1;
            }
            int countGood = 0;
            for (int j = 0; j < _runtimeData.RoadPositions.Count; j++)
            {
                if (newColumn[j] == false && bools[j] == false)
                    countGood++;
            }
            if (countGood > 1)
            {
                isGood = true;
                break;
            }
        }
        if (!isGood)
            newColumn = Enumerable.Repeat(false, _runtimeData.RoadPositions.Count).ToArray();

        _runtimeData.Grid.Value = newColumn;
    }

    public override void OnUpdate()
    {
        _currentDistanceStep += _runtimeData.CurrentSpeed * Time.deltaTime;
        if (_currentDistanceStep >= _gameCycleService.GameConfig.ObjectSpawnRate)
        {
            GenerateNewColumn();
            _currentDistanceStep = 0;
        }
    }
}
