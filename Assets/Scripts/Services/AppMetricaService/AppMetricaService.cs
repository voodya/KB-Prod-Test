using Cysharp.Threading.Tasks;
using Io.AppMetrica;
using Newtonsoft.Json;
using System.Collections.Generic;

public interface IAppMetricaService : IBootable
{
    void EndGame(uint currenDist, uint currentEat);
    void LaunchGame(uint playId);
}


public class AppMetricaService : IAppMetricaService
{

    private ISaveService _saveService;
    private string _metricSaveKey = "testkey";
    private string _apiKey;
    private MetricData _data;

    private const string DISTANCE_CURRENT = "DistCur";
    private const string DISTANCE_RECORD = "DistRec";
    private const string EAT_CURRENT = "EatCur";
    private const string EAT_RECORD = "EatRec";
    private const string SESSION_ID = "Session";
    private const string PLAY_ID = "Play";

    public int Priority => 200;

    public AppMetricaService(ISaveService saveService, string apiKey)
    {
        _saveService = saveService;
        _apiKey = apiKey;
    }

    public async UniTask Boot()
    {
        AppMetrica.Activate(new AppMetricaConfig(_apiKey)
        {
            FirstActivationAsUpdate = !IsFirstLaunch(),
        });
        LaunchApp();
    }

    private bool IsFirstLaunch()
    {
        if (!_saveService.TryGetData(_metricSaveKey, out _data))
        {
            _data = new MetricData();
            _saveService.SetData(_metricSaveKey, _data);
        }
        return _data.IsFirstLaunch;
    }

    private void LaunchApp()
    {
        UserData data;
        if (!_saveService.TryGetData(nameof(UserData), out data))
        {
            data = new UserData();
            data.SessionId++;
            _saveService.SetData(nameof(UserData), data);
        }

        Dictionary<string, uint> eventInfo = new Dictionary<string, uint>()
        {
            { SESSION_ID, data.SessionId },
            { DISTANCE_RECORD, data.DistanceRecord },
            { EAT_RECORD, data.EatRecord }
        };

        string eventParameters = JsonConvert.SerializeObject(eventInfo);
        AppMetrica.ReportEvent("Launch", eventParameters);
    }

    public void LaunchGame(uint playId)
    {
        UserData data;
        if (!_saveService.TryGetData(nameof(UserData), out data))
        {
            data = new UserData();
            _saveService.SetData(nameof(UserData), data);
        }

        Dictionary<string, uint> eventInfo = new Dictionary<string, uint>()
        {
            { SESSION_ID, data.SessionId },
            { DISTANCE_RECORD, data.DistanceRecord },
            { EAT_RECORD, data.EatRecord },
            { PLAY_ID, playId }
        };

        string eventParameters = JsonConvert.SerializeObject(eventInfo);
        AppMetrica.ReportEvent("LaunchGame", eventParameters);
    }

    public void EndGame(uint currenDist, uint currentEat)
    {
        UserData data;
        if (!_saveService.TryGetData(nameof(UserData), out data))
        {
            data = new UserData();
            _saveService.SetData(nameof(UserData), data);
        }

        Dictionary<string, uint> eventInfo = new Dictionary<string, uint>()
        {
            { SESSION_ID, data.SessionId },
            { DISTANCE_RECORD, data.DistanceRecord },
            { EAT_RECORD, data.EatRecord },
            { DISTANCE_CURRENT, currenDist },
            { EAT_CURRENT, currentEat }
        };

        string eventParameters = JsonConvert.SerializeObject(eventInfo);
        AppMetrica.ReportEvent("FinishGame", eventParameters);
    }
}

[System.Serializable]
public class MetricData
{
    public string UserId;
    public bool IsFirstLaunch;

    public MetricData()
    {
        IsFirstLaunch = true;
    }

}

