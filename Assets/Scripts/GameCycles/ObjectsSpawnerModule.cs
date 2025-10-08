using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ObjectsSpawnerModule : ABaseModule
{
    private Obstacle _obstacle;
    private List<Sprite> _obstacleViews;
    private List<Sprite> _eatViews;
    private List<Sprite> _largeObstacleViews;
    private List<Obstacle> _generated;
    private Obstacle _eat;
    private Obstacle _largeObstacle;
    private IPoolingService _poolingService;
    public ObjectsSpawnerModule(IRuntimeUserData runtimeUserData,
        IPoolingService poolingService,
        Obstacle obstacle,
        Obstacle eat,
        Obstacle largeObstacle) : base(runtimeUserData)
    {
        _poolingService = poolingService;
        _obstacle = obstacle;
        _eat = eat;
        _largeObstacle = largeObstacle;
    }

    public override void OnStart(IMainGameService gameCycleService)
    {
        base.OnStart(gameCycleService);
        _generated = new List<Obstacle>();
        _poolingService.Register(_obstacle);
        _poolingService.Register(_eat);
        _poolingService.Register(_largeObstacle);
        _runtimeData.Grid.Subscribe(GenerateObjects).AddTo(_compositeDisposable);
    }

    private void GenerateObjects(bool[] column)
    {
        if (column == null) return;
        for (int i = 0; i < column.Length; i++)
        {
            if (column[i])
            {
                if(i+1 < column.Length && column[i+1])
                {
                    var newObj = _poolingService.GetObject(_largeObstacle);
                    newObj.transform.SetParent(_runtimeData.RoadRects[i]);
                    newObj.gameObject.SetActive(true);
                    _generated.Add(newObj);
                    newObj.Rect.anchoredPosition = Vector2.right * _gameCycleService.GamePlayScene.GameField.rect.width * 2;

                }
                else
                {
                    var newObj = _poolingService.GetObject(_obstacle);
                    newObj.gameObject.SetActive(true);
                    newObj.transform.SetParent(_runtimeData.RoadRects[i]);
                    _generated.Add(newObj);
                    newObj.Rect.anchoredPosition = Vector2.right * _gameCycleService.GamePlayScene.GameField.rect.width * 2;
                }
                
            }
            else
            {
                if(UnityEngine.Random.Range(0, 100) > 65)
                {
                    var newObj = _poolingService.GetObject(_eat);
                    newObj.transform.SetParent(_runtimeData.RoadRects[i]);
                    newObj.gameObject.SetActive(true);
                    _generated.Add(newObj);
                    newObj.Rect.anchoredPosition = Vector2.right * _gameCycleService.GamePlayScene.GameField.rect.width * 2;

                }
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        for (int i = 0; i < _generated.Count; i++)
        {
            if (_generated[i].transform.position.x < -50)
            {
                _poolingService.ReleaseObject(_generated[i], _obstacle.name);
                _generated.Remove(_generated[i]);
                continue;
            }

            _generated[i]?.Rect?.Translate(Vector3.left * _runtimeData.CurrentSpeed * Time.deltaTime);
        }
    }

    public override void OnEnd()
    {
        base.OnEnd();
        if(_generated != null)
        for (int i = 0; i < _generated.Count; i++)
        {
            _poolingService.ReleaseObject(_generated[i], _obstacle.name);
            _generated.Remove(_generated[i]);
        }
    }
}
